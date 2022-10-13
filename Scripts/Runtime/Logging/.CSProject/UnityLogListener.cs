using System;
using System.Reflection;
using Anvil.CSharp.Logging;
using UnityEngine;
using StackFrame = System.Diagnostics.StackFrame;
using StackTrace = System.Diagnostics.StackTrace;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Scripting;
using Logger = Anvil.CSharp.Logging.Logger;
using System.Linq;
using System.IO;

namespace Anvil.Unity.Logging
{
    /// <summary>
    /// An implementation of <see cref="ILogListener"/> for the Unity <see cref="Debug"/> logger.
    /// Captures all logging made through <see cref="Debug"/> and redirects them through a <see cref="Logger"/>.
    /// </summary>
    [DefaultLogListener]
    [Preserve]
    public sealed class UnityLogListener : ILogListener, UnityEngine.ILogHandler
    {
        /// <summary>
        /// Place on a method to make the <see cref="UnityLogListener" /> use the next
        /// call up the stack for the log caller context.
        /// </summary>
        [AttributeUsage(AttributeTargets.Method)]
        public class ExcludeAttribute : Attribute { }

        /// <summary>
        /// The context delivered when a context can't be resolved
        /// </summary>
        private sealed class UnknownContext { }

        private readonly struct LogMessage
        {
            public readonly string Message;
            public readonly LogLevel LogLevel;
            public readonly bool IsFromBurstedContext;

            public LogMessage(string message, LogLevel logLevel, bool isFromBurstedContext)
            {
                Message = message;
                LogLevel = logLevel;
                IsFromBurstedContext = isFromBurstedContext;
            }
        }

        // Pumps events off the main thread to indicate that pending logs
        // should be processed.
        // We want to process the queue as often as possible (within reason) so we can handle
        // the logs as close to the time they were emitted as possible.
        [DefaultExecutionOrder(int.MaxValue)]   // Execute last in each phase
        private sealed class PendingLogPump : MonoBehaviour
        {
            public event Action OnProcessPendingLogs;
            private Coroutine m_EndOfFrameRoutine;

            private void Start()
            {
                m_EndOfFrameRoutine = StartCoroutine(RunEndOfFrameRoutine());
            }

            private void FixedUpdate()
            {
                OnProcessPendingLogs?.Invoke();
            }

            private void Update()
            {
                OnProcessPendingLogs?.Invoke();
            }

            private void LateUpdate()
            {
                OnProcessPendingLogs?.Invoke();
            }

            private void OnPreRender()
            {
                OnProcessPendingLogs?.Invoke();
            }

            private void OnPostRender()
            {
                OnProcessPendingLogs?.Invoke();
            }

            private IEnumerator RunEndOfFrameRoutine()
            {
                while (true)
                {
                    yield return new WaitForEndOfFrame();
                    OnProcessPendingLogs?.Invoke();
                }
            }

            private void OnDestroy()
            {
                StopCoroutine(m_EndOfFrameRoutine);

                // This is our last chance to process
                OnProcessPendingLogs?.Invoke();
                OnProcessPendingLogs = null;
            }
        }

        // When determining the context of a log, any frames in the stack trace matching these types are skipped
        private static readonly HashSet<string> SKIPPED_STACK_FRAME_TYPES = new HashSet<string>
        {
            "System.Diagnostics.Debug",
            "System.Diagnostics.Trace",
            "System.Diagnostics.TraceInternal",
            "System.Diagnostics.TraceListener",
            "UnityEngine.Debug",
            "UnityEngine.Logger",
            "UnityEngine.Assertions.Assert",
            // Check for Unity's new stubbed out logging class that currently just proxies the existing logging system.
            // This was found in the v0.17 of the Unity.Entities package
            "Unity.Debug",
        };

        private static readonly string UNITY_DEBUG_ASSEMBLY_STRING = typeof(Debug).Assembly.ToString();

        private static UnityLogListener s_Instance;

        // With the Burst compiler, it's common to disable the Domain Reload step when entering play mode to increase iteration time.
        // https://docs.unity3d.com/Packages/com.unity.entities@0.17/manual/install_setup.html
        // Because of this, statics will preserve state across play sessions.
        // `RuntimeInitializeOnLoadMethod` ensures we reset the state on every play session.
        // `RuntimeInitializeLoadType.SubsystemRegistration` represents the earliest moment of all the entries in the enum.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            //Touch Log system to make sure this listener gets re-created ASAP.
            Log.GetStaticLogger(typeof(UnityLogListener));
        }

        private readonly ConcurrentQueue<LogMessage> m_PendingLogs;

        // The log handler in place before this listener initialized.
        // Called when a log comes through that this listener has already handled.
        private readonly UnityEngine.ILogHandler m_ExistingLogHandler;

        // true when this listener is handling a log from Burst compiled code.
        // Must be specially handled because log calls from Burst compiled code don't pass
        // through the Debug.unityLogger.logHandler. This prevents us from blocking
        // the default message from being emitted to Unity's console.
        //
        // We still want log calls from Burst compiled code to pass through the Log
        // system so that non-unity handlers may handle the message.
        //
        // So, we prevent our decorated log message from emitting to the console
        // using this flag.
        //  - The Log system gets a chance to handle log messages from burst compiled code
        //  - The consumer only sees one message posted to Unity's console.
        //
        // win-win...
        //  Except the poor developers that have to maintain this disaster and adapt it when
        //  Unity inevitably improves logging within a Burst context.
        private bool m_IsHandlingBurstedLog = false;


        public UnityLogListener()
        {
            if (s_Instance != null)
            {
                throw new Exception($"There can only be one instance of the {nameof(UnityLogListener)} and it has already been initialized.");
            }
            s_Instance = this;

            m_PendingLogs = new ConcurrentQueue<LogMessage>();

            m_ExistingLogHandler = Debug.unityLogger.logHandler;
            Debug.unityLogger.logHandler = this;

            Application.logMessageReceivedThreaded += Application_logMessageReceivedThreaded;

            GameObject pendingLogPumpGO = new GameObject($"{nameof(UnityLogListener)}_{nameof(pendingLogPumpGO)}");
            pendingLogPumpGO.AddComponent<PendingLogPump>().OnProcessPendingLogs += PendingLogPump_OnProcessPendingLogs;
            pendingLogPumpGO.hideFlags = HideFlags.DontSave | HideFlags.HideInHierarchy;
        }

        /// <inheritdoc />
        [UnityLogListener.Exclude]
        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            // Bypass redirecting unity log messages to logger if logger is already handling the log.
            // This happens when UnityLogHandler is being used.
            if (Log.IsHandlingLog)
            {
                // If this is a log call from Burst compiled code then skip emitting
                // to Unity's console. Log calls from Burst compiled code don't pass
                // through Debug.unityLogger.logHandler.
                if (m_IsHandlingBurstedLog)
                {
                    return;
                }

                m_ExistingLogHandler.LogFormat(logType, context, format, args);
                return;
            }

            // Make UnityEngine.Debug.Assert failures throw an exception instead of just logging an error
            // NOTE: Skip this function and UnityEngine.Logger (check frame index 2)
            MethodBase assertMethod = new StackFrame(2, fNeedFileInfo: true)?.GetMethod();
            if (assertMethod != null &&
                assertMethod.Name == nameof(Debug.Assert) &&
                assertMethod.DeclaringType.Assembly.ToString() == UNITY_DEBUG_ASSEMBLY_STRING)
            {
                throw new Exception(string.Format(format, args));
            }

            SendToLogger(context, LogTypeToLogLevel(logType), string.Format(format, args));
        }

        /// <inheritdoc />
        [UnityLogListener.Exclude]
        public void LogException(Exception exception, UnityEngine.Object context)
        {
            // Bypass redirecting unity log messages to logger if logger is already handling the log.
            // This happens when UnityLogHandler is being used.
            if (Log.IsHandlingLog)
            {
                // If this is a log call from Burst compiled code then skip emitting
                // to Unity's console. Log calls from Burst compiled code don't pass
                // through Debug.unityLogger.logHandler.
                if (m_IsHandlingBurstedLog)
                {
                    return;
                }

                m_ExistingLogHandler.LogException(exception, context);
                return;
            }

            SendToLogger(context, LogLevel.Error, exception.ToString(), exception);
        }

        private void Application_logMessageReceivedThreaded(string condition, string stackTrace, LogType type)
        {
            if (Log.IsHandlingLog)
            {
                return;
            }

            // Only Burst messages will ever get this far because Unity's default log handler (m_ExistingLogHandler)
            // is the code responsible for dispatching the logMessageReceived(+Threaded) event.
            // So the flows are:
            //
            // From non-burst
            //  - Debug.Log
            //   - UnityLogListener.LogFormat
            //    - UnityLogListener.SendToLogger - because Log.IsHandlingLog == false
            //     - Log.DispatchLog - set Log.IsHandlingLog = true
            //      - UnityLogHandler.HandleLog
            //       - Debug.Log
            //        - UnityLogListener.LogFormat
            //         - m_ExistingLogHandler.LogFormat - because Log.IsHandlingLog == true
            //          - Application.logMessageReceived(+Threaded)
            //           - UnityLogListener.Application_logMessageReceivedThreaded - Do nothing because Log.IsHandling == true
            //          - m_ExistingLogHandler emits log to the Unity console
            //
            // From burst
            //  - Debug.Log
            //   - Application.logMessageReceived(+Threaded)
            //    - UnityLogListener.Application_logMessageReceivedThreaded - set m_IsHandlingBurstedLog = true
            //     - UnityLogListener.SendToLogger - because Log.IsHandlingLog == false
            //      - Log.DispatchLog - set Log.IsHandlingLog = true
            //       - UnityLogHandler.HandleLog
            //        - Debug.Log
            //         - UnityLogListener.LogFormat - Do nothing because m_IsHandlingBurstedLog = true
            //  - Something external (BurstCompilerService.Log?) emits the log to the Unity console
            //
            // From a Logger instance
            //  - Logger.Debug
            //   - Log.DispatchLog - set Log.IsHandlingLog = true
            //    - UnityLogHandler.HandleLog
            //     - Debug.Log
            //      - UnityLogListener.LogFormat
            //       - m_ExistingLogHandler.LogFormat - because Log.IsHandlingLog == true
            //        - Application.logMessageReceived(+Threaded)
            //         - UnityLogListener.Application_logMessageReceivedThreaded - Do nothing because Log.IsHandling == true
            //        - m_ExistingLogHandler emits log to the Unity console
            m_PendingLogs.Enqueue(new LogMessage(condition, LogTypeToLogLevel(type), true));
        }

        private void PendingLogPump_OnProcessPendingLogs()
        {
            ProcessPendingLogs();
        }

        private void ProcessPendingLogs()
        {
            while (m_PendingLogs.TryDequeue(out LogMessage message))
            {
                m_IsHandlingBurstedLog = message.IsFromBurstedContext;
                SendToLogger(null, message.LogLevel, message.Message);
            }
            m_IsHandlingBurstedLog = false;
        }

        private void SendToLogger(UnityEngine.Object context, LogLevel logLevel, string message, Exception exception = null)
        {
            (StackFrame callerFrame, MethodBase callerMethod) = ResolveCaller(exception);

            string callerPath = callerFrame?.GetFileName() ?? callerMethod?.ReflectedType.Name;

            Logger logger = context != null ? Log.GetLogger(context) : Log.GetStaticLogger(ResolveContextFromMethod(callerMethod));
            logger.AtLevel(logLevel, message, callerPath, callerMethod?.Name, callerFrame?.GetFileLineNumber() ?? 0);
        }

        private (StackFrame callerFrame, MethodBase callerMethod) ResolveCaller(Exception exception = null)
        {
            StackTrace stackTrace = (exception == null ? null : new StackTrace(exception, fNeedFileInfo: true));
            // If no explicit stack trace is given, skip 4 frames (GetNextFrame, ResolveCaller, SendToLogger, and LogFormat/LogException)
            int frameIndex = (exception == null ? 4 : 0);

            StackFrame frame;
            MethodBase method;
            while ((frame = GetNextFrame()) != null && (method = frame.GetMethod()) != null)
            {
                if (method.GetCustomAttribute<ExcludeAttribute>(inherit: true) != null)
                {
                    continue;
                }

                if (SKIPPED_STACK_FRAME_TYPES.Contains(method.DeclaringType.FullName))
                {
                    continue;
                }

                return (frame, method);
            }

            return (null, null);

            StackFrame GetNextFrame()
            {
                // Get each frame with `new StackFrame()` for performance reasons; using `new StackTrace()` to get all
                // frames at once can be significantly more expensive, especially for long call stacks.
                StackFrame nextFrame = (stackTrace != null
                    ? stackTrace.GetFrame(frameIndex)
                    : new StackFrame(frameIndex, fNeedFileInfo: true));

                ++frameIndex;
                return nextFrame;
            }
        }


        private Type ResolveContextFromMethod(MethodBase callerMethod)
        {
            return callerMethod != null ? callerMethod.ReflectedType : typeof(UnknownContext);
        }

        private LogLevel LogTypeToLogLevel(LogType logType)
        {
            switch (logType)
            {
                case LogType.Assert:
                case LogType.Exception:
                case LogType.Error:
                    return LogLevel.Error;

                case LogType.Warning:
                    return LogLevel.Warning;

                case LogType.Log:
                    return LogLevel.Debug;

                default:
                    throw new ArgumentException("Unknown log type: " + logType);
            }
        }
    }
}
using System;
using System.Reflection;
using Anvil.CSharp.Logging;
using UnityEngine;
using StackFrame = System.Diagnostics.StackFrame;
using System.Collections.Concurrent;

namespace Anvil.Unity.Logging
{
    /// <summary>
    /// An implementation of <see cref="ILogListener"/> for the Unity <see cref="Debug"/> logger.
    /// Captures all logging made through <see cref="Debug"/> and redirects them through a <see cref="Log.Logger"/>.
    /// </summary>
    [DefaultLogListener]
    public sealed class UnityLogListener : ILogListener, UnityEngine.ILogHandler
    {
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

        private class LateUpdatePump : MonoBehaviour
        {
            public event Action OnLateUpdate;

            private void LateUpdate()
            {
                OnLateUpdate?.Invoke();
            }
        }

        private static UnityLogListener s_Instance;

        // With the Burst compiler, it's common to disable the Domain Reload step when entering play mode to increase iteration time.
        // https://docs.unity3d.com/Packages/com.unity.entities@0.17/manual/install_setup.html
        // Because of this, statics will preserve state across play sessions.
        // `RuntimeInitializeOnLoadMethod` ensures we reset the state on every play session.
        // `RuntimeInitializeLoadType.SubsystemRegistration` represents the earliest moment of all the entries in the enum.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            s_Instance = null;
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
            Debug.Assert(s_Instance == null, $"There can only be one instance of the {nameof(UnityLogListener)} and it has already been initialized.");
            s_Instance = this;

            m_PendingLogs = new ConcurrentQueue<LogMessage>();

            m_ExistingLogHandler = Debug.unityLogger.logHandler;
            Debug.unityLogger.logHandler = this;

            Application.logMessageReceivedThreaded += Application_logMessageReceivedThreaded;

            GameObject lateUpdatePumpGO = new GameObject($"{nameof(UnityLogListener)}_{nameof(lateUpdatePumpGO)}");
            lateUpdatePumpGO.AddComponent<LateUpdatePump>().OnLateUpdate += LateUpdatePump_OnLateUpdate;
            lateUpdatePumpGO.hideFlags = HideFlags.DontSave|HideFlags.HideInHierarchy;
        }

        /// <inheritdoc />
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

            SendToLogger(context, LogTypeToLogLevel(logType), string.Format(format, args));
        }

        /// <inheritdoc />
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

            SendToLogger(context, LogLevel.Error, exception.ToString());
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
            //  - Someting external (BurstCompilerService.Log?) emits the log to the Unity console
            //      
            // From a Log.Logger instance
            //  - Log.Logger.Debug
            //   - Log.DispatchLog - set Log.IsHandlingLog = true
            //    - UnityLogHandler.HandleLog
            //     - Debug.Log
            //      - UnityLogListener.LogFormat
            //       - m_ExistingLogHandler.LogFormat - because Log.IsHandlingLog == true
            //        - Application.logMessageReceived(+Threaded)
            //         - UnityLogListener.Application_logMessageReceivedThreaded - Do nothing because Log.IsHandling == true
            //        - m_ExistingLogHandler emits log to the Unity console
            m_PendingLogs.Enqueue(new LogMessage(condition, LogTypeToLogLevel(type), true));
            //m_IsHandlingBurstedLog = true;
            //SendToLogger(null, LogTypeToLogLevel(type), condition);
            //m_IsHandlingBurstedLog = false;
        }

        private void LateUpdatePump_OnLateUpdate()
        {
            ProcessPendingLogs();
        }

        private void ProcessPendingLogs()
        {
            while(m_PendingLogs.TryDequeue(out LogMessage message))
            {
                m_IsHandlingBurstedLog = message.IsFromBurstedContext;
                SendToLogger(null, message.LogLevel, message.Message);
            }
            m_IsHandlingBurstedLog = false;
        }

        private void SendToLogger(UnityEngine.Object context, LogLevel logLevel, string message)
        {
            // Skip 4 frames to get to the original caller
            // 1 - LogException or LogFormat
            // 2 - UnityEngine.Logger.Log(+Warning, +Error, ...), Assert
            // 3 - UnityEngine.Debug.Log(+Warning, +Error, ...), Assert
            // 4 - Caller of Debug.Log(+Warning, +Error, ...), Assert
            StackFrame stackFrame = new StackFrame(4, true);

            // Check to see if this call went through Unity's new stubbed out logging class that
            // currently just proxies the existing logging system.
            // This was found in the v0.17 of the Unity.Entities package
            if (stackFrame.GetMethod()?.ReflectedType.FullName == "Unity.Debug")
            {
                // Go one deeper to overcome the proxy.
                stackFrame = new StackFrame(5, true);
            }

            Log.Logger logger = context != null ? Log.GetLogger(context) : Log.GetStaticLogger(ResolveContextFromStack(stackFrame));
            logger.AtLevel(logLevel, message, stackFrame.GetFileName(), stackFrame.GetMethod()?.Name, stackFrame.GetFileLineNumber());
        }

        private Type ResolveContextFromStack(StackFrame stackFrame)
        {
            MethodBase method = stackFrame.GetMethod();

            return method != null ? method.ReflectedType : typeof(UnknownContext);
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
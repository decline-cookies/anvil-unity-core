using System;
using Anvil.CSharp.Logging;
using UnityEngine;
using StackFrame = System.Diagnostics.StackFrame;

namespace Anvil.Unity.Logging
{
    /// <summary>
    /// An implementation of <see cref="ILogListener"/> for the Unity <see cref="Debug"/> logger.
    /// Captures all logging made through <see cref="Debug"/> and redirects them through a <see cref="Log.Logger"/>.
    /// </summary>
    [DefaultLogListener]
    public sealed class UnityLogListener : ILogListener, UnityEngine.ILogHandler
    {
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
        }

        // The log handler in place before this listener initialized.
        // Called when a log comes through that this listener has already handled.
        private readonly UnityEngine.ILogHandler m_ExistingLogHandler;

        public UnityLogListener()
        {
            Debug.Assert(s_Instance == null, $"There can only be one instance of the {nameof(UnityLogListener)} and it has already been initialized.");
            s_Instance = this;

            m_ExistingLogHandler = Debug.unityLogger.logHandler;
            Debug.unityLogger.logHandler = this;
        }

        /// <inheritdoc />
        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            // Bypass redirecting unity log messages to logger if logger is already handling the log.
            // This happens when UnityLogHandler is being used.
            if (Log.IsHandlingLog)
            {
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
                m_ExistingLogHandler.LogException(exception, context);
                return;
            }

            SendToLogger(context, LogLevel.Error, exception.ToString());   
        }

        private void SendToLogger(UnityEngine.Object context, LogLevel logLevel, string message)
        {
            // Skip 4 frames to get to the original caller
            // 1 - LogException or LogFormat
            // 2 - UnityEngine.Logger.Log(+Warning, +Error, ...), Assert
            // 3 - UnityEngine.Debug.Log(+Warning, +Error, ...), Assert
            // 4 - Caller of Debug.Log(+Warning, +Error, ...), Assert
            StackFrame stackFrame = new StackFrame(4, true);
            object loggerContext = ResolveLogContext(context, stackFrame);
            Log.GetLogger(loggerContext).AtLevel(logLevel, message, stackFrame.GetFileName(), stackFrame.GetMethod().Name, stackFrame.GetFileLineNumber());
        }

        private object ResolveLogContext(UnityEngine.Object unityContext, StackFrame stackFrame)
        {
            return unityContext == null ? stackFrame.GetMethod().ReflectedType : (object)unityContext;
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
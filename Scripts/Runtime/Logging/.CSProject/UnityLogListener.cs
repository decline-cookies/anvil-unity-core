using System;
using Anvil.CSharp.Logging;
using UnityEngine;
using StackFrame = System.Diagnostics.StackFrame;

namespace Anvil.Unity.Logging
{
    [DefaultLogListener]
    public class UnityLogListener : ILogListener, UnityEngine.ILogHandler
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

            StackFrame stackFrame = GetLogCallerStackFrame();
            object loggerContext = ResolveLogContext(context, stackFrame);
            Log.GetLogger(loggerContext).AtLevel(LogTypeToLogLevel(logType), string.Format(format, args), stackFrame.GetFileName(), stackFrame.GetMethod().Name, stackFrame.GetFileLineNumber());
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

            StackFrame stackFrame = GetLogCallerStackFrame();
            object loggerContext = ResolveLogContext(context, stackFrame);
            Log.GetLogger(loggerContext).Error(exception.ToString(), stackFrame.GetFileName(), stackFrame.GetMethod().Name, stackFrame.GetFileLineNumber());
        }

        private StackFrame GetLogCallerStackFrame()
        {
            return new StackFrame(4, true);
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
using System;
using Anvil.CSharp.Logging;
using UnityEngine;
using ILogHandler = Anvil.CSharp.Logging.ILogHandler;

namespace Anvil.Unity.Logging
{
    /// <summary>
    /// Forwards logs to UnityEngine.Debug.
    /// </summary>
    [DefaultLogHandler(PRIORITY)]
    public class UnityLogHandler : ILogHandler
    {
        public const uint PRIORITY = ConsoleLogHandler.PRIORITY + 10;

        public void HandleLog(LogLevel level, string message)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    Debug.Log(message);
                    break;
                case LogLevel.Warning:
                    Debug.LogWarning(message);
                    break;
                case LogLevel.Error:
                    Debug.LogError(message);
                    break;
                default:
                    throw new ArgumentException($"Unhandled log level: {level}");
            }
        }
    }
}

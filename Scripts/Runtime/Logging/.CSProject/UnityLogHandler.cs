using System;
using System.IO;
using Anvil.CSharp.Logging;
using UnityEngine;
using ILogHandler = Anvil.CSharp.Logging.ILogHandler;

namespace Anvil.Unity.Logging
{
    /// <summary>
    /// Forwards logs from <see cref="Log"/> to <see cref="Debug"/>.
    /// </summary>
    [DefaultLogHandler(PRIORITY)]
    public class UnityLogHandler : ILogHandler
    {
        public const uint PRIORITY = ConsoleLogHandler.PRIORITY + 10;

        public void HandleLog(
            LogLevel level,
            string message,
            string callerDerivedTypeName,
            string callerPath,
            string callerName,
            int callerLine)
        {
            message = $"({Path.GetFileNameWithoutExtension(callerPath)}|{callerName}:{callerLine}) {message}";

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
                    throw new NotImplementedException($"Unhandled log level: {level}");
            }
        }
    }
}

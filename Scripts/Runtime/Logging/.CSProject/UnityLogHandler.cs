using System;
using System.IO;
using Anvil.CSharp.Logging;
using UnityEngine;
using UnityEngine.Scripting;
using ILogHandler = Anvil.CSharp.Logging.ILogHandler;

namespace Anvil.Unity.Logging
{
    /// <summary>
    /// Forwards logs from <see cref="Log"/> to <see cref="Debug"/>.
    /// </summary>
    [DefaultLogHandler(PRIORITY)]
    [Preserve]
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
            string callerFile = Path.GetFileNameWithoutExtension(callerPath);

            if (callerLine > 0)
            {
                message = $"({callerFile}:{callerLine}|{callerName}) {message}";
            }
            else
            {
                message = $"({callerFile}|{callerName}) {message}";
            }

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
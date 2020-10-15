using System;
using Anvil.CSharp.Logging;
using UnityEngine;
using ILogHandler = Anvil.CSharp.Logging.ILogHandler;

namespace Anvil.Unity.Logging
{
    [DefaultLogHandler(priority: 10)]
    public class UnityLogHandler : ILogHandler
    {
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

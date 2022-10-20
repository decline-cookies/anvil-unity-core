using System;
using Anvil.CSharp.Logging;
using UnityEngine;
using UnityEngine.Scripting;

namespace Anvil.Unity.Logging
{
    /// <summary>
    /// Forwards logs from <see cref="Log"/> to <see cref="Debug"/>.
    /// </summary>
    [DefaultLogHandler(PRIORITY)]
    [Preserve]
    public class UnityLogHandler : AbstractLogHandler
    {
        public const uint PRIORITY = ConsoleLogHandler.PRIORITY + 10;

        protected override string DefaultLogFormat =>
            $"({LOG_PART_CALLER_TYPE}.{LOG_PART_CALLER_METHOD}) {LOG_PART_MESSAGE}\n" +
            $"(at {LOG_PART_CALLER_FILE}:{LOG_PART_CALLER_LINE})";

        protected override void HandleFormattedLog(LogLevel level, string formattedLog)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    Debug.Log(formattedLog);
                    break;
                case LogLevel.Warning:
                    Debug.LogWarning(formattedLog);
                    break;
                case LogLevel.Error:
                    Debug.LogError(formattedLog);
                    break;
                default:
                    throw new NotImplementedException($"Unhandled log level: {level}");
            }
        }
    }
}
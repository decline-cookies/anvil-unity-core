using System;
using System.Collections.Generic;
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

        // Colors taken from https://stackoverflow.com/a/20298027/640196
        // removed colors that don't work well with Unity's dark theme.
        private static readonly string[] COLORS = new[]
        {
            "#00FF00",
            "#FF0000",
            "#01FFFE",
            "#FFA6FE",
            "#FFDB66",
            "#006401",
            "#95003A",
            "#007DB5",
            "#FF00F6",
            "#FFEEE8",
            "#774D00",
            "#90FB92",
            "#0076FF",
            "#D5FF00",
            "#FF937E",
            "#6A826C",
            "#FF029D",
            "#FE8900",
            "#7E2DD2",
            "#85A900",
            "#FF0056",
            "#A42400",
            "#00AE7E",
            "#BDC6FF",
            "#BDD393",
            "#00B917",
            "#9E008E",
            "#C28C9F",
            "#FF74A3",
            "#01D0FF",
            "#E56FFE",
            "#788231",
            "#0E4CA1",
            "#91D0CB",
            "#BE9970",
            "#968AE8",
            "#BB8800",
            "#DEFF74",
            "#00FFC6",
            "#FFE502",
            "#008F9C",
            "#98FF52",
            "#7544B1",
            "#B500FF",
            "#00FF78",
            "#FF6E41",
            "#005F39",
            "#5FAD4E",
            "#A75740",
            "#A5FFD2",
            "#FFB167",
            "#009BFF",
            "#E85EBE"
        };

        private readonly Dictionary<int, int> m_RainbowColorLookup = new Dictionary<int, int>();
        private int m_NextAvailableColorIndex = 0;

        private LogHighlightMode m_HighlightMode;

        /// <summary>
        /// Sets the mode used to select a highlight color.
        /// See <see cref="LogHighlightMode"/> for options.
        /// </summary>
        public LogHighlightMode HighlightMode
        {
            get => m_HighlightMode;
            set
            {
                m_HighlightMode = value;
                m_RainbowColorLookup.Clear();
                m_NextAvailableColorIndex = 0;
            }
        }

        protected override string DefaultLogFormat
        {
            get =>
                $"<color={LOG_PART_LOG_HIGHLIGHT_COLOR}>({LOG_PART_CALLER_TYPE}.{LOG_PART_CALLER_METHOD})</color> {LOG_PART_MESSAGE}\n"
                + $"(at {LOG_PART_CALLER_FILE}:{LOG_PART_CALLER_LINE})";
        }

        public UnityLogHandler() : base()
        {
            HighlightMode = LogHighlightMode.Constant;
        }

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

        protected override string GetHighlightColorFor(in CallerInfo callerInfo)
        {
            return HighlightMode switch
            {
                LogHighlightMode.Constant => "#DDDDDD",
                LogHighlightMode.Rainbow_PerType => GetColorForType(callerInfo.TypeName),
                LogHighlightMode.Rainbow_PerMethod => GetColorForMethod(in callerInfo),
                LogHighlightMode.Rainbow_PerCallSite => GetColorForCallSite(in callerInfo),
                _ => throw new NotImplementedException($"Not handling implemented for {nameof(HighlightMode)} {HighlightMode}")
            };
        }

        private string GetColorForType(string callerInfoTypeName)
        {
            int typenameKey = callerInfoTypeName.GetHashCode();
            if (!m_RainbowColorLookup.TryGetValue(typenameKey, out int colorIndex))
            {
                colorIndex = m_NextAvailableColorIndex;
                m_RainbowColorLookup.Add(typenameKey, m_NextAvailableColorIndex);
                IncrementNextAvailableColorIndex();
            }

            return COLORS[colorIndex];
        }

        private string GetColorForMethod(in CallerInfo callerInfo)
        {
            int callsiteKey = (callerInfo.FilePath, callerInfo.MethodName).GetHashCode();
            if (!m_RainbowColorLookup.TryGetValue(callsiteKey, out int colorIndex))
            {
                colorIndex = m_NextAvailableColorIndex;
                m_RainbowColorLookup.Add(callsiteKey, m_NextAvailableColorIndex);
                IncrementNextAvailableColorIndex();
            }

            return COLORS[colorIndex];
        }

        private string GetColorForCallSite(in CallerInfo callerInfo)
        {
            int callsiteKey = (callerInfo.FilePath, callerInfo.LineNumber).GetHashCode();
            if (!m_RainbowColorLookup.TryGetValue(callsiteKey, out int colorIndex))
            {
                colorIndex = m_NextAvailableColorIndex;
                m_RainbowColorLookup.Add(callsiteKey, m_NextAvailableColorIndex);
                IncrementNextAvailableColorIndex();
            }

            return COLORS[colorIndex];
        }

        private void IncrementNextAvailableColorIndex()
        {
            m_NextAvailableColorIndex = (m_NextAvailableColorIndex + 1) % COLORS.Length;
        }

        // ----- Inner Types ---- //
        public enum LogHighlightMode
        {
            /// <summary>
            /// A single highlight color used for all messages
            /// </summary>
            Constant,

            /// <summary>
            /// A highlight color is selected for each unique type a log is emitted from.
            /// </summary>
            Rainbow_PerType,

            /// <summary>
            /// A highlight color is selected for each unique method a log is emitted from.
            /// </summary>
            Rainbow_PerMethod,

            /// <summary>
            /// A highlight color is selected for each unique call site a log is emitted from.
            /// </summary>
            Rainbow_PerCallSite
        }
    }
}
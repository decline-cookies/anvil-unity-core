using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Anvil.UnityEditor.Project
{
    /// <summary>
    /// Simply outputs to Unity's <see cref="Debug.Log"/>
    /// </summary>
    /// <remarks>Modification of Jack Dunstan's UnityTextWriter: https://jacksondunstan.com/articles/2986</remarks>
    public class UnityConsoleTextWriter : TextWriter
    {
        private readonly StringBuilder m_Buffer = new StringBuilder();

        /// <summary>
        /// <inheritdoc cref="TextWriter.Flush"/>
        /// </summary>
        public override void Flush()
        {
            Debug.Log(m_Buffer.ToString());
            m_Buffer.Length = 0;
        }

        /// <summary>
        /// <inheritdoc cref="TextWriter.Write"/>
        /// </summary>
        public override void Write(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            m_Buffer.Append(value);

            if (value.EndsWith(Environment.NewLine))
            {
                Flush();
            }
        }

        /// <summary>
        /// <inheritdoc cref="TextWriter.Write"/>
        /// </summary>
        public override void Write(char value)
        {
            m_Buffer.Append(value);

            //Environment.NewLine is '\r\n' on Windows, so as a char we only care about the '\n'
            if (value == '\n')
            {
                Flush();
            }
        }

        /// <summary>
        /// <inheritdoc cref="TextWriter.Write"/>
        /// </summary>
        public override void Write(char[] buffer, int index, int count)
        {
            Write(new string(buffer, index, count));
        }

        /// <summary>
        /// <inheritdoc cref="TextWriter.Encoding"/>
        /// </summary>
        public override Encoding Encoding => Encoding.Default;
    }
}


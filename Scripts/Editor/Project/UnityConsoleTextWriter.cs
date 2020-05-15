using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Anvil.UnityEditor.Project
{
    /// <summary>
    /// Modification of Jack Dunstan's UnityTextWriter: https://jacksondunstan.com/articles/2986
    /// This class simply outputs to Unity's <see cref="Debug.Log"/>
    /// </summary>
    public class UnityConsoleTextWriter : TextWriter
    {
        private readonly StringBuilder m_Buffer = new StringBuilder();

        public override void Flush()
        {
            Debug.Log(m_Buffer.ToString());
            m_Buffer.Length = 0;
        }

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

        public override void Write(char value)
        {
            m_Buffer.Append(value);

            //Environment.NewLine is '\r\n' on Windows, so as a char we only care about the '\n'
            if (value == '\n')
            {
                Flush();
            }
        }

        public override void Write(char[] buffer, int index, int count)
        {
            Write(new string(buffer, index, count));
        }

        public override Encoding Encoding => Encoding.Default;
    }
}


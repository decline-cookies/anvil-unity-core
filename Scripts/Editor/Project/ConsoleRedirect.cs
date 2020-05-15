using System;
using System.IO;
using UnityEditor;

namespace Anvil.UnityEditor.Project
{
    /// <summary>
    /// Editor helper to redirect <see cref="Console.WriteLine"/> and related functions to output in the Unity Console
    /// Window.
    /// </summary>
    [InitializeOnLoad]
    public class ConsoleRedirect
    {
        static ConsoleRedirect()
        {
            //Thread Safe
            Console.SetOut(TextWriter.Synchronized(new UnityConsoleTextWriter()));
        }
    }
}

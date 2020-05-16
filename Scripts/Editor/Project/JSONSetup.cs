using System;
using System.IO;
using Anvil.CSharp.Data;
using Anvil.Unity.Data;
using UnityEditor;

namespace Anvil.UnityEditor.Project
{
    /// <summary>
    /// Editor helper to redirect <see cref="Console.WriteLine"/> and related functions to output in the Unity Console
    /// Window.
    /// </summary>
    [InitializeOnLoad]
    public class JSONSetup
    {
        static JSONSetup()
        {
            JSON.OverrideInstance(new UnityTinyJSONWorker());
        }
    }
}

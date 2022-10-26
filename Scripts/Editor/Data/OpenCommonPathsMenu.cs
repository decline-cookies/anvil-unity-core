using System;
using System.IO;
using Anvil.CSharp.Logging;
using UnityEditor;
using UnityEngine;

namespace Anvil.Unity.Editor.Data
{
    internal static class OpenCommonPathsMenu
    {
        [MenuItem("Anvil/Open Common Path/Persistent Data Directory")]
        internal static void OpenPersistentDataDirectory() => CreateAndOpen(Application.persistentDataPath);

        [MenuItem("Anvil/Open Common Path/Temporary Cache Directory")]
        internal static void OpenTemporaryCacheDirectory() => CreateAndOpen(Application.temporaryCachePath);

        [MenuItem("Anvil/Open Common Path/Assets Directory")]
        internal static void OpenDataDirectory() => CreateAndOpen(Application.dataPath);

        private static void CreateAndOpen(string path)
        {
            Log.GetStaticLogger(typeof(OpenCommonPathsMenu)).Debug($"Opening: {path}");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            Application.OpenURL("file://" + Uri.EscapeUriString(path));
        }
    }
}
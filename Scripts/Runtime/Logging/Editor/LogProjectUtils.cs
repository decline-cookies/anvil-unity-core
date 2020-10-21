using System;
using System.IO;
using System.Linq;
using System.Xml;
using Anvil.CSharp.Logging;
using UnityEditor;
using UnityEngine;

namespace Anvil.Unity.Logging
{
    [InitializeOnLoad]
    public static class LogProjectUtils
    {
        private const string PROJECT_FOLDER_NAME = ".CSProject";

        private static readonly string CSHARP_ASSEMBLY_NAME = typeof(Log).Namespace;
        private static readonly string UNITY_ASSEMBLY_NAME = typeof(LogProjectUtils).Namespace;
        private const string UNITY_CORE_ASSEMBLY_NAME = "UnityEngine.CoreModule";

        private static readonly string CSHARP_ASSEMBLY_PATH;
        private static readonly string CSHARP_SOLUTION_PATH;
        private static readonly string UNITY_SOLUTION_PATH;
        private static readonly string UNITY_CORE_ASSEMBLY_PATH;

#if UNITY_EDITOR_WIN
        private const string EDITOR_ASSEMBLY_PATH = @"Data\Managed\UnityEngine\UnityEngine.CoreModule.dll";
#elif UNITY_EDITOR_OSX
        private const string EDITOR_ASSEMBLY_PATH = @"Unity.app/Contents/Managed/UnityEngine/UnityEngine.CoreModule.dll";
#endif

        static LogProjectUtils()
        {
            string unityRootPath = Path.GetDirectoryName(EditorApplication.applicationPath) ?? string.Empty;
            UNITY_CORE_ASSEMBLY_PATH = Path.Combine(unityRootPath, EDITOR_ASSEMBLY_PATH);

            FindAssemblyPaths(CSHARP_ASSEMBLY_NAME, out CSHARP_ASSEMBLY_PATH, out CSHARP_SOLUTION_PATH);
            FindAssemblyPaths(UNITY_ASSEMBLY_NAME, out string _, out UNITY_SOLUTION_PATH);

            void FindAssemblyPaths(string assemblyName, out string assemblyPath, out string projectPath)
            {
                assemblyPath = AssetDatabase.FindAssets(assemblyName)
                    .Select(AssetDatabase.GUIDToAssetPath)
                    .SingleOrDefault(p => Path.GetExtension(p) == ".dll");

                string rootPath = Path.GetDirectoryName(assemblyPath);

                if (string.IsNullOrEmpty(rootPath))
                {
                    throw new Exception($"Failed to find path for {assemblyName}");
                }

                projectPath = Path.Combine(Path.GetFullPath(rootPath), PROJECT_FOLDER_NAME, $"{assemblyName}.sln");
            }
        }

        [MenuItem("Anvil/Logging/Open C# Project")]
        private static void OpenCSharpProject() => Application.OpenURL(GetFileURL(CSHARP_SOLUTION_PATH));

        [MenuItem("Anvil/Logging/Open Unity Project")]
        private static void OpenUnityProject()
        {
            UpdateUnityProjectReferences();
            Application.OpenURL(GetFileURL(UNITY_SOLUTION_PATH));
        }

        private static void UpdateUnityProjectReferences()
        {
            string projectPath = Path.ChangeExtension(UNITY_SOLUTION_PATH, ".csproj") ?? string.Empty;

            XmlDocument document = new XmlDocument();
            document.Load(projectPath);
            FindReferenceNode(document, CSHARP_ASSEMBLY_NAME).InnerText = Path.GetFullPath(CSHARP_ASSEMBLY_PATH);
            FindReferenceNode(document, UNITY_CORE_ASSEMBLY_NAME).InnerText = UNITY_CORE_ASSEMBLY_PATH;
            document.Save(projectPath);

            XmlNode FindReferenceNode(XmlNode parent, string assemblyName)
            {
                if (parent.Name == "Reference" && parent.Attributes != null &&
                    parent.Attributes["Include"].Value.Contains(assemblyName))
                {
                    return parent["HintPath"];
                }

                foreach (XmlNode child in parent.ChildNodes)
                {
                    XmlNode result = FindReferenceNode(child, assemblyName);

                    if (result != null)
                    {
                        return result;
                    }
                }

                return null;
            }
        }

        private static string GetFileURL(string path) => $"file://{path}";
    }
}

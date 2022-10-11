using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Anvil.CSharp.Logging;
using UnityEditor;
using UnityEngine;

namespace Anvil.Unity.Logging
{
    /// <summary>
    /// Editor tools to ease working on the <see cref="Log"/> C# and Unity projects.
    /// </summary>
    [InitializeOnLoad]
    public static class LogProjectUtils
    {
#if UNITY_EDITOR_WIN
        private const string EDITOR_ASSEMBLY_PATH = @"Data\Managed\UnityEngine\UnityEngine.CoreModule.dll";
#elif UNITY_EDITOR_OSX
        private const string EDITOR_ASSEMBLY_PATH = @"Unity.app/Contents/Managed/UnityEngine/UnityEngine.CoreModule.dll";
#endif

        private const string PROJECT_FOLDER_NAME = ".CSProject";

        private const string UNITY_CORE_ASSEMBLY_NAME = "UnityEngine.CoreModule";

        private static readonly string CSHARP_ASSEMBLY_NAME = typeof(Log).Assembly.GetName().Name;
        private static readonly string UNITY_ASSEMBLY_NAME = typeof(UnityLogHandler).Assembly.GetName().Name;

        private static readonly string CSHARP_ASSEMBLY_PATH;
        private static readonly string CSHARP_SOLUTION_PATH;
        private static readonly string UNITY_SOLUTION_PATH;
        private static readonly string UNITY_CORE_ASSEMBLY_PATH;

        [SuppressMessage("Performance", "HLQ005:Avoid Single() and SingleOrDefault()")]
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

        [SuppressMessage("Performance", "HLQ005:Avoid Single() and SingleOrDefault()")]
        private static void UpdateUnityProjectReferences()
        {
            string projectPath = Path.ChangeExtension(UNITY_SOLUTION_PATH, ".csproj") ?? string.Empty;

            XElement root = XElement.Load(projectPath);

            FindReferenceElement(CSHARP_ASSEMBLY_NAME).Value = Path.GetFullPath(CSHARP_ASSEMBLY_PATH);
            FindReferenceElement(UNITY_CORE_ASSEMBLY_NAME).Value = UNITY_CORE_ASSEMBLY_PATH;

            root.Save(projectPath);

            XElement FindReferenceElement(string assemblyName) =>
                (from itemGroup in root.Elements("ItemGroup")
                from reference in itemGroup.Elements("Reference")
                where ((string)reference.Attribute("Include")).Contains(assemblyName)
                select reference)
                .Single().Element("HintPath");
        }

        private static string GetFileURL(string path) => $"file://{path}";
    }
}
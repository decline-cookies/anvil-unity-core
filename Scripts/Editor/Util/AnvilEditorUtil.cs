using System.IO;
using Anvil.CSharp.Data;
using Anvil.UnityEditor.Data;

namespace Anvil.UnityEditor.Util
{
    public static class AnvilEditorUtil
    {
        public static string ConvertPathToPlatform(string path)
        {
            return path.Replace(AnvilEditorConstants.FORWARD_SLASH, Path.DirectorySeparatorChar);
        }

        public static string ConvertPathToAssetDatabase(string path)
        {
            return path.Replace(Path.DirectorySeparatorChar, AnvilEditorConstants.FORWARD_SLASH);
        }

        public static string ConvertAssetsRelativeToProjectRelativePath(string assetsRelativePath)
        {
            return $"{AnvilEditorConstants.FOLDER_ASSETS}{assetsRelativePath}";
        }

        public static string ConvertProjectRelativeToAssetsRelativePath(string projectRelativePath)
        {
            return projectRelativePath.Replace(AnvilEditorConstants.FOLDER_ASSETS, string.Empty);
        }

        public static string ConvertAssetsRelativeToAbsolutePath(string assetsRelativePath)
        {
            return $"{AnvilEditorConstants.PATH_ABSOLUTE_TO_ASSETS}{assetsRelativePath}";
        }

        public static string ConvertAbsolutePathToAssetsRelativePath(string absolutePath)
        {
            return absolutePath.Replace(AnvilEditorConstants.PATH_ABSOLUTE_TO_ASSETS, string.Empty);
        }

        public static void SaveVOToDisk<T>(T vo, string absolutePath)
            where T : AbstractAnvilVO
        {
            absolutePath = ConvertPathToPlatform(absolutePath);

            string directory = Path.GetDirectoryName(absolutePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(absolutePath, vo.ToJSON());
        }

        public static T LoadVOFromDisk<T>(string absolutePath)
            where T : AbstractAnvilVO, new()
        {
            absolutePath = ConvertPathToPlatform(absolutePath);

            if (!File.Exists(absolutePath))
            {
                return new T();
            }

            string fileContents = File.ReadAllText(absolutePath);
            return JSON.Decode<T>(fileContents);
        }
    }
}


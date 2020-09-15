using System.IO;

namespace Anvil.UnityEditor.Util
{
    public static class AnvilEditorUtil
    {
        private const char FORWARD_SLASH = '/';
        private static readonly string FOLDER_ASSETS = $"Assets{Path.DirectorySeparatorChar}";

        private static readonly string PATH_ABSOLUTE_TO_PROJECT = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}";
        private static readonly string PATH_ABSOLUTE_TO_ASSETS = $"{PATH_ABSOLUTE_TO_PROJECT}{FOLDER_ASSETS}";

        public static string ConvertPathToPlatform(string path)
        {
            return path.Replace(FORWARD_SLASH, Path.DirectorySeparatorChar);
        }

        public static string ConvertPathToAssetDatabase(string path)
        {
            return path.Replace(Path.DirectorySeparatorChar, FORWARD_SLASH);
        }

        public static string ConvertAssetsRelativeToProjectRelativePath(string assetsRelativePath)
        {
            return $"{FOLDER_ASSETS}{assetsRelativePath}";
        }

        public static string ConvertProjectRelativeToAssetsRelativePath(string projectRelativePath)
        {
            return projectRelativePath.Replace(FOLDER_ASSETS, string.Empty);
        }

        public static string ConvertAssetsRelativeToAbsolutePath(string assetsRelativePath)
        {
            return $"{PATH_ABSOLUTE_TO_ASSETS}{assetsRelativePath}";
        }

        public static string ConvertAbsolutePathToAssetsRelativePath(string absolutePath)
        {
            return absolutePath.Replace(PATH_ABSOLUTE_TO_ASSETS, string.Empty);
        }
    }
}


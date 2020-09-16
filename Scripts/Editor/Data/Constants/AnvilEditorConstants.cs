using System.IO;

namespace Anvil.UnityEditor.Data
{
    public static class AnvilEditorConstants
    {
        public const char FORWARD_SLASH = '/';
        public static readonly string FOLDER_ASSETS = $"Assets{Path.DirectorySeparatorChar}";
        public static readonly string FOLDER_ANVIL = $"Anvil{Path.DirectorySeparatorChar}";

        public static readonly string PATH_ABSOLUTE_TO_PROJECT = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}";
        public static readonly string PATH_ABSOLUTE_TO_ASSETS = $"{PATH_ABSOLUTE_TO_PROJECT}{FOLDER_ASSETS}";

        public const string EXTENSION_JSON = ".json";
    }
}


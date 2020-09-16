using System.IO;

namespace Anvil.UnityEditor.Data
{
    public static class AnvilEditorAssetManagementConstants
    {
        public static readonly string FOLDER_ASSET_MANAGEMENT = $"AssetManagement{Path.DirectorySeparatorChar}";

        public static readonly string FILE_NAME_CONFIG = $"Config";

        public static readonly string FILE_CONFIG = $"{FILE_NAME_CONFIG}{AnvilEditorConstants.EXTENSION_JSON}";


        public static readonly string PATH_ABSOLUTE_ASSET_MANAGEMENT_CONFIG_VO = $"{AnvilEditorConstants.PATH_ABSOLUTE_TO_PROJECT}{AnvilEditorConstants.FOLDER_ANVIL}{FOLDER_ASSET_MANAGEMENT}{FILE_CONFIG}";

    }
}


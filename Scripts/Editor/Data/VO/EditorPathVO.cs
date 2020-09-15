using Anvil.CSharp.Data;
using Anvil.UnityEditor.Util;
using TinyJSON;

namespace Anvil.UnityEditor.Data
{
    public class EditorPathVO : AbstractAnvilVO
    {
        [Include, EncodeName("Path")]
        private string m_Path;

        public EditorPathVO()
        {
            m_Path = string.Empty;
        }

        public string AssetsRelativePath
        {
            get => m_Path;
            set => m_Path = AnvilEditorUtil.ConvertPathToPlatform(value);
        }

        public string ProjectRelativePath
        {
            get => AnvilEditorUtil.ConvertAssetsRelativeToProjectRelativePath(m_Path);
            set
            {
                string assetsRelativePath = AnvilEditorUtil.ConvertProjectRelativeToAssetsRelativePath(value);
                AssetsRelativePath = assetsRelativePath;
            }
        }

        public string AbsolutePath
        {
            get => AnvilEditorUtil.ConvertAssetsRelativeToAbsolutePath(m_Path);
            set
            {
                string assetsRelativePath = AnvilEditorUtil.ConvertAbsolutePathToAssetsRelativePath(value);
                AssetsRelativePath = assetsRelativePath;
            }
        }

        [AfterDecode]
        private void UpdateAfterDecode()
        {
            m_Path = AnvilEditorUtil.ConvertPathToPlatform(m_Path);
        }

        [BeforeEncode]
        private void UpdateBeforeEncode()
        {
            m_Path = AnvilEditorUtil.ConvertPathToAssetDatabase(m_Path);
        }
    }
}


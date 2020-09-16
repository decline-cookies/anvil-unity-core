using Anvil.CSharp.Data;
using Anvil.UnityEditor.Util;
using TinyJSON;

namespace Anvil.UnityEditor.Data
{
    public class EditorPathVO : AbstractAnvilVO
    {
        [Include, EncodeName("AssetsRelativePath")]
        private string m_AssetsRelativePath;

        public EditorPathVO()
        {
            m_AssetsRelativePath = string.Empty;
        }

        public string AssetsRelativePath
        {
            get => m_AssetsRelativePath;
            set => m_AssetsRelativePath = AnvilEditorUtil.ConvertPathToPlatform(value);
        }

        public string ProjectRelativePath
        {
            get => AnvilEditorUtil.ConvertAssetsRelativeToProjectRelativePath(m_AssetsRelativePath);
            set
            {
                string assetsRelativePath = AnvilEditorUtil.ConvertProjectRelativeToAssetsRelativePath(value);
                AssetsRelativePath = assetsRelativePath;
            }
        }

        public string AbsolutePath
        {
            get => AnvilEditorUtil.ConvertAssetsRelativeToAbsolutePath(m_AssetsRelativePath);
            set
            {
                string assetsRelativePath = AnvilEditorUtil.ConvertAbsolutePathToAssetsRelativePath(value);
                AssetsRelativePath = assetsRelativePath;
            }
        }

        [AfterDecode]
        private void UpdateAfterDecode()
        {
            m_AssetsRelativePath = AnvilEditorUtil.ConvertPathToPlatform(m_AssetsRelativePath);
        }

        [BeforeEncode]
        private void UpdateBeforeEncode()
        {
            m_AssetsRelativePath = AnvilEditorUtil.ConvertPathToAssetDatabase(m_AssetsRelativePath);
        }
    }
}


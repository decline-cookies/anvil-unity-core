using Anvil.UnityEditor.Data;
using Anvil.UnityEditor.Util;
using UnityEditor;

namespace Anvil.UnityEditor.Asset
{
    [InitializeOnLoad]
    public class AssetManagementController
    {
        private const string HAS_PERFORMED_INITIAL_SCAN = "AAM_HAS_PERFORMED_INITIAL_SCAN";

        public static AssetManagementController Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = new AssetManagementController();
                }

                return s_Instance;
            }
        }
        private static AssetManagementController s_Instance;

        static AssetManagementController()
        {
            if (SessionState.GetBool(HAS_PERFORMED_INITIAL_SCAN, false))
            {
                return;
            }
            SessionState.SetBool(HAS_PERFORMED_INITIAL_SCAN, true);
        }

        private AssetManagementConfigVO m_AssetManagementConfigVO;


        public AssetManagementConfigVO AssetManagementConfigVO
        {
            get
            {
                if (m_AssetManagementConfigVO == null)
                {
                    m_AssetManagementConfigVO = AnvilEditorUtil.LoadVOFromDisk<AssetManagementConfigVO>(AnvilEditorAssetManagementConstants.PATH_ABSOLUTE_ASSET_MANAGEMENT_CONFIG_VO);
                }

                return m_AssetManagementConfigVO;
            }
        }

        public AssetManagementController()
        {
        }

        public void SaveConfigVO()
        {
            AnvilEditorUtil.SaveVOToDisk(AssetManagementConfigVO, AnvilEditorAssetManagementConstants.PATH_ABSOLUTE_ASSET_MANAGEMENT_CONFIG_VO);
        }
    }
}


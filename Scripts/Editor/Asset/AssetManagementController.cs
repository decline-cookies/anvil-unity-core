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
                    //TODO: Load from disk
                    m_AssetManagementConfigVO = new AssetManagementConfigVO();
                }

                return m_AssetManagementConfigVO;
            }
        }

        public AssetManagementController()
        {
        }
    }
}


using Anvil.UnityEditor.Data;
using Anvil.UnityEditor.Util;
using UnityEditor;

namespace Anvil.UnityEditor.Asset
{
    [InitializeOnLoad]
    public class AMController
    {
        private const string HAS_PERFORMED_INITIAL_SCAN = "AAM_HAS_PERFORMED_INITIAL_SCAN";

        public static AMController Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = new AMController();
                }

                return s_Instance;
            }
        }
        private static AMController s_Instance;

        static AMController()
        {
            if (SessionState.GetBool(HAS_PERFORMED_INITIAL_SCAN, false))
            {
                return;
            }
            SessionState.SetBool(HAS_PERFORMED_INITIAL_SCAN, true);
        }

        private AMConfigVO m_AMConfigVO;


        public AMConfigVO AMConfigVO
        {
            get
            {
                if (m_AMConfigVO == null)
                {
                    m_AMConfigVO = AnvilEditorUtil.LoadVOFromDisk<AMConfigVO>(AnvilEditorAMConstants.PATH_ABSOLUTE_ASSET_MANAGEMENT_CONFIG_VO);
                }

                return m_AMConfigVO;
            }
        }

        public AMController()
        {
        }

        public void SaveConfigVO()
        {
            AnvilEditorUtil.SaveVOToDisk(AMConfigVO, AnvilEditorAMConstants.PATH_ABSOLUTE_ASSET_MANAGEMENT_CONFIG_VO);
        }
    }
}


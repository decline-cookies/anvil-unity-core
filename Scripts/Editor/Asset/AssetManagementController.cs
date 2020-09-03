using UnityEditor;

namespace Anvil.UnityEditor.Asset
{
    [InitializeOnLoad]
    public class AssetManagementController
    {
        private const string HAS_PERFORMED_INITIAL_SCAN = "AAM_HAS_PERFORMED_INITIAL_SCAN";

        static AssetManagementController()
        {
            if (SessionState.GetBool(HAS_PERFORMED_INITIAL_SCAN, false))
            {
                return;
            }
            SessionState.SetBool(HAS_PERFORMED_INITIAL_SCAN, true);


        }
    }
}


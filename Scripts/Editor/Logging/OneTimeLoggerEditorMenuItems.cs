using Anvil.CSharp.Logging;
using UnityEditor;

namespace Anvil.Unity.Logging
{
    /// <summary>
    /// Unity Editor menu items related to the <see cref="OneTimeLogger"/>
    /// </summary>
    public static class OneTimeLoggerEditorMenuItems
    {
        [MenuItem("Anvil/Logging/Reset One Time Logs")]
        private static void ResetOneTimeLogs()
        {
            Log.GetStaticLogger(typeof(OneTimeLoggerEditorMenuItems)).Debug("Resetting one time log filter...");
            OneTimeLogger.Reset();
        }
    }
}
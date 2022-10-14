using Anvil.CSharp.Logging;
using UnityEditor;

namespace Anvil.Unity.Editor.AssetImport
{
    /// <summary>
    /// Prevents the asset database from refreshing while the editor is in play mode.
    /// This means that any changed assets aren't imported and changed source isn't recompiled.
    ///
    /// NOTE: When this is enabled it overrides the behaviour of
    /// Preferences -> General -> Script Changes While Playing
    /// (In versions of Unity that include that option)
    /// </summary>
    [InitializeOnLoad]
    public static class PreventAutoRefreshWhilePlaying
    {
        private const string MENU_PATH = "Anvil/Prevent Asset Refresh While Playing";
        private const string PREFSKEY_IS_ENABLED = MENU_PATH + ":IsEnabled";

        private static readonly Logger s_Logger = Log.GetStaticLogger(typeof(PreventAutoRefreshWhilePlaying));
        private static bool s_IsAutoRefreshDisabled = false;

        private static bool IsEnabled
        {
            get => EditorPrefs.GetBool(PREFSKEY_IS_ENABLED);
            set => EditorPrefs.SetBool(PREFSKEY_IS_ENABLED, value);
        }

        static PreventAutoRefreshWhilePlaying()
        {
            EditorApplication.playModeStateChanged += EditorApplication_PlayModeStateChanged;
        }

        [MenuItem(MENU_PATH)]
        private static void ToggleEnabled()
        {
            IsEnabled = !IsEnabled;

            if (IsEnabled)
            {
                EditorUtility.DisplayDialog(
                    "Note",
                    "Enabling this option prevents assets and scripts from reloading while the editor is playing. This overrides the behaviour of \"Preferences -> General -> Script Changes While Playing\".",
                    "Thanks Boss"
                    );
            }

            bool shouldPreventRefresh = IsEnabled && EditorApplication.isPlaying;
            if (shouldPreventRefresh)
            {
                PreventAutoRefresh();
            }
            else
            {
                AllowAutoRefresh();
            }
        }

        [MenuItem(MENU_PATH, true)]
        private static bool ToggleEnabled_Validator()
        {
            // Make sure the menu is showing the correct checked state
            Menu.SetChecked(MENU_PATH, IsEnabled);
            return true;
        }

        private static void EditorApplication_PlayModeStateChanged(PlayModeStateChange state)
        {
            if (!IsEnabled)
            {
                return;
            }

            switch (state)
            {
                case PlayModeStateChange.EnteredPlayMode:
                    PreventAutoRefresh();
                    break;

                case PlayModeStateChange.EnteredEditMode:
                    AllowAutoRefresh();
                    break;
            }
        }

        private static void PreventAutoRefresh()
        {
            if (!s_IsAutoRefreshDisabled)
            {
                s_IsAutoRefreshDisabled = true;
                AssetDatabase.DisallowAutoRefresh();
                s_Logger.Debug("Auto asset refresh disabled.");
            }
        }

        private static void AllowAutoRefresh()
        {
            if (s_IsAutoRefreshDisabled)
            {
                AssetDatabase.AllowAutoRefresh();
                s_IsAutoRefreshDisabled = false;
                s_Logger.Debug("Auto asset refresh enabled.");

                AssetDatabase.Refresh();
            }
        }
    }
}
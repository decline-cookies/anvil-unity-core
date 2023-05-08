using Anvil.CSharp.Logging;
using UnityEditor;

namespace Anvil.Unity.Editor.AssetImport
{
    /// <summary>
    /// Prevents the asset database from refreshing until the developer presses the play button.
    /// This means that any changed assets aren't imported and changed source isn't recompiled.
    ///
    /// No more waiting for Unity to compile before you can press play and then wait for your game to start!
    ///
    /// NOTE: When this is enabled it overrides the behaviour of
    /// Preferences -> General -> Script Changes While Playing
    /// (In versions of Unity that include that option)
    /// </summary>
    [InitializeOnLoad]
    public static class DeferAutoRefreshUntilPlayPressed
    {
        private const string MENU_PATH = "Anvil/Defer Asset Refresh Until Play Pressed";
        private const string PREFSKEY_IS_ENABLED = MENU_PATH + ":IsEnabled";

        private static readonly Logger s_Logger = Log.GetStaticLogger(typeof(DeferAutoRefreshUntilPlayPressed));
        public static bool IsAutoRefreshDisabled { get; private set; } = false;

        public static bool IsEnabled
        {
            get => EditorPrefs.GetBool(PREFSKEY_IS_ENABLED);
            private set => EditorPrefs.SetBool(PREFSKEY_IS_ENABLED, value);
        }

        static DeferAutoRefreshUntilPlayPressed()
        {
            EditorApplication.playModeStateChanged += EditorApplication_PlayModeStateChanged;
            UpdateAutoRefreshAtRest();
        }

        [MenuItem(MENU_PATH)]
        private static void ToggleEnabled()
        {
            IsEnabled = !IsEnabled;

            if (IsEnabled)
            {
                EditorUtility.DisplayDialog(
                    "Note",
                    "Enabling this option prevents assets and scripts from reloading until you press play on the editor. "
                    + "\n\nThis overrides the behaviour of \"Preferences -> General -> Script Changes While Playing\"."
                    + "\n\nYou can force a refresh/compile without pressing play with ⌘+R",
                    "Thanks Boss");
            }

            UpdateAutoRefreshAtRest();
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

            // We're a little over-eager calling prevent refresh because other editor scripts can mess with the
            // flow so that the order isn't always cleanly ExitEdit -> EnterPlay -> ExitPlay -> EnterEdit.
            // Example: ScreenshotOnEditorStop changes the IsPlaying flag during a state change and causes
            // the following flow when play stops:
            // ExitPlay -> ExitEdit (!) -> ExitPlay -> EnterEdit
            // (!) - Indicates an unexpected state change
            switch (state)
            {
                case PlayModeStateChange.ExitingPlayMode:
                    PreventAutoRefresh();
                    break;

                case PlayModeStateChange.ExitingEditMode:
                    if (!EditorApplication.isPlaying)
                    {
                        AllowAutoRefresh();
                    }
                    break;

                case PlayModeStateChange.EnteredPlayMode:
                    PreventAutoRefresh();
                    break;

                case PlayModeStateChange.EnteredEditMode:
                    PreventAutoRefresh();
                    break;
            }
        }

        /// <summary>
        /// Set the auto refresh state when the editor state hasn't changed.
        /// Ensures that we're blocking or allowing auto refresh based on the <see cref="IsEnabled"/> state.
        /// </summary>
        private static void UpdateAutoRefreshAtRest()
        {
            bool shouldPreventRefresh = IsEnabled;
            if (shouldPreventRefresh)
            {
                PreventAutoRefresh();
            }
            else
            {
                AllowAutoRefresh();
            }
        }

        private static void PreventAutoRefresh()
        {
            if (!IsAutoRefreshDisabled)
            {
                IsAutoRefreshDisabled = true;
                AssetDatabase.DisallowAutoRefresh();
            }
        }

        private static void AllowAutoRefresh()
        {
            if (IsAutoRefreshDisabled)
            {
                AssetDatabase.AllowAutoRefresh();
                IsAutoRefreshDisabled = false;

                // Only refresh if the other prevention script isn't blocking refresh.
                if (!PreventAutoRefreshWhilePlaying.IsAutoRefreshDisabled)
                {
                    AssetDatabase.Refresh();
                }
            }
        }
    }
}
using Anvil.CSharp.Logging;
using Anvil.Unity.Editor.UI;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Anvil.Unity.Editor.Build
{

#if UNITY_ANDROID
    /// <summary>
    /// Caches keystore information to <see cref="EditorPrefs"/> so that developers don't have to re-enter it each time
    /// Unity is launched.
    /// </summary>
    public class AndroidKeystoreHelper : IPreprocessBuildWithReport
    {
        private const string MENU_PATH_PREFIX = "Anvil/Keystore";
        private static readonly Logger Logger = Log.GetStaticLogger(typeof(AndroidKeystoreHelper));

        private static string PrefsKeyPrefix
        {
            get => $"{MENU_PATH_PREFIX}:{PlayerSettings.productName}-{nameof(AndroidKeystoreHelper)}";
        }
        private static string KeystorePassPrefsKey
        {
            get => $"{PrefsKeyPrefix}-keystorePass";
        }
        private static string KeystoreAliasPassPrefsKey
        {
            get => $"{PrefsKeyPrefix}-keystoreAliasPass";
        }

        /// <summary>
        /// The keystore values that Unity doesn't clear between sessions.
        /// </summary>
        private static bool ArePersistentKeystoreValuesSet
        {
            get => !string.IsNullOrEmpty(PlayerSettings.Android.keystoreName)
                   && !string.IsNullOrEmpty(PlayerSettings.Android.keyaliasName);
        }

        private static bool AreKeystorePasswordsSet
        {
            get => !string.IsNullOrEmpty(PlayerSettings.Android.keystorePass)
                   && !string.IsNullOrEmpty(PlayerSettings.Android.keyaliasPass);
        }


        [MenuItem(MENU_PATH_PREFIX+"/Set Saved Keystore Passwords")]
        private static void SetKeystorePasswords()
        {
            Logger.Debug("Starting...");
            bool result = ApplyKeystorePasswordsFromUser();
            Logger.Debug($"Complete. WasSuccessful:{result}");
        }

        [MenuItem(MENU_PATH_PREFIX+"/Clear Keystore Passwords")]
        private static void ClearKeystorePasswordsFromPrefs()
        {
            Logger.Debug("Starting...");
            EditorPrefs.DeleteKey(KeystorePassPrefsKey);
            EditorPrefs.DeleteKey(KeystoreAliasPassPrefsKey);
            Logger.Debug("Complete. Keys: (see below)" +
                         $"\n {KeystorePassPrefsKey}" +
                         $"\n {KeystoreAliasPassPrefsKey}");

            EditorUtility.DisplayDialog(
                "Keystore Passwords Deleted",
                "Keystore passwords have been deleted from prefs.",
                "Ok");
        }

        private static bool ApplyKeystorePasswordsFromPrefs()
        {
            PlayerSettings.Android.keystorePass = EditorPrefs.GetString(KeystorePassPrefsKey);
            PlayerSettings.Android.keyaliasPass = EditorPrefs.GetString(KeystoreAliasPassPrefsKey);

            Logger.Debug($"Keystore passwords retrieved from prefs. WasSuccessful:{AreKeystorePasswordsSet}, Keys:(see below)" +
                         $"\n {KeystorePassPrefsKey}" +
                         $"\n {KeystoreAliasPassPrefsKey}");
            return AreKeystorePasswordsSet;
        }

        private static bool ApplyKeystorePasswordsFromUser()
        {
            EditorInputDialogResult keystorePass = EditorInputDialog.Show("Enter Keystore Password",
                "Please enter the keystore password.",
                "Keystore Password",
                "Ok",
                "Cancel");

            if (keystorePass.Result == EditorInputDialogResult.ResultAction.Cancel)
            {
                Logger.Debug("Cancelled");
                return false;
            }

            EditorInputDialogResult keystoreAliasPass = EditorInputDialog.Show("Enter Keystore Alias Pass",
                "Please enter the keystore alias password.",
                "Keystore Alias Password",
                "Ok",
                "Cancel");

            if (keystoreAliasPass.Result == EditorInputDialogResult.ResultAction.Cancel)
            {
                Logger.Debug("Cancelled");
                return false;
            }

            bool saveToPrefsResult = EditorUtility.DisplayDialog(
                "Save to Editor Prefs?",
                "Unity forgets these passwords each time the editor is quit." +
                "\n\nWould you like to save them to your editor prefs and automatically enter them during future sessions?",
                "Yes!",
                "No");

            if (saveToPrefsResult)
            {
                Logger.Debug("Saving keystore details to editor prefs. Keys: (see below)" +
                             $"\n {KeystorePassPrefsKey}" +
                             $"\n {KeystoreAliasPassPrefsKey}");
                EditorPrefs.SetString(KeystorePassPrefsKey, keystorePass.ResultText);
                EditorPrefs.SetString(KeystoreAliasPassPrefsKey, keystoreAliasPass.ResultText);
            }

            PlayerSettings.Android.keystorePass = keystorePass.ResultText;
            PlayerSettings.Android.keyaliasPass = keystoreAliasPass.ResultText;

            Logger.Debug($"Keystore passwords set! WasSuccessful:{AreKeystorePasswordsSet}");
            return AreKeystorePasswordsSet;
        }


        /// <inheritdoc cref="IPreprocessBuildWithReport"/>
        public int callbackOrder
        {
            get => 0;
        }

        /// <summary>
        /// Created by the Unity Editor when building to device to pre-process the build.
        /// Not intended to be used manually by developers.
        /// </summary>
        public AndroidKeystoreHelper()
        {

        }

        /// <summary>
        /// Verifies that the keystore is configured and that the passwords are set.
        /// If the passwords are not set this method attempts to retrieve the passwords from <see cref="EditorPrefs"/>.
        /// If the passwords do not exist in preferences then the user is given the opportunity to provide them and
        /// optionally save them to <see cref="EditorPrefs"/>.
        /// </summary>
        /// <param name="report"><inheritdoc cref="IPreprocessBuildWithReport"/></param>
        /// <exception cref="BuildFailedException">Thrown if the build cannot be completed.</exception>
        public void OnPreprocessBuild(BuildReport report)
        {
            Logger.Debug("Checking if keystore is configured...");

            if (!ArePersistentKeystoreValuesSet)
            {
                EditorUtility.DisplayDialog(
                    "Keystore Error",
                    "Persistent keystore information has not been set.."
                    + "\n\nPlease set the values in \"Project Settings -> Player -> Publish Settings\"", "Ok");
                throw new BuildFailedException(
                    "Basic keystore information is not set. Please set in \"Project Settings -> Player -> Publish Settings\"");
            }

            if (AreKeystorePasswordsSet)
            {
                Logger.Debug("Keystore passwords present. Continuing build!");
                return;
            }

            Logger.Debug("Keystore passwords are not set. Attempting to apply them from editor prefs...");
            if (ApplyKeystorePasswordsFromPrefs())
            {
                Logger.Debug("Keystore passwords applied. Continuing build!");
                return;
            }

            Logger.Debug("Keystore passwords are not present in prefs. Asking user for them...");
            bool shouldEnterPasswordsNowResult = EditorUtility.DisplayDialog(
                "Keystore Warning",
                "Your keystore password and alias password aren't set or saved in preferences."
                + "\n\nThis will cause the build to fail. Would you like to enter them now?",
                "Yes",
                "No");

            if (!shouldEnterPasswordsNowResult)
            {
                Logger.Debug("User does not want to enter passwords. Continuing build to let Unity fail.");
                return;
            }

            if (!ApplyKeystorePasswordsFromUser())
            {
                Logger.Debug("Failed to set passwords. Continuing build to let Unity fail.");
                return;
            }

            Logger.Debug("Keystore passwords applied. Continuing build!");
        }
    }
#endif
}
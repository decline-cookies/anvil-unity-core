using UnityEditor;

namespace Anvil.Unity.Editor.Debug
{
    /// <summary>
    /// Sets up script defines for common Anvil specific script defines
    /// </summary>
    public static class AnvilDebugSafetyScriptDefines
    {
        /// <summary>
        /// Script Define for code safety checks.
        /// Should not be performance intensive.
        /// </summary>
        public const string ANVIL_DEBUG_SAFETY = "ANVIL_DEBUG_SAFETY";
        
        /// <summary>
        /// Script Define for code safety checks that are expensive to determine.
        /// </summary>
        public const string ANVIL_DEBUG_SAFETY_EXPENSIVE = "ANVIL_DEBUG_SAFETY_EXPENSIVE";
        
        
        private const string ANVIL_DEBUG_SAFETY_MENU_PATH = ScriptDefinesToggle.MENU_PATH_BASE + "/" + ANVIL_DEBUG_SAFETY;
        private const string ANVIL_DEBUG_SAFETY_EXPENSIVE_MENU_PATH = ScriptDefinesToggle.MENU_PATH_BASE + "/" + ANVIL_DEBUG_SAFETY_EXPENSIVE;

        //For ANVIL_DEBUG_SAFETY, we don't require anything to turn it on, but we need to ensure that ANVIL_DEBUG_SAFETY_EXPENSIVE is turned off if we are off
        private static readonly ScriptDefineDefinition ANVIL_DEBUG_SAFETY_DEFINITION = new ScriptDefineDefinition(ANVIL_DEBUG_SAFETY,
                                                                                                                  ANVIL_DEBUG_SAFETY_MENU_PATH,
                                                                                                                  null,
                                                                                                                  new[]
                                                                                                                  {
                                                                                                                      ANVIL_DEBUG_SAFETY_EXPENSIVE
                                                                                                                  });

        //For ANVIL_DEBUG_SAFETY_EXPENSIVE, we require ANVIL_DEBUG_SAFETY to be on if we're on, but we don't need to turn anything off if we turn off
        private static readonly ScriptDefineDefinition ANVIL_DEBUG_SAFETY_EXPENSIVE_DEFINITION = new ScriptDefineDefinition(ANVIL_DEBUG_SAFETY_EXPENSIVE,
                                                                                                                            ANVIL_DEBUG_SAFETY_EXPENSIVE_MENU_PATH,
                                                                                                                            new[]
                                                                                                                            {
                                                                                                                                ANVIL_DEBUG_SAFETY
                                                                                                                            },
                                                                                                                            null);


        [MenuItem(ANVIL_DEBUG_SAFETY_MENU_PATH, true)]
        private static bool Toggle_Validator_Safety()
        {
            return ScriptDefinesToggle.Toggle_Validator(ANVIL_DEBUG_SAFETY_DEFINITION);
        }

        [MenuItem(ANVIL_DEBUG_SAFETY_MENU_PATH)]
        private static void Toggle_Safety()
        {
            ScriptDefinesToggle.Toggle(ANVIL_DEBUG_SAFETY_DEFINITION);
        }

        [MenuItem(ANVIL_DEBUG_SAFETY_EXPENSIVE_MENU_PATH, true)]
        private static bool Toggle_Validator_Safety_Expensive()
        {
            return ScriptDefinesToggle.Toggle_Validator(ANVIL_DEBUG_SAFETY_EXPENSIVE_DEFINITION);
        }

        [MenuItem(ANVIL_DEBUG_SAFETY_EXPENSIVE_MENU_PATH)]
        private static void Toggle_Safety_Expensive()
        {
            ScriptDefinesToggle.Toggle(ANVIL_DEBUG_SAFETY_EXPENSIVE_DEFINITION);
        }
    }
}

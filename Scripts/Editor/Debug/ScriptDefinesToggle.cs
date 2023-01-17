using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;

namespace Anvil.Unity.Editor.Debug
{
    /// <summary>
    /// Editor helper class to easily toggle on and off Script Defines and their dependencies.
    /// </summary>
    [InitializeOnLoad]
    public static class ScriptDefinesToggle
    {
        /// <summary>
        /// The base location for where items should appear in the menu.
        /// </summary>
        public const string MENU_PATH_BASE = "Anvil/Script Defines";
        
        private static readonly NamedBuildTarget NAMED_BUILD_TARGET;
        private static readonly Dictionary<string, ScriptDefineDefinition> SCRIPT_DEFINE_DEFINITIONS_LOOKUP;

        static ScriptDefinesToggle()
        {
            SCRIPT_DEFINE_DEFINITIONS_LOOKUP = new Dictionary<string, ScriptDefineDefinition>();
            
            BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;
            BuildTargetGroup buildTargetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);
            NAMED_BUILD_TARGET = NamedBuildTarget.FromBuildTargetGroup(buildTargetGroup);
        }
        
        /// <summary>
        /// Validator function to show if a given Script Define is currently enabled or not.
        /// Call this to validate a Script Define menu item.
        /// </summary>
        /// <param name="scriptDefineDefinition">The <see cref="ScriptDefineDefinition"/> representing the define</param>
        /// <returns>True as Script Defines can be toggled at any time</returns>
        public static bool Toggle_Validator(ScriptDefineDefinition scriptDefineDefinition)
        {
            PlayerSettings.GetScriptingDefineSymbols(NAMED_BUILD_TARGET, out string[] defines);
            bool isEnabled = defines.Contains(scriptDefineDefinition.Define);
            Menu.SetChecked(scriptDefineDefinition.MenuPath, isEnabled);
            return true;
        }
        
        /// <summary>
        /// Toggle function to turn on or off a given Script Define.
        /// Call this to toggle a Script Define from the menu.
        /// </summary>
        /// <param name="scriptDefineDefinition">The <see cref="ScriptDefineDefinition"/> representing the define</param>
        public static void Toggle(ScriptDefineDefinition scriptDefineDefinition)
        {
            //App level script defines may require framework level script defines to be turned on, this ensures
            //that we get a project consolidated view of what to turn on/off
            scriptDefineDefinition = GetMergedScriptDefineDefinition(scriptDefineDefinition);
            
            PlayerSettings.GetScriptingDefineSymbols(NAMED_BUILD_TARGET, out string[] defines);
            HashSet<string> definesHashSet = defines.ToHashSet();
            
            //If the define is already turned on
            if (definesHashSet.Contains(scriptDefineDefinition.Define))
            {
                //Turn it off as well as everything that depends on it
                definesHashSet.Remove(scriptDefineDefinition.Define);
                foreach (string dependentDefine in scriptDefineDefinition.DependentDefines)
                {
                    definesHashSet.Remove(dependentDefine);
                }
            }
            else
            {
                //Turn it on as well as everything that it requires
                definesHashSet.Add(scriptDefineDefinition.Define);
                foreach (string requiredDefine in scriptDefineDefinition.RequiredDefines)
                {
                    definesHashSet.Add(requiredDefine);
                }
            }

            PlayerSettings.SetScriptingDefineSymbols(NAMED_BUILD_TARGET, definesHashSet.ToArray());
        }

        private static ScriptDefineDefinition GetMergedScriptDefineDefinition(ScriptDefineDefinition scriptDefineDefinition)
        {
            //If we've never seen this before, we'll take this as the ground truth.
            if (!SCRIPT_DEFINE_DEFINITIONS_LOOKUP.TryGetValue(scriptDefineDefinition.Define, out ScriptDefineDefinition cachedScriptDefineDefinition))
            {
                cachedScriptDefineDefinition = scriptDefineDefinition;
                SCRIPT_DEFINE_DEFINITIONS_LOOKUP.Add(scriptDefineDefinition.Define, cachedScriptDefineDefinition);
            }
            //Otherwise we will merge the incoming definitions requirements with ours to allow other assemblies to require/depend on our defines
            else
            {
                cachedScriptDefineDefinition.MergeIn(scriptDefineDefinition);
            }
            
            return cachedScriptDefineDefinition;
        }
    }
}

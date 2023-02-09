using Anvil.CSharp.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Callbacks;

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
        private static readonly Logger LOGGER;

        static ScriptDefinesToggle()
        {
            LOGGER = Log.GetStaticLogger(typeof(ScriptDefinesToggle));
            SCRIPT_DEFINE_DEFINITIONS_LOOKUP = new Dictionary<string, ScriptDefineDefinition>();

            BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;
            BuildTargetGroup buildTargetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);
            NAMED_BUILD_TARGET = NamedBuildTarget.FromBuildTargetGroup(buildTargetGroup);
        }

        [DidReloadScripts]
        private static void ValidateSafety()
        {
            PlayerSettings.GetScriptingDefineSymbols(NAMED_BUILD_TARGET, out string[] defines);
            HashSet<string> definesHashSet = defines.ToHashSet();
            int definesHashSetCount = definesHashSet.Count;

            foreach (ScriptDefineDefinition scriptDefineDefinition in SCRIPT_DEFINE_DEFINITIONS_LOOKUP.Values)
            {
                ValidateForErrors(scriptDefineDefinition);
                //If we're not active we don't need to check our requirements
                if (!definesHashSet.Contains(scriptDefineDefinition.Define))
                {
                    continue;
                }

                //Ensure that our requirements are present in case some other script or the developer manually removed them
                var requiredDefines = scriptDefineDefinition
                    .RequiredDefines.Where(define => !definesHashSet.Contains(define));
                foreach (string define in requiredDefines)
                {
                    LOGGER.Error($"{scriptDefineDefinition.Define} requires that {define} is set, but it's not present! Removing {scriptDefineDefinition.Define} and all dependencies if they were set.");
                    RemoveDependentDefines(scriptDefineDefinition, definesHashSet);
                }
            }

            //If we made any changes, then update the PlayerSettings
            if (definesHashSet.Count != definesHashSetCount)
            {
                PlayerSettings.SetScriptingDefineSymbols(NAMED_BUILD_TARGET, definesHashSet.ToArray());
            }
        }

        /// <summary>
        /// Registers a <see cref="ScriptDefineDefinition"/> so that the editor is aware of requirements and dependencies
        /// for a specific Script Define
        /// </summary>
        /// <param name="scriptDefineDefinition">The <see cref="ScriptDefineDefinition"/> to register.</param>
        public static void RegisterScriptDefineDefinition(ScriptDefineDefinition scriptDefineDefinition)
        {
            //Get the cached Script Define Definition if it exists, otherwise make sure it's added
            //We now have our ground truth
            ScriptDefineDefinition cachedScriptDefineDefinition = GetOrRegisterCachedScriptDefineDefinition(scriptDefineDefinition);

            //We want to create script definitions for all our requirements and dependencies as well if they don't exist
            EnsureRelatedScriptDefineDefinitionsAreCreated(scriptDefineDefinition);

            //Ensure that all Script Define Definitions know about each other
            UpdateRelationships(cachedScriptDefineDefinition, scriptDefineDefinition);
        }

        private static ScriptDefineDefinition GetOrRegisterCachedScriptDefineDefinition(ScriptDefineDefinition scriptDefineDefinition)
        {
            if (!SCRIPT_DEFINE_DEFINITIONS_LOOKUP.TryGetValue(scriptDefineDefinition.Define, out ScriptDefineDefinition cachedScriptDefineDefinition))
            {
                cachedScriptDefineDefinition = scriptDefineDefinition;
                SCRIPT_DEFINE_DEFINITIONS_LOOKUP.Add(cachedScriptDefineDefinition.Define, cachedScriptDefineDefinition);
            }

            return cachedScriptDefineDefinition;
        }

        private static ScriptDefineDefinition GetCachedScriptDefineDefinition(string define)
        {
            if (!SCRIPT_DEFINE_DEFINITIONS_LOOKUP.TryGetValue(define, out ScriptDefineDefinition scriptDefineDefinition))
            {
                throw new InvalidOperationException($"Trying to get Script Define Definition for {define} but it doesn't exist! Did you call {nameof(RegisterScriptDefineDefinition)}?");
            }

            return scriptDefineDefinition;
        }

        private static void EnsureRelatedScriptDefineDefinitionsAreCreated(ScriptDefineDefinition scriptDefineDefinition)
        {
            EnsureRelatedScriptDefineDefinitionsAreCreated(scriptDefineDefinition.RequiredDefines);
            EnsureRelatedScriptDefineDefinitionsAreCreated(scriptDefineDefinition.DependentDefines);
        }

        private static void EnsureRelatedScriptDefineDefinitionsAreCreated(HashSet<string> relatedDefines)
        {
            foreach (string define in relatedDefines)
            {
                if (SCRIPT_DEFINE_DEFINITIONS_LOOKUP.TryGetValue(define, out ScriptDefineDefinition relatedScriptDefineDefinition))
                {
                    continue;
                }

                relatedScriptDefineDefinition = new ScriptDefineDefinition(define, null, null, null);
                SCRIPT_DEFINE_DEFINITIONS_LOOKUP.Add(define, relatedScriptDefineDefinition);
            }
        }

        private static void ValidateForErrors(ScriptDefineDefinition scriptDefineDefinition)
        {
            string[] dupes = scriptDefineDefinition.DependentDefines.Intersect(scriptDefineDefinition.RequiredDefines).ToArray();
            if (dupes.Any())
            {
                throw new InvalidOperationException($"Circular reference detected! The script define {scriptDefineDefinition.Define} requires and depends on {string.Join(",", dupes)}. Please check your {nameof(ScriptDefineDefinition)}s to ensure you have set them up properly.");
            }
        }

        private static void UpdateRelationships(
            ScriptDefineDefinition cachedScriptDefineDefinition,
            ScriptDefineDefinition incomingScriptDefineDefinition)
        {
            foreach (string define in incomingScriptDefineDefinition.RequiredDefines)
            {
                cachedScriptDefineDefinition.RequiredDefines.Add(define);
                ScriptDefineDefinition requiredDefineDefinition = SCRIPT_DEFINE_DEFINITIONS_LOOKUP[define];
                requiredDefineDefinition.DependentDefines.Add(cachedScriptDefineDefinition.Define);
            }

            foreach (string define in incomingScriptDefineDefinition.DependentDefines)
            {
                cachedScriptDefineDefinition.DependentDefines.Add(define);
                ScriptDefineDefinition dependentDefineDefinition = SCRIPT_DEFINE_DEFINITIONS_LOOKUP[define];
                dependentDefineDefinition.RequiredDefines.Add(cachedScriptDefineDefinition.Define);
            }
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
            scriptDefineDefinition = GetCachedScriptDefineDefinition(scriptDefineDefinition.Define);

            PlayerSettings.GetScriptingDefineSymbols(NAMED_BUILD_TARGET, out string[] defines);
            HashSet<string> definesHashSet = defines.ToHashSet();

            //If the define is already turned on
            if (definesHashSet.Contains(scriptDefineDefinition.Define))
            {
                LOGGER.Debug($"Removing {scriptDefineDefinition.Define} and all dependencies if they were set.");
                //Turn it off as well as everything that depends on it
                RemoveDependentDefines(scriptDefineDefinition, definesHashSet);
            }
            else
            {
                LOGGER.Debug($"Adding {scriptDefineDefinition.Define} and all requirements if not already set.");
                //Turn it on as well as everything that it requires
                AddRequiredDefines(scriptDefineDefinition, definesHashSet);
            }

            PlayerSettings.SetScriptingDefineSymbols(NAMED_BUILD_TARGET, definesHashSet.ToArray());
        }

        private static void RemoveDependentDefines(ScriptDefineDefinition scriptDefineDefinition, HashSet<string> definesHashSet)
        {
            //Remove ourself
            definesHashSet.Remove(scriptDefineDefinition.Define);
            //Recursively remove any other dependencies
            IEnumerable<ScriptDefineDefinition> dependentDefineDefinitions = scriptDefineDefinition.DependentDefines.Select(GetCachedScriptDefineDefinition);
            foreach (ScriptDefineDefinition dependentDefineDefinition in dependentDefineDefinitions)
            {
                RemoveDependentDefines(dependentDefineDefinition, definesHashSet);
            }
        }

        private static void AddRequiredDefines(ScriptDefineDefinition scriptDefineDefinition, HashSet<string> definesHashSet)
        {
            //Add ourself
            definesHashSet.Add(scriptDefineDefinition.Define);
            //Recursively add any other requirements
            IEnumerable<ScriptDefineDefinition> requiredDefineDefinitions = scriptDefineDefinition.RequiredDefines.Select(GetCachedScriptDefineDefinition);
            foreach (ScriptDefineDefinition requiredDefineDefinition in requiredDefineDefinitions)
            {
                AddRequiredDefines(requiredDefineDefinition, definesHashSet);
            }
        }
    }
}
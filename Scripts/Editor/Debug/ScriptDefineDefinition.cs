using System;
using System.Collections.Generic;
using System.Linq;

namespace Anvil.Unity.Editor.Debug
{
    /// <summary>
    /// Defines information for use with a Script Define and the behaviour when toggling on and off.
    /// <see cref="ScriptDefinesToggle"/>
    /// </summary>
    public class ScriptDefineDefinition
    {
        internal string Define { get; }
        internal string MenuPath { get; }
        internal HashSet<string> RequiredDefines { get; }
        internal HashSet<string> DependentDefines { get; }

        public ScriptDefineDefinition(string define, string menuPath, IEnumerable<string> requiredDefines, IEnumerable<string> dependentDefines)
        {
            Define = define;
            MenuPath = menuPath;
            RequiredDefines = requiredDefines != null ? requiredDefines.ToHashSet() : new HashSet<string>();
            DependentDefines = dependentDefines != null ? dependentDefines.ToHashSet() : new HashSet<string>();
        }

        internal void MergeIn(ScriptDefineDefinition other)
        {
            int requiredCount = RequiredDefines.Count;
            int dependentCount = DependentDefines.Count;
            RequiredDefines.UnionWith(other.RequiredDefines);
            DependentDefines.UnionWith(other.DependentDefines);
            
            //If something changed we should validate it's safe
            if (RequiredDefines.Count == requiredCount
             && DependentDefines.Count == dependentCount)
            {
                return;
            }

            HashSet<string> dupes = new HashSet<string>(RequiredDefines);
            dupes.IntersectWith(DependentDefines);
            if (dupes.Count == 0)
            {
                return;
            }

            throw new InvalidOperationException($"Circular reference detected! The script define {Define} requires and depends on {string.Join("," , dupes)}. Please check your {nameof(ScriptDefineDefinition)}s to ensure you have set them up properly.");
        }
    }
}

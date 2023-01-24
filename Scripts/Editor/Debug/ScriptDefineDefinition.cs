using System.Collections.Generic;
using System.Linq;

namespace Anvil.Unity.Editor.Debug
{
    /// <summary>
    /// Defines information for use with a Script Define and the behaviour when toggling on and off.
    /// See: <see cref="ScriptDefinesToggle"/>
    /// </summary>
    public class ScriptDefineDefinition
    {
        internal string Define { get; }
        internal string MenuPath { get; }
        internal HashSet<string> RequiredDefines { get; }
        internal HashSet<string> DependentDefines { get; }

        /// <summary>
        /// Creates a new <see cref="ScriptDefineDefinition"/> instance.
        /// </summary>
        /// <param name="define">The Script Define to add/remove to/from PlayerSettings</param>
        /// <param name="menuPath">The path to show in the menu for toggling on and off. <see cref="ScriptDefinesToggle.MENU_PATH_BASE"/></param>
        /// <param name="requiredDefines">Script Defines that are required to be set in order for this to be set.</param>
        /// <param name="dependentDefines">Script Defines that require this to be set.</param>
        public ScriptDefineDefinition(string define, string menuPath, IEnumerable<string> requiredDefines, IEnumerable<string> dependentDefines)
        {
            Define = define;
            MenuPath = menuPath;
            RequiredDefines = requiredDefines != null
                ? requiredDefines.ToHashSet()
                : new HashSet<string>();
            DependentDefines = dependentDefines != null
                ? dependentDefines.ToHashSet()
                : new HashSet<string>();
        }
    }
}

using Anvil.Unity.Data;
using UnityEditor;

namespace Anvil.UnityEditor.Data
{
    /// <summary>
    /// Editor utility that will run after a script recompile to set a <see cref="UnityTinyJSONParser"/> as the
    /// parser to use for JSON encoding/decoding via <see cref="JSON"/>
    /// </summary>
    [InitializeOnLoad]
    public class UnityOverrideJSONParser
    {
        static UnityOverrideJSONParser()
        {
            UnityTinyJSONParser.Register();
        }
    }
}

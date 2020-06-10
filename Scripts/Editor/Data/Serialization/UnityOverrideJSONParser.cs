using Anvil.Unity.Data;
using UnityEditor;

namespace Anvil.UnityEditor.Data
{
    [InitializeOnLoad]
    public class UnityOverrideJSONParser
    {
        static UnityOverrideJSONParser()
        {
            UnityTinyJSONParser.Register();
        }
    }
}
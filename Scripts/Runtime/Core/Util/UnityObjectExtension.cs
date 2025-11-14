using UnityEngine;

namespace Anvil.Unity.Core
{
    /// <summary>
    /// A collection of extension methods for working with <see cref="Object"/>s.
    /// </summary>
    public static class UnityObjectExtension
    {
        private const string CLONE_SUFFIX = "(Clone)";

        public static void RemoveCloneSuffix(this Object obj)
        {
            if (obj.name.EndsWith(CLONE_SUFFIX))
            {
                obj.name = obj.name[..^CLONE_SUFFIX.Length];
            }
        }
    }
}

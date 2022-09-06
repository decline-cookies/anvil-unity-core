using UnityEngine;

namespace Anvil.Unity.Data
{
    /// <summary>
    /// Resets the <see cref="CSharp.Data.ID"/> static class on load.
    /// This allows for Unity's domain reloading to be disabled which speeds up Burst compiler development.
    /// </summary>
    internal static class ID
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Reset()
        {
            CSharp.Data.ID.Dispose();
        }
    }
}

using Anvil.CSharp.Logging;
using UnityEngine;

namespace Anvil.Unity.Logging
{
    /// <summary>
    /// Hooks into <see cref="RuntimeInitializeOnLoadMethod"/> to reset static Log state
    /// when projects don't perform domain reloads between runs.
    /// </summary>
    internal class Logging_StaticStateResetHelper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            OneTimeLogger.Reset();
        }
    }
}
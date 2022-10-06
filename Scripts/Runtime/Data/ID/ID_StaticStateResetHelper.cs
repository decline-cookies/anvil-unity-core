using Anvil.CSharp.Data;
using Anvil.CSharp.Logging;
using UnityEngine;

namespace Anvil.Unity.Data
{
    /// <summary>
    /// Resets the static state of <see cref="ID"/>.
    /// Required when Domain Reload is disabled to prevent instances from a previous run
    /// session from listening to the static event.
    /// </summary>
    internal static class ID_StaticStateResetHelper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            Log.GetStaticLogger(typeof(ID_StaticStateResetHelper)).Debug("Resetting ID state.");
            ID.ResetState();
        }
    }
}
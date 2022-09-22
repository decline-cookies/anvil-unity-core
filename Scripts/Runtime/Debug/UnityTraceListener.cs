using System;
using UnityEngine;

namespace Anvil.Unity.Debugging
{
    /// <summary>
    /// A custom <see cref="System.Diagnostics.TraceListener"> for Unity, to allow the use of <see cref="System.Diagnostics"> functions.
    /// By default, all of the .NET Debug/Trace functions have no effect in Unity, without a listener manually forwarding calls.
    /// 
    /// The most common and important functions enabled by this listener are:
    /// - <see cref="System.Diagnostics.Debug.WriteLine"> (forwarded to UnityEngine.Debug.Log)
    /// - <see cref="System.Diagnostics.Debug.Assert"> (forwarded to a thrown exception)
    /// 
    /// Note: Both <see cref="System.Diagnostics.Debug"> and <see cref="System.Diagnostics.Trace"> functions use the same
    /// listener, the difference is that Debug functions become no-ops in release builds, while Trace always works.
    /// </summary>
    public class UnityTraceListener : System.Diagnostics.TraceListener
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            System.Diagnostics.Trace.Listeners.Add(new UnityTraceListener());
        }

        public override void Write(string message) { UnityEngine.Debug.Log(message); }
        public override void WriteLine(string message) { UnityEngine.Debug.Log(message); }

        public override void Fail(string message) { throw new Exception(message); }
        public override void Fail(string message, string details) { throw new Exception($"{message} {details}"); }
    }
}

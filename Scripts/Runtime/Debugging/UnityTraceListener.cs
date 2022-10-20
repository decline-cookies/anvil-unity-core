using Anvil.Unity.Logging;

namespace Anvil.Unity.Debugging
{
    /// <summary>
    /// A custom <see cref="System.Diagnostics.TraceListener" /> for Unity, to allow the use of <see cref="System.Diagnostics" /> functions.
    /// By default, all of the .NET Debug/Trace functions have no effect in Unity, without a listener manually forwarding calls.
    /// 
    /// The most common and important functions enabled by this listener are:
    /// - <see cref="System.Diagnostics.Debug.WriteLine" /> (forwarded to UnityEngine.Debug.Log)
    /// - <see cref="System.Diagnostics.Debug.Assert" /> (forwarded to a thrown exception)
    /// 
    /// Note: Both <see cref="System.Diagnostics.Debug" /> and <see cref="System.Diagnostics.Trace" /> functions use the same
    /// listener, the difference is that Debug functions become no-ops in release builds, while Trace always works.
    /// </summary>
    internal class UnityTraceListener : System.Diagnostics.TraceListener
    {
        private static UnityTraceListener s_Instance;

        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            if (s_Instance != null)
            {
                System.Diagnostics.Trace.Listeners.Remove(s_Instance);
                s_Instance = null;
            }

            s_Instance = new UnityTraceListener();
            System.Diagnostics.Trace.Listeners.Add(s_Instance);
        }

        private UnityTraceListener() { }

        [UnityLogListener.Exclude]
        public override void WriteLine(string message) { UnityEngine.Debug.Log(message); }
        [UnityLogListener.Exclude]
        public override void Write(string message) { WriteLine(message); }

        [UnityLogListener.Exclude]
        public override void Fail(string message) { throw new System.Exception(message); }
        [UnityLogListener.Exclude]
        public override void Fail(string message, string details) { Fail($"{message}\n{details}"); }
    }
}

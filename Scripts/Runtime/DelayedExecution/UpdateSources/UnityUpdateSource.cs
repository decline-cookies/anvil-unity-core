using UnityEngine.Scripting;

namespace Anvil.Unity.DelayedExecution
{
    /// <summary>
    /// A specific <see cref="AbstractUnityUpdateSource"/> that dispatches an <see cref="AbstractUnityUpdateSource{T}.OnUpdate"/>
    /// event to an <see cref="UpdateHandle"/> when <see cref="MonoBehaviour.Update"/> is called by Unity.
    /// <see cref="UnityUpdateSourceMonoBehaviour"/>
    /// </summary>
    public class UnityUpdateSource : AbstractUnityUpdateSource<UnityUpdateSourceMonoBehaviour>
    {
        // Required to prevent constructor from being stripped when this type is referenced.
        [RequiredMember]
        public UnityUpdateSource() : base() { }
    }
}

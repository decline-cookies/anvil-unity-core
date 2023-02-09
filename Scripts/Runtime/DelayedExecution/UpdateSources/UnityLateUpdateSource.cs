using UnityEngine.Scripting;

namespace Anvil.Unity.DelayedExecution
{
    /// <summary>
    /// A specific <see cref="AbstractUnityUpdateSource"/> that dispatches an <see cref="AbstractUnityUpdateSource{T}.OnUpdate"/>
    /// event to an <see cref="UpdateHandle"/> when <see cref="MonoBehaviour.LateUpdate"/> is called by Unity.
    /// <see cref="UnityLateUpdateSourceMonoBehaviour"/>
    /// </summary>
    public class UnityLateUpdateSource : AbstractUnityUpdateSource<UnityLateUpdateSourceMonoBehaviour>
    {
        // Required to prevent constructor from being stripped when this type is referenced.
        [RequiredMember]
        public UnityLateUpdateSource() : base() { }
    }
}

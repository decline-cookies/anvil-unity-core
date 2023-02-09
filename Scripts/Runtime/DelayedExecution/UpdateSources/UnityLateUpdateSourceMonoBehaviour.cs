namespace Anvil.Unity.DelayedExecution
{
    /// <summary>
    /// A specific <see cref="MonoBehaviour"/> that dispatches an <see cref="AbstractUnityUpdateSource{T}.OnUpdate"/>
    /// event to an <see cref="UpdateHandle"/> when <see cref="MonoBehaviour.LateUpdate"/>
    /// is called by Unity.
    /// <see cref="UnityLateUpdateSource"/>
    /// </summary>
    public class UnityLateUpdateSourceMonoBehaviour : AbstractUnityUpdateSourceMonoBehaviour
    {
        private void LateUpdate()
        {
            TriggerOnUpdateDispatchFunction();
        }
    }
}

namespace Anvil.Unity.DelayedExecution
{
    /// <summary>
    /// A specific <see cref="MonoBehaviour"/> that dispatches an <see cref="AbstractUnityUpdateSource{T}.OnUpdate"/>
    /// event to an <see cref="UpdateHandle"/> when <see cref="MonoBehaviour.FixedUpdate"/>
    /// is called by Unity.
    /// <see cref="UnityFixedUpdateSource"/>
    /// </summary>
    public class UnityFixedUpdateSourceMonoBehaviour : AbstractUnityUpdateSourceMonoBehaviour
    {
        private void FixedUpdate()
        {
            TriggerOnUpdateDispatchFunction();
        }
    }
}

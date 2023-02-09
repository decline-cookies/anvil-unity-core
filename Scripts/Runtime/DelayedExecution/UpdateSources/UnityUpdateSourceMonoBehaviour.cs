namespace Anvil.Unity.DelayedExecution
{
    /// <summary>
    /// A specific <see cref="MonoBehaviour"/> that dispatches an <see cref="AbstractUnityUpdateSource{T}.OnUpdate"/>
    /// event to an <see cref="UpdateHandle"/> when <see cref="MonoBehaviour.Update"/>
    /// is called by Unity.
    /// <see cref="UnityUpdateSource"/>
    /// </summary>
    public class UnityUpdateSourceMonoBehaviour : AbstractUnityUpdateSourceMonoBehaviour
    {
        private void Update()
        {
            TriggerOnUpdateDispatchFunction();
        }
    }
}

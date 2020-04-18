namespace Anvil.Unity.DelayedExecution
{
    /// <summary>
    /// A specific MonoBehaviour that dispatches an OnUpdate event to an <see cref="UpdateHandle"/> when LateUpdate
    /// is called by Unity.
    /// </summary>
    public class UnityLateUpdateSourceMonoBehaviour : AbstractUnityUpdateSourceMonoBehaviour
    {
        private void LateUpdate()
        {
            TriggerOnUpdateDispatchFunction();
        }
    }
}
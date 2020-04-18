namespace Anvil.Unity.DelayedExecution
{
    /// <summary>
    /// A specific MonoBehaviour that dispatches an OnUpdate event to an <see cref="UpdateHandle"/> when FixedUpdate
    /// is called by Unity.
    /// </summary>
    public class UnityFixedUpdateSourceMonoBehaviour : AbstractUnityUpdateSourceMonoBehaviour
    {
        private void FixedUpdate()
        {
            TriggerOnUpdateDispatchFunction();
        }
    }
}
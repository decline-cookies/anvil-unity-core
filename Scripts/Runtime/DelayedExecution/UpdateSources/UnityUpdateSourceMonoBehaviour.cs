namespace Anvil.Unity.DelayedExecution
{
    /// <summary>
    /// A specific MonoBehaviour that dispatches an OnUpdate event to an <see cref="UpdateHandle"/> when Update
    /// is called by Unity.
    /// </summary>
    public class UnityUpdateSourceMonoBehaviour : AbstractUnityUpdateSourceMonoBehaviour
    {
        private void Update()
        {
            TriggerOnUpdateDispatchFunction();
        }
    }
}
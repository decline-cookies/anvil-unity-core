namespace Anvil.Unity.DelayedExecution
{
    public class UnityFixedUpdateSourceMonoBehaviour : AbstractUnityUpdateSourceMonoBehaviour
    {
        private void FixedUpdate()
        {
            TriggerOnUpdateDispatchFunction();
        }
    }
}
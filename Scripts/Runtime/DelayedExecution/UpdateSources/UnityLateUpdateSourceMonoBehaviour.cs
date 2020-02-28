namespace Anvil.Unity.DelayedExecution
{
    public class UnityLateUpdateSourceMonoBehaviour : AbstractUnityUpdateSourceMonoBehaviour
    {
        private void LateUpdate()
        {
            TriggerOnUpdateDispatchFunction();
        }
    }
}
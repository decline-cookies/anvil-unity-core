namespace Anvil.Unity.DelayedExecution
{
    public class UnityUpdateSourceMonoBehaviour : AbstractUnityUpdateSourceMonoBehaviour
    {
        private void Update()
        {
            TriggerOnUpdateDispatchFunction();
        }
    }
}
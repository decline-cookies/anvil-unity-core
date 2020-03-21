namespace Anvil.Unity.Core
{
    public abstract class AbstractAnvilApplication : AbstractAnvilMonoBehaviour
    {
        private void Awake()
        {
            Init();
        }

        protected virtual void Init()
        {
            
        }
    }
}


namespace Anvil.Unity.Core
{
    public abstract class AbstractAnvilApplication : AbstractAnvilMonoBehaviour
    {
        protected override void Awake()
        {
            Init();
        }

        protected virtual void Init()
        {
            
        }
    }
}


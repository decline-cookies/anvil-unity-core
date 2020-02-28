namespace Anvil.Unity.Core
{
    public abstract class AnvilAbstractApplication : AnvilAbstractMonoBehaviour
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


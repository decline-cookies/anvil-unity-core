using Anvil.Unity.Core;

namespace Anvil.Unity.Content
{
    public abstract class AbstractContent : AbstractAnvilMonoBehaviour
    {
        internal AbstractContentController ContentController { private get; set; }
        
        internal bool IsContentDisposing { get; private set; }

        protected override void DisposeSelf()
        {
            if (IsContentDisposing)
            {
                return;
            }
            IsContentDisposing = true;

            if (ContentController != null && !ContentController.IsContentControllerDisposing)
            {
                ContentController.Dispose();
                ContentController = null;
            }
            
            base.DisposeSelf();
        }

        public T GetContentController<T>() where T : AbstractContentController
        {
            return (T)ContentController;
        }

    }
}


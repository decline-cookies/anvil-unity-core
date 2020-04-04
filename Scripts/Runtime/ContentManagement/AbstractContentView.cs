using Anvil.Unity.Core;

namespace Anvil.Unity.ContentManagement
{
    public abstract class AbstractContentView : AbstractAnvilMonoBehaviour
    {
        internal AbstractContentController ContentController { private get; set; }
        
        internal bool IsContentViewDisposing { get; private set; }

        protected override void DisposeSelf()
        {
            if (IsContentViewDisposing)
            {
                return;
            }
            IsContentViewDisposing = true;

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


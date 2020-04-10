using Anvil.Unity.Core;

namespace Anvil.Unity.Content
{
    public abstract class AbstractContent<T> : AbstractContent where T : AbstractContentController
    {
        public new T Controller
        {
            get { return (T)base.Controller; }
        }
    }

    public abstract class AbstractContent : AbstractAnvilMonoBehaviour
    {
        public AbstractContentController Controller { get; internal set; }
        
        internal bool IsContentDisposing { get; private set; }

        protected override void DisposeSelf()
        {
            if (IsContentDisposing)
            {
                return;
            }
            IsContentDisposing = true;

            if (Controller != null && !Controller.IsContentControllerDisposing)
            {
                Controller.Dispose();
                Controller = null;
            }
            
            base.DisposeSelf();
        }

    }
}


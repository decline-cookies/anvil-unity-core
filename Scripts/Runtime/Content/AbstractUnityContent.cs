using Anvil.CSharp.Content;
using Anvil.Unity.Core;

namespace Anvil.Unity.Content
{
    public abstract class AbstractUnityContent<T> : AbstractUnityContent where T : AbstractContentController
    {
        public new T Controller => (T)base.Controller;
    }

    public abstract class AbstractUnityContent : AbstractAnvilMonoBehaviour, IContent
    {
        public AbstractContentController Controller { get; internal set; }
        
        public bool IsContentDisposing { get; private set; }

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


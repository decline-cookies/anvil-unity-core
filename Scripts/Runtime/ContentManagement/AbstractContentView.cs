using Anvil.Unity.Core;

namespace Anvil.Unity.ContentManagement
{
    public abstract class AbstractContentView : AbstractAnvilMonoBehaviour
    {
        private AbstractContentController m_ContentController;
        
        internal bool IsContentViewDisposing { get; private set; }

        protected override void DisposeSelf()
        {
            if (IsContentViewDisposing)
            {
                return;
            }
            IsContentViewDisposing = true;

            if (m_ContentController != null && !m_ContentController.IsContentControllerDisposing)
            {
                m_ContentController.Dispose();
                m_ContentController = null;
            }
            
            base.DisposeSelf();
        }

        internal void SetContentController(AbstractContentController contentController)
        {
            m_ContentController = contentController;
        }

        public T GetContentController<T>() where T : AbstractContentController
        {
            return (T)m_ContentController;
        }
    }
}


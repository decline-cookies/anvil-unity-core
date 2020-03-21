using Anvil.CSharp.Core;

namespace Anvil.Unity.ContentManagement
{
    public class ContentGroup : AbstractAnvilDisposable
    {
        public ContentGroupConfigVO ConfigVO { get; private set; }

        private AbstractContentController m_ActiveContentController;
        private AbstractContentController m_PendingContentController;

        public ContentGroup(ContentGroupConfigVO contentGroupConfigVO)
        {
            ConfigVO = contentGroupConfigVO;
        }

        protected override void DisposeSelf()
        {
            base.DisposeSelf();
        }

        public void Show(AbstractContentController contentController)
        {
            
        }
    }
}


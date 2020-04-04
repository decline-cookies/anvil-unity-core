using Anvil.CSharp.Core;
using Anvil.CSharp.DelayedExecution;
using Anvil.Unity.DelayedExecution;
using UnityEngine;

namespace Anvil.Unity.ContentManagement
{
    public class ContentLayer : AbstractAnvilDisposable
    {
        public ContentLayerConfigVO ConfigVO { get; private set; }
        public Transform ContentLayerRoot { get; private set; }
        
        public ContentManager ContentManager { get; private set; }
        
        private AbstractContentController m_ActiveContentController;
        private AbstractContentController m_PendingContentController;

        private UpdateHandle m_UpdateHandle;

        public ContentLayer(ContentLayerConfigVO contentGroupLayerVO, ContentManager contentManager)
        {
            ConfigVO = contentGroupLayerVO;
            ContentManager = contentManager;
            
            m_UpdateHandle = UpdateHandle.Create<UnityUpdateSource>();

            InitGameObject();
        }

        protected override void DisposeSelf()
        {
            if (m_UpdateHandle != null)
            {
                m_UpdateHandle.Dispose();
                m_UpdateHandle = null;
            }
            base.DisposeSelf();
        }

        private void InitGameObject()
        {
            GameObject groupRootGO = new GameObject($"[CL - {ConfigVO.ID}]");
            ContentLayerRoot = groupRootGO.transform;
            ContentLayerRoot.SetParent(ContentManager.ContentRoot);
            ContentLayerRoot.localPosition = ConfigVO.LocalPosition;
            ContentLayerRoot.localRotation = Quaternion.identity;
            ContentLayerRoot.localScale = Vector3.one;
        }

        public void Show(AbstractContentController contentController)
        {
            //Validate the passed in controller to ensure we avoid weird cases such as:
            // - Showing the same instance that is already showing or about to be shown
            // - Might have a pending controller in the process of loading
            
            
            m_PendingContentController = contentController;

            //If there's an Active Controller currently being shown, we need to clear it.
            if (m_ActiveContentController != null)
            {
                m_ActiveContentController.PlayOut();
            }
            else
            {
                //Otherwise we can just show the pending controller
                ShowPendingContentController();
            }
        }

        public void Clear()
        {
            Show(null);
        }

        private void ShowPendingContentController()
        {
            if (m_PendingContentController == null)
            {
                return;
            }
            //We can't show the pending controller right away because we may not have the necessary assets loaded. 
            //So we need to construct a Sequential Command and populate with the required commands to load the assets needed. 

            m_PendingContentController.OnLoadComplete += HandleOnPendingContentControllerLoadComplete;
            m_PendingContentController.Load();
        }

        private void HandleOnPendingContentControllerLoadComplete()
        {
            m_PendingContentController.OnLoadComplete -= HandleOnPendingContentControllerLoadComplete;
            
            m_PendingContentController.InitAfterLoadComplete();

            AbstractContentView contentView = m_PendingContentController.GetContentView<AbstractContentView>();
            Transform transform = contentView.transform;
            transform.SetParent(ContentLayerRoot);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;

            AttachLifeCycleListeners(m_PendingContentController);
            m_PendingContentController.PlayIn();
        }

        private void AttachLifeCycleListeners(AbstractContentController contentController)
        {
            contentController.OnPlayInComplete += HandleOnPlayInComplete;
            contentController.OnPlayOutComplete += HandleOnPlayOutComplete;
        }
        
        private void RemoveLifeCycleListeners(AbstractContentController contentController)
        {
            contentController.OnPlayInComplete -= HandleOnPlayInComplete;
            contentController.OnPlayOutComplete -= HandleOnPlayOutComplete;
        }

        private void HandleOnPlayInComplete()
        {
            m_PendingContentController.OnPlayInComplete -= HandleOnPlayInComplete;

            m_ActiveContentController = m_PendingContentController;
            m_PendingContentController = null;
            
            m_ActiveContentController.InitAfterPlayInComplete();
        }

        private void HandleOnPlayOutComplete()
        {
            if (m_ActiveContentController != null)
            {
                RemoveLifeCycleListeners(m_ActiveContentController);
                m_ActiveContentController.Dispose();
            }

            ShowPendingContentController();
        }

        
        
        
    }
}


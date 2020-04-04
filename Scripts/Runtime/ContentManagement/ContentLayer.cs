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
        
        public ContentGroup ContentGroup { get; private set; }
        
        private AbstractContentController m_ActiveContentController;
        private AbstractContentController m_PendingContentController;

        private UpdateHandle m_UpdateHandle;

        public ContentLayer(ContentLayerConfigVO contentGroupLayerVO, ContentGroup contentGroup)
        {
            ConfigVO = contentGroupLayerVO;
            ContentGroup = contentGroup;
            
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
            ContentLayerRoot.SetParent(ContentGroup.ContentGroupRoot);
            ContentLayerRoot.localPosition = ConfigVO.LocalPosition;
            ContentLayerRoot.localRotation = Quaternion.identity;
            ContentLayerRoot.localScale = Vector3.one;
        }

        public void Show(AbstractContentController contentController)
        {
            //Validate the passed in controller to ensure we avoid weird cases such as:
            // - Showing the same instance that is already showing or about to be shown
            
            
            m_PendingContentController = contentController;

            //If there's an Active Controller currently being shown, we need to clear it.
            
            //Otherwise we can just show the pending controller
            ShowPendingContentController();
        }

        private void ShowPendingContentController()
        {
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

            m_PendingContentController.OnPlayInComplete += HandleOnPendingContentControllerPlayInComplete;
            m_PendingContentController.PlayIn();
        }

        private void HandleOnPendingContentControllerPlayInComplete()
        {
            m_PendingContentController.OnPlayInComplete -= HandleOnPendingContentControllerPlayInComplete;

            m_ActiveContentController = m_PendingContentController;
            m_PendingContentController = null;

            m_ActiveContentController.InitAfterPlayInComplete();
        }
        
        
    }
}


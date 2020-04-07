using System;
using Anvil.CSharp.Core;
using Anvil.CSharp.DelayedExecution;
using Anvil.Unity.DelayedExecution;
using UnityEngine;

namespace Anvil.Unity.Content
{
    public class ContentGroup : AbstractAnvilDisposable
    {
        public event Action<AbstractContentController> OnLoadStart;
        public event Action<AbstractContentController> OnLoadComplete;
        public event Action<AbstractContentController> OnPlayInStart;
        public event Action<AbstractContentController> OnPlayInComplete;
        public event Action<AbstractContentController> OnPlayOutStart;
        public event Action<AbstractContentController> OnPlayOutComplete;
        
        
        
        public ContentGroupConfigVO ConfigVO { get; private set; }
        public Transform ContentGroupRoot { get; private set; }
        
        public ContentManager ContentManager { get; private set; }
        
        public AbstractContentController ActiveContentController { get; private set; }
        private AbstractContentController m_PendingContentController;

        private UpdateHandle m_UpdateHandle;

        public ContentGroup(ContentGroupConfigVO contentGroupGroupVO, ContentManager contentManager)
        {
            ConfigVO = contentGroupGroupVO;
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
            ContentGroupRoot = groupRootGO.transform;
            ContentGroupRoot.SetParent(ConfigVO.AlternativeGameObjectRoot == null ? ContentManager.ContentRoot : ConfigVO.AlternativeGameObjectRoot);
            ContentGroupRoot.localPosition = ConfigVO.LocalPosition;
            ContentGroupRoot.localRotation = Quaternion.identity;
            ContentGroupRoot.localScale = Vector3.one;
        }

        public void Show(AbstractContentController contentController)
        {
            //TODO: Validate the passed in controller to ensure we avoid weird cases such as:
            // - Showing the same instance that is already showing or about to be shown
            // - Might have a pending controller in the process of loading
            
            m_PendingContentController = contentController;

            //If there's an Active Controller currently being shown, we need to clear it.
            if (ActiveContentController != null)
            {
                OnPlayOutStart?.Invoke(ActiveContentController);
                ActiveContentController.InternalPlayOut();
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
            
            
            ActiveContentController = m_PendingContentController;
            m_PendingContentController = null;
            ActiveContentController.ContentGroup = this;

            OnLoadStart?.Invoke(ActiveContentController);
            ActiveContentController.OnLoadComplete += HandleOnLoadComplete;
            ActiveContentController.Load();
        }

        private void HandleOnLoadComplete()
        {
            ActiveContentController.OnLoadComplete -= HandleOnLoadComplete;
            OnLoadComplete?.Invoke(ActiveContentController);
            
            ActiveContentController.InternalInitAfterLoadComplete();

            AbstractContent content = ActiveContentController.GetContent<AbstractContent>();
            Transform transform = content.transform;
            transform.SetParent(ContentGroupRoot);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;

            AttachLifeCycleListeners(ActiveContentController);
            OnPlayInStart?.Invoke(ActiveContentController);
            ActiveContentController.InternalPlayIn();
        }

        private void AttachLifeCycleListeners(AbstractContentController contentController)
        {
            contentController.OnPlayInComplete += HandleOnPlayInComplete;
            contentController.OnPlayOutComplete += HandleOnPlayOutComplete;
            contentController.OnClear += HandleOnClear;
        }
        
        private void RemoveLifeCycleListeners(AbstractContentController contentController)
        {
            contentController.OnPlayInComplete -= HandleOnPlayInComplete;
            contentController.OnPlayOutComplete -= HandleOnPlayOutComplete;
            contentController.OnClear -= HandleOnClear;
        }

        private void HandleOnPlayInComplete()
        {
            ActiveContentController.OnPlayInComplete -= HandleOnPlayInComplete;
            OnPlayInComplete?.Invoke(ActiveContentController);

            ActiveContentController.InternalInitAfterPlayInComplete();
        }

        private void HandleOnPlayOutComplete()
        {
            if (ActiveContentController != null)
            {
                OnPlayOutComplete?.Invoke(ActiveContentController);
                RemoveLifeCycleListeners(ActiveContentController);
                ActiveContentController.Dispose();
                ActiveContentController = null;
            }

            ShowPendingContentController();
        }

        private void HandleOnClear()
        {
            Clear();
        }

        
        
        
    }
}


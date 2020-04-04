using System;
using Anvil.CSharp.Core;
using UnityEngine;

namespace Anvil.Unity.ContentManagement
{
    public abstract class AbstractContentController : AbstractAnvilDisposable
    {
        public event Action OnPlayInComplete;
        public event Action OnPlayOutComplete;
        public event Action OnLoadComplete;
        
        public ContentControllerConfigVO ConfigVO { get; private set; }

        private AbstractContentView m_ContentView;

        //TODO: Remove later on
        private ResourceRequest m_ResourceRequest;

        protected AbstractContentController()
        {
            ConfigVO = new ContentControllerConfigVO();
            InitConfigVO(ConfigVO);
        }

        protected override void DisposeSelf()
        {
            OnPlayInComplete = null;
            OnPlayOutComplete = null;
            OnLoadComplete = null;
            base.DisposeSelf();
        }

        public T GetContentView<T>() where T : AbstractContentView
        {
            return (T)m_ContentView;
        }

        protected abstract void InitConfigVO(ContentControllerConfigVO configVO);

        internal void Load()
        {
            //TODO: Need to load all the required assets.
            //TODO: Need to load the actual prefab or scene.
            //TODO: Need to allow the user to insert their own logic in here.
            
            //For now we'll just assume it's a prefab and we're Resources.Loading it.
            //TODO: Support addressables

            m_ResourceRequest = Resources.LoadAsync<GameObject>(ConfigVO.ContentLoadingID);
            m_ResourceRequest.completed += HandleOnResourceLoaded;
        }

        private void HandleOnResourceLoaded(AsyncOperation asyncOperation)
        {
            GameObject instance = (GameObject)GameObject.Instantiate(m_ResourceRequest.asset);
            //TODO: Properly sanitize the name with a Regex via util method
            instance.name = instance.name.Replace("(Clone)", string.Empty);
            m_ContentView = instance.GetComponent<AbstractContentView>();
            
            m_ResourceRequest.completed -= HandleOnResourceLoaded;
            OnLoadComplete?.Invoke();
        }

        public virtual void InitAfterLoadComplete()
        {
            
        }

        public abstract void PlayIn();
        
        protected virtual void PlayInComplete()
        {
            
        }

        public virtual void InitAfterPlayInComplete()
        {
            
        }
        
        public abstract void PlayOut();

        protected virtual void PlayOutComplete()
        {
            
        }
        
        

    }
}


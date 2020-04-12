using System;
using Anvil.CSharp.Core;
using UnityEngine;

namespace Anvil.Unity.Content
{
    public abstract class AbstractContentController<T> : AbstractContentController where T : AbstractContent
    {
        public new T Content
        {
            get { return (T)base.Content; }
        }

        public AbstractContentController(string contentGroupID, string contentLoadingID)
            : base(contentGroupID, contentLoadingID) { }
    }

    public abstract class AbstractContentController : AbstractAnvilDisposable
    {
        public event Action OnPlayInComplete;
        public event Action OnPlayOutComplete;
        public event Action OnLoadComplete;

        public event Action OnClear;


        public readonly string ContentGroupID;
        public readonly string ContentLoadingID;

        public AbstractContent Content { get; private set; }
        //TODO: Remove later on
        private ResourceRequest m_ResourceRequest;


        public ContentGroup ContentGroup { get; internal set; }
        internal bool IsContentControllerDisposing { get; private set; }

        protected AbstractContentController(string contentGroupID, string contentLoadingID)
        {
            ContentGroupID = contentGroupID;
            ContentLoadingID = contentLoadingID;
            //TODO: Handle overrides for additional loading dependency settings.
        }

        protected override void DisposeSelf()
        {
            if (IsContentControllerDisposing)
            {
                return;
            }
            IsContentControllerDisposing = true;
            
            
            OnPlayInComplete = null;
            OnPlayOutComplete = null;
            OnLoadComplete = null;
            OnClear = null;

            if (Content != null && !Content.IsContentDisposing)
            {
                Content.Dispose();
                Content = null;
            }
            
            base.DisposeSelf();
        }

        internal void Load()
        {
            //TODO: Need to load all the required assets.
            //TODO: Need to load the actual prefab or scene.
            //TODO: Need to allow the user to insert their own logic in here.
            
            //For now we'll just assume it's a prefab and we're Resources.Loading it.
            //TODO: Support addressables

            m_ResourceRequest = Resources.LoadAsync<GameObject>(ContentLoadingID);
            m_ResourceRequest.completed += HandleOnResourceLoaded;
        }

        private void HandleOnResourceLoaded(AsyncOperation asyncOperation)
        {
            GameObject instance = (GameObject)GameObject.Instantiate(m_ResourceRequest.asset);
            //TODO: Properly sanitize the name with a Regex via util method
            instance.name = instance.name.Replace("(Clone)", string.Empty);
            Content = instance.GetComponent<AbstractContent>();
            Content.Controller = this;
            
            m_ResourceRequest.completed -= HandleOnResourceLoaded;
            OnLoadComplete?.Invoke();
        }

        internal void InternalInitAfterLoadComplete()
        {
            InitAfterLoadComplete();
        }

        protected abstract void InitAfterLoadComplete();

        internal void InternalPlayIn()
        {
            PlayIn();
        }

        protected abstract void PlayIn();
        
        protected virtual void PlayInComplete()
        {
            OnPlayInComplete?.Invoke();
        }

        internal void InternalInitAfterPlayInComplete()
        {
            InitAfterPlayInComplete();
        }

        protected abstract void InitAfterPlayInComplete();

        internal void InternalPlayOut()
        {
            PlayOut();
        }
        
        protected abstract void PlayOut();

        protected virtual void PlayOutComplete()
        {
            OnPlayOutComplete?.Invoke();
        }

        public void Clear()
        {
            OnClear?.Invoke();
        }
        

    }
}


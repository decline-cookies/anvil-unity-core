using System;
using System.Collections.Generic;
using Anvil.CSharp.Core;
using UnityEngine;

namespace Anvil.Unity.Content
{
    public class ContentManager : AbstractAnvilDisposable
    {
        public event Action<AbstractContentController> OnLoadStart;
        public event Action<AbstractContentController> OnLoadComplete;
        public event Action<AbstractContentController> OnPlayInStart;
        public event Action<AbstractContentController> OnPlayInComplete;
        public event Action<AbstractContentController> OnPlayOutStart;
        public event Action<AbstractContentController> OnPlayOutComplete;
        
        private Dictionary<string, ContentGroup> m_ContentGroups = new Dictionary<string, ContentGroup>();
        
        public string ID { get; private set; }
        public Transform ContentRoot { get; private set; }

        public ContentManager(string id, Transform contentRoot)
        {
            ID = id;
            ContentRoot = contentRoot;
        }
        
        protected override void DisposeSelf()
        {
            OnLoadStart = null;
            OnLoadComplete = null;
            OnPlayInStart = null;
            OnPlayInComplete = null;
            OnPlayOutStart = null;
            OnPlayOutComplete = null;
            
            if (m_ContentGroups != null)
            {
                foreach (ContentGroup contentGroup in m_ContentGroups.Values)
                {
                    contentGroup.Dispose();
                }
                m_ContentGroups.Clear();
                m_ContentGroups = null;
            }
            
            base.DisposeSelf();
        }
        
        public ContentManager CreateContentGroup(ContentGroupConfigVO contentGroupConfigVO)
        {
            if (m_ContentGroups.ContainsKey(contentGroupConfigVO.ID))
            {
                throw new Exception($"Content Groups ID of {contentGroupConfigVO.ID} is already registered with the Content Manager with ID {ID}!");
            }
            
            ContentGroup contentGroup = new ContentGroup(contentGroupConfigVO, this);
            m_ContentGroups.Add(contentGroupConfigVO.ID, contentGroup);

            AddLifeCycleListeners(contentGroup);
            
            return this;
        }

        public ContentGroup GetContentGroup(string id)
        {
            if (!m_ContentGroups.ContainsKey(id))
            {
                throw new Exception($"Tried to get Content Group with ID {id} but none exists!");
            }

            return m_ContentGroups[id];
        }
        


        public void Show(AbstractContentController contentController)
        {
            string contentGroupID = contentController.ConfigVO.ContentGroupID;

            if (!m_ContentGroups.ContainsKey(contentGroupID))
            {
                throw new Exception($"ContentGroupID of {contentGroupID} does not exist in the Content Manager with ID {ID}. Did you add the Content Group?");
            }

            ContentGroup contentGroup = m_ContentGroups[contentGroupID];
            contentGroup.Show(contentController);
        }

        public void ClearContentGroup(string contentGroupID)
        {
            if (!m_ContentGroups.ContainsKey(contentGroupID))
            {
                throw new Exception($"ContentGroupID of {contentGroupID} does not exist in the Content Manager with ID {ID}. Did you add the Content Group?");
            }
            
            ContentGroup contentGroup = m_ContentGroups[contentGroupID];
            contentGroup.Clear();
        }

        private void AddLifeCycleListeners(ContentGroup contentGroup)
        {
            contentGroup.OnLoadStart += HandleOnLoadStart;
            contentGroup.OnLoadComplete += HandleOnLoadComplete;
            contentGroup.OnPlayInStart += HandleOnPlayInStart;
            contentGroup.OnPlayInComplete += HandleOnPlayInComplete;
            contentGroup.OnPlayOutStart += HandleOnPlayOutStart;
            contentGroup.OnPlayOutComplete += HandleOnPlayOutComplete;
        }

        private void HandleOnLoadStart(AbstractContentController contentController)
        {
            OnLoadStart?.Invoke(contentController);
        }
        
        private void HandleOnLoadComplete(AbstractContentController contentController)
        {
            OnLoadComplete?.Invoke(contentController);
        }
        
        private void HandleOnPlayInStart(AbstractContentController contentController)
        {
            OnPlayInStart?.Invoke(contentController);
        }
        
        private void HandleOnPlayInComplete(AbstractContentController contentController)
        {
            OnPlayInComplete?.Invoke(contentController);
        }
        
        private void HandleOnPlayOutStart(AbstractContentController contentController)
        {
            OnPlayOutStart?.Invoke(contentController);
        }
        
        private void HandleOnPlayOutComplete(AbstractContentController contentController)
        {
            OnPlayOutComplete?.Invoke(contentController);
        }
    }
}


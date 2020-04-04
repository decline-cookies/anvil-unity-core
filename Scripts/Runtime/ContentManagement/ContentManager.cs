using System;
using System.Collections.Generic;
using Anvil.CSharp.Core;
using UnityEngine;

namespace Anvil.Unity.ContentManagement
{
    public class ContentManager : AbstractAnvilDisposable
    {
        public event Action<AbstractContentController> OnLoadStart;
        public event Action<AbstractContentController> OnLoadComplete;
        public event Action<AbstractContentController> OnPlayInStart;
        public event Action<AbstractContentController> OnPlayInComplete;
        public event Action<AbstractContentController> OnPlayOutStart;
        public event Action<AbstractContentController> OnPlayOutComplete;
        
        private Dictionary<string, ContentLayer> m_ContentLayers = new Dictionary<string, ContentLayer>();
        
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
            
            if (m_ContentLayers != null)
            {
                foreach (ContentLayer contentLayer in m_ContentLayers.Values)
                {
                    contentLayer.Dispose();
                }
                m_ContentLayers.Clear();
                m_ContentLayers = null;
            }
            
            base.DisposeSelf();
        }
        
        public ContentManager CreateContentLayer(ContentLayerConfigVO contentLayerConfigVO)
        {
            if (m_ContentLayers.ContainsKey(contentLayerConfigVO.ID))
            {
                throw new Exception($"Content Layer ID of {contentLayerConfigVO.ID} is already registered with the Content Manager with ID {ID}!");
            }
            
            ContentLayer contentLayer = new ContentLayer(contentLayerConfigVO, this);
            m_ContentLayers.Add(contentLayerConfigVO.ID, contentLayer);

            AddLifeCycleListeners(contentLayer);
            
            return this;
        }

        public ContentLayer GetContentLayer(string id)
        {
            if (!m_ContentLayers.ContainsKey(id))
            {
                throw new Exception($"Tried to get Content Layer with ID {id} but none exists!");
            }

            return m_ContentLayers[id];
        }
        


        public void Show(AbstractContentController contentController)
        {
            string contentLayerID = contentController.ConfigVO.ContentLayerID;

            if (!m_ContentLayers.ContainsKey(contentLayerID))
            {
                throw new Exception($"ContentLayerID of {contentLayerID} does not exist in the Content Manager with ID {ID}. Did you add the Content Layer?");
            }

            ContentLayer contentLayer = m_ContentLayers[contentLayerID];
            contentLayer.Show(contentController);
        }

        public void ClearContentLayer(string contentLayerID)
        {
            if (!m_ContentLayers.ContainsKey(contentLayerID))
            {
                throw new Exception($"ContentLayerID of {contentLayerID} does not exist in the Content Manager with ID {ID}. Did you add the Content Layer?");
            }
            
            ContentLayer contentLayer = m_ContentLayers[contentLayerID];
            contentLayer.Clear();
        }

        private void AddLifeCycleListeners(ContentLayer contentLayer)
        {
            contentLayer.OnLoadStart += HandleOnLoadStart;
            contentLayer.OnLoadComplete += HandleOnLoadComplete;
            contentLayer.OnPlayInStart += HandleOnPlayInStart;
            contentLayer.OnPlayInComplete += HandleOnPlayInComplete;
            contentLayer.OnPlayOutStart += HandleOnPlayOutStart;
            contentLayer.OnPlayOutComplete += HandleOnPlayOutComplete;
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


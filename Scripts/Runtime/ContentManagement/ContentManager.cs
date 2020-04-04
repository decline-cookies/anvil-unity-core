using System;
using System.Collections.Generic;
using Anvil.CSharp.Core;
using UnityEngine;

namespace Anvil.Unity.ContentManagement
{
    public class ContentManager : AbstractAnvilDisposable
    {
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
    }
}


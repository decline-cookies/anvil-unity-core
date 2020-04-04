using System;
using System.Collections.Generic;
using Anvil.CSharp.Core;
using UnityEngine;

namespace Anvil.Unity.ContentManagement
{
    public class ContentManager : AbstractAnvilDisposable
    {
        private Dictionary<string, ContentGroup> m_ContentGroups = new Dictionary<string, ContentGroup>();

        protected override void DisposeSelf()
        {
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

        public void CreateContentGroup(ContentGroupConfigVO contentGroupConfigVO, Transform groupRoot)
        {
            if (m_ContentGroups.ContainsKey(contentGroupConfigVO.ID))
            {
                throw new Exception($"ContentGroupID of {contentGroupConfigVO.ID} is already registered with the Content Manager!");
            }
            
            ContentGroup contentGroup = new ContentGroup(contentGroupConfigVO, groupRoot);
            m_ContentGroups.Add(contentGroupConfigVO.ID, contentGroup);

            foreach (ContentLayerConfigVO contentLayerConfigVO in contentGroupConfigVO.ContentLayers.Values)
            {
                contentGroup.CreateContentLayer(contentLayerConfigVO);
            }
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
                throw new Exception($"ContentGroupID of {contentGroupID} does not exist in the Content Manager. Did you add the Content Group to the Content Manager?");
            }

            ContentGroup contentGroup = m_ContentGroups[contentGroupID];
            contentGroup.Show(contentController);
        }
    }
}


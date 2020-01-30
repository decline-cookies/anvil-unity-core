using System;
using System.Collections.Generic;
using Anvil.CSharp.Core;

namespace Anvil.Unity.Core
{
    public class ContentManager : AnvilAbstractDisposable
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


        public void Show(AbstractContentController contentController)
        {
            string contentGroupID = contentController.ConfigVO.ContentGroupID;
            if (string.IsNullOrEmpty(contentGroupID))
            {
                throw new Exception($"ContentGroupID is null or empty when trying to show {contentController}");
            }

            if (!m_ContentGroups.ContainsKey(contentGroupID))
            {
                throw new Exception($"ContentGroupID of {contentGroupID} does not exist in the Content Manager. Did you add the Content Group?");
            }

            ContentGroup contentGroup = m_ContentGroups[contentGroupID];
            contentGroup.Show(contentController);
        }
    }
}


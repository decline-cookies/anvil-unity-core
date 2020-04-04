using System;
using System.Collections.Generic;
using Anvil.CSharp.Data;

namespace Anvil.Unity.ContentManagement
{
    public class ContentGroupConfigVO : AbstractAnvilVO
    {
        public static ContentGroupConfigVO Create(string id)
        {
            ContentGroupConfigVO contentGroupConfigVO = new ContentGroupConfigVO()
            {
                ID = id
            };
            return contentGroupConfigVO;
        }
        
        public string ID { get; private set; }

        public readonly Dictionary<string, ContentLayerConfigVO> ContentLayers = new Dictionary<string, ContentLayerConfigVO>();

        private ContentGroupConfigVO()
        {
            
        }

        public ContentGroupConfigVO AddContentLayer(ContentLayerConfigVO contentLayerConfigVO)
        {
            if (ContentLayers.ContainsKey(contentLayerConfigVO.ID))
            {
                throw new Exception($"Tried to add a Content Layer with ID {contentLayerConfigVO.ID} to Content Group with ID {ID} but it already exists!");
            }

            ContentLayers.Add(contentLayerConfigVO.ID, contentLayerConfigVO);
            return this;
        }
    }
}


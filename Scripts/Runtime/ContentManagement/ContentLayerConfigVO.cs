using Anvil.CSharp.Data;
using UnityEngine;

namespace Anvil.Unity.ContentManagement
{
    public class ContentLayerConfigVO : AbstractAnvilVO
    {
        public static ContentLayerConfigVO Create(string id, Vector3 localPosition)
        {
            ContentLayerConfigVO contentLayerConfigVO = new ContentLayerConfigVO()
            {
                ID = id,
                LocalPosition = localPosition
            };
            return contentLayerConfigVO;
        }
        
        public string ID { get; private set; }
        public Vector3 LocalPosition { get; private set; }

        private ContentLayerConfigVO()
        {
            
        }
    }
}


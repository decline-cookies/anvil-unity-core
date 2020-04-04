using Anvil.CSharp.Data;

namespace Anvil.Unity.ContentManagement
{
    public class ContentControllerConfigVO : AbstractAnvilVO
    {
        public string ContentGroupID;
        public string ContentLayerID;
        public string ContentLoadingID;
        
        //TODO: Some way to denote that the loading ID is a prefab or a scene
        //TODO: Other dependencies to load before loading the Content
        //TODO: Support addressables
    }
}


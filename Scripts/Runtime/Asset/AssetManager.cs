using Anvil.CSharp.Core;

namespace Anvil.Unity.Asset
{
    public class AssetManager : AbstractAnvilDisposable
    {
        public LoadRequest CreateLoadRequest()
        {
            //Add this to some in flight loadrequests list
            return new LoadRequest();
        }
    }
}


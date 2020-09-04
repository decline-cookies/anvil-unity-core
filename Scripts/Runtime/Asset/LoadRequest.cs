using Anvil.CSharp.Core;

namespace Anvil.Unity.Asset
{
    public class LoadRequest : AbstractAnvilDisposable
    {
        public LoadRequest()
        {

        }

        protected override void DisposeSelf()
        {
            base.DisposeSelf();
        }

        public LoadRequest Add(string assetID)
        {
            //Lookup the assetID to get information such as the library and it's variant and to know if it's actually loaded or not already
            return this;
        }
    }
}


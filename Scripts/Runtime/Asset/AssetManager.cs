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

        public void SetVariantLevel()
        {
            //TODO: for a given library, set the strategy for which assets to use in its variants.
            //Ex - Characters -> Set "Quality" to "Ultra" (Quality has defined list of Ultra, High, Medium, Low. If Ultra doesn't exist, fallback to High).
            //Ex - Characters -> Set "Common" to "Common-EN_US"

            //If any libraries are currently loaded, they now need to be re-loaded to reflect the changes to the settings
        }

        public void LoadLibraries()
        {
            //TODO: for a given list of libraries, we want to load them and their variants into memory given their configured variant level.
            //If any of them don't exist on disk, we need to download them. Otherwise just loading into memory is fine.
            //Want to ensure that we keep it around if multiple places are going to request them.

        }

        public void LoadAssets()
        {
            //TODO: For a specific list of assets we want to load those assets into memory so that we can easily get instances or references to them at runtime.
        }

        public void GetAsset()
        {
            //TODO: For a given loaded asset, get a new instance of it OR reference to it.
        }
    }
}


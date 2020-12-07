namespace Anvil.Unity.Asset
{
    public class ResourcesVariant : AbstractVariant
    {
        public ResourcesVariant(VariantVO variantVO) : base(variantVO)
        {

        }

        protected override void DisposeSelf()
        {
            base.DisposeSelf();
        }

        protected override VariantLoadingStatus InitDefaultLoadingStatus()
        {
            return VariantLoadingStatus.OnDisk;
        }

        protected override void DownloadToDiskImpl()
        {
            //DOES NOTHING - Internal to Resources
        }

        protected override void DeleteFromDiskImpl()
        {
            //DOES NOTHING - Internal to Resources
        }

        protected override void LoadIntoMemoryImpl()
        {
            //TODO: Take the Asset manifest from the VO and register with the global lookups.
            //Trigger the completion event
        }

        protected override void UnloadFromMemoryImpl()
        {
            //TODO: Take the Asset manifest from the VO and unregister with the global lookups
            //Trigger the completion event
        }
    }
}

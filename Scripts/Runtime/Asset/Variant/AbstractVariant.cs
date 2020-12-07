using Anvil.CSharp.Core;

namespace Anvil.Unity.Asset
{
    public abstract class AbstractVariant : AbstractAnvilDisposable
    {
        private readonly VariantVO m_VariantVO;

        public VariantLoadingStatus LoadingStatus
        {
            get;
            protected set;
        }

        protected AbstractVariant(VariantVO variantVO)
        {
            m_VariantVO = variantVO;
            LoadingStatus = InitDefaultLoadingStatus();
        }

        protected override void DisposeSelf()
        {
            base.DisposeSelf();
        }

        public void DownloadToDisk()
        {
            LoadingStatus = VariantLoadingStatus.DownloadingToDisk;
            DownloadToDiskImpl();
        }

        public void DeleteFromDisk()
        {
            LoadingStatus = VariantLoadingStatus.DeletingFromDisk;
            DeleteFromDiskImpl();
        }

        public void LoadIntoMemory()
        {
            LoadingStatus = VariantLoadingStatus.LoadingIntoMemory;
            LoadIntoMemoryImpl();
        }

        public void UnloadFromMemory()
        {
            LoadingStatus = VariantLoadingStatus.UnloadingFromMemory;
            UnloadFromMemoryImpl();
        }

        protected abstract VariantLoadingStatus InitDefaultLoadingStatus();
        protected abstract void DownloadToDiskImpl();
        protected abstract void DeleteFromDiskImpl();
        protected abstract void LoadIntoMemoryImpl();
        protected abstract void UnloadFromMemoryImpl();

    }
}


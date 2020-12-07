namespace Anvil.Unity.Asset
{
    public enum VariantLoadingStatus
    {
        None,
        DownloadingToDisk,
        OnDisk,
        DeletingFromDisk,
        LoadingIntoMemory,
        InMemory,
        UnloadingFromMemory
    }
}


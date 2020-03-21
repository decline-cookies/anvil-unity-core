using Anvil.CSharp.Core;

namespace Anvil.Unity.ContentManagement
{
    public abstract class AbstractContentController : AbstractAnvilDisposable
    {
        public ContentControllerConfigVO ConfigVO { get; private set; }
    }
}


using Anvil.CSharp.Core;

namespace Anvil.Unity.Core
{
    public abstract class AbstractContentController : AnvilAbstractDisposable
    {
        public ContentControllerConfigVO ConfigVO { get; private set; }
    }
}


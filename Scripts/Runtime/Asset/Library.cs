using Anvil.CSharp.Core;

namespace Anvil.Unity.Asset
{
    public class Library : AbstractAnvilDisposable
    {
        private LibraryVO m_LibraryVO;

        public Library(LibraryVO libraryVO)
        {
            m_LibraryVO = libraryVO;
        }

        protected override void DisposeSelf()
        {
            base.DisposeSelf();
        }

        public void LoadIntoMemory()
        {
            //TODO: For all variants that match our current variant rules, load them into memory if they aren't already
        }
    }
}


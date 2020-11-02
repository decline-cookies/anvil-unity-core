using Anvil.CSharp.Data;
using Anvil.UnityEditor.Data;

namespace Anvil.UnityEditor.Asset
{
    public class LibraryCreationPathVO : AbstractAnvilVO, IEditorSectionVO<LibraryCreationPathVO>
    {
        public string Name;
        public readonly EditorPathVO Path;

        public bool IsBeingEdited
        {
            get;
            set;
        }

        public LibraryCreationPathVO()
        {
            Path = new EditorPathVO();
        }

        public void CopyInto(LibraryCreationPathVO other)
        {
            other.Name = Name;
            other.Path.AssetsRelativePath = Path.AssetsRelativePath;
        }
    }
}


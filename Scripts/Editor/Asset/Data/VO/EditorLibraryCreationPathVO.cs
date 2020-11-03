using Anvil.CSharp.Data;
using Anvil.UnityEditor.Data;

namespace Anvil.UnityEditor.Asset
{
    public class EditorLibraryCreationPathVO : AbstractAnvilVO, IEditorSectionVO<EditorLibraryCreationPathVO>
    {
        public string Name;
        public readonly EditorPathVO Path;

        public bool IsBeingEdited
        {
            get;
            set;
        }

        public EditorLibraryCreationPathVO()
        {
            Path = new EditorPathVO();
        }

        public void CopyInto(EditorLibraryCreationPathVO other)
        {
            other.Name = Name;
            other.Path.AssetsRelativePath = Path.AssetsRelativePath;
        }
    }
}


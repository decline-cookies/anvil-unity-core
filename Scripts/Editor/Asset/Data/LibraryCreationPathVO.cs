using Anvil.CSharp.Data;
using Anvil.UnityEditor.Data;
using TinyJSON;

namespace Anvil.UnityEditor.Asset
{
    public class LibraryCreationPathVO : AbstractAnvilVO
    {
        [Exclude] public bool IsBeingEdited;

        public string Name;
        public readonly EditorPathVO Path;

        public LibraryCreationPathVO()
        {
            Path = new EditorPathVO();
        }
    }
}


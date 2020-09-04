using Anvil.CSharp.Data;
using TinyJSON;

namespace Anvil.UnityEditor.Asset
{
    public class LibraryCreationPathVO : AbstractAnvilVO
    {
        [Exclude] public bool IsBeingEdited;

        public string Name;
        public string AssetsRelativePath;

        public LibraryCreationPathVO()
        {

        }
    }
}


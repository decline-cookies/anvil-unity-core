using System.Collections.Generic;
using Anvil.CSharp.Data;

namespace Anvil.UnityEditor.Asset
{
    public class AssetManagementConfigVO : AbstractAnvilVO
    {
        public int DefaultLibraryCreationPathIndex;
        public readonly List<LibraryCreationPathVO> LibraryCreationPaths;

        public AssetManagementConfigVO()
        {
            LibraryCreationPaths = new List<LibraryCreationPathVO>();
        }
    }
}


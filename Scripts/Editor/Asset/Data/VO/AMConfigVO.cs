using System.Collections.Generic;
using Anvil.CSharp.Data;

namespace Anvil.UnityEditor.Asset
{
    public class AMConfigVO : AbstractAnvilVO
    {
        public int DefaultLibraryCreationPathIndex;
        public readonly List<EditorLibraryCreationPathVO> LibraryCreationPaths;

        public int DefaultVariantPresetIndex;
        public readonly List<EditorVariantPresetVO> VariantPresets;

        public AMConfigVO()
        {
            LibraryCreationPaths = new List<EditorLibraryCreationPathVO>();
            VariantPresets = new List<EditorVariantPresetVO>();
        }
    }
}


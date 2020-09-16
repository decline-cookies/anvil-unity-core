using System.Collections.Generic;
using Anvil.CSharp.Data;

namespace Anvil.UnityEditor.Asset
{
    public class EditorLibraryVO : AbstractAnvilVO
    {
        public string Name;
        public List<EditorLibraryVariantVO> Variants;

        public EditorLibraryVO()
        {
            Variants = new List<EditorLibraryVariantVO>();
        }
    }
}


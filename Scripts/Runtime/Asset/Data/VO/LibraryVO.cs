using System.Collections.Generic;
using Anvil.CSharp.Data;

namespace Anvil.Unity.Asset
{
    public class LibraryVO : AbstractAnvilVO
    {
        public string ID;
        public string VariantFallbackID;
        public readonly Dictionary<string, VariantVO> Variants;

        public LibraryVO()
        {
            Variants = new Dictionary<string, VariantVO>();
        }
    }
}


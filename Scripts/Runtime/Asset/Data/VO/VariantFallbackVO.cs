using System.Collections.Generic;
using Anvil.CSharp.Data;

namespace Anvil.Unity.Asset
{
    public class VariantFallbackVO : AbstractAnvilVO
    {
        public string ID;
        public readonly List<string> FallbackVariantIDs;

        public VariantFallbackVO()
        {
            FallbackVariantIDs = new List<string>();
        }
    }
}


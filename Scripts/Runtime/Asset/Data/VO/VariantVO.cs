using System.Collections.Generic;
using Anvil.CSharp.Data;

namespace Anvil.Unity.Asset
{
    public class VariantVO : AbstractAnvilVO
    {
        public string ID;
        public VariantType Type;
        public readonly Dictionary<string, AssetVO> Assets;

        public VariantVO()
        {
            Assets = new Dictionary<string, AssetVO>();
        }
    }
}


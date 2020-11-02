using System.Collections.Generic;
using Anvil.CSharp.Data;

namespace Anvil.UnityEditor.Asset
{
    public class EditorLibrarySourceVariantVO : AbstractAnvilVO
    {
        public string Name;
        public readonly List<EditorLibraryPublishedVariantVO> PublishedVariants;

        public EditorLibrarySourceVariantVO()
        {
            PublishedVariants = new List<EditorLibraryPublishedVariantVO>();
        }
    }
}


using System.Collections.Generic;
using Anvil.CSharp.Data;
using Anvil.UnityEditor.Data;

namespace Anvil.UnityEditor.Asset
{
    public class EditorLibrarySourceVariantVO : AbstractAnvilVO, IEditorSectionVO<EditorLibrarySourceVariantVO>
    {
        public string Name;
        public readonly List<EditorLibraryPublishedVariantVO> PublishedVariants;

        public bool IsBeingEdited
        {
            get;
            set;
        }

        public EditorLibrarySourceVariantVO()
        {
            PublishedVariants = new List<EditorLibraryPublishedVariantVO>();
        }

        public void CopyInto(EditorLibrarySourceVariantVO other)
        {
            other.Name = Name;
            other.PublishedVariants.Clear();
            foreach (EditorLibraryPublishedVariantVO publishedVariantVO in other.PublishedVariants)
            {
                EditorLibraryPublishedVariantVO otherPublishedVariantVO = new EditorLibraryPublishedVariantVO
                {
                    Name = publishedVariantVO.Name,
                    Type = publishedVariantVO.Type
                };

                other.PublishedVariants.Add(otherPublishedVariantVO);
            }
        }
    }
}


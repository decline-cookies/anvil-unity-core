using System.Collections.Generic;
using Anvil.CSharp.Data;
using TinyJSON;

namespace Anvil.UnityEditor.Asset
{
    public class EditorVariantPresetVO : AbstractAnvilVO
    {
        [Exclude] public bool IsBeingEdited;

        public string Name;
        public readonly List<EditorLibrarySourceVariantVO> SourceVariants;

        public EditorVariantPresetVO()
        {
            SourceVariants = new List<EditorLibrarySourceVariantVO>();
        }

        public void CopyInto(EditorVariantPresetVO other)
        {
            other.Name = Name;
            other.SourceVariants.Clear();
            foreach (EditorLibrarySourceVariantVO sourceVariantVO in SourceVariants)
            {
                EditorLibrarySourceVariantVO otherSourceVariantVO = new EditorLibrarySourceVariantVO
                {
                    Name = sourceVariantVO.Name
                };

                foreach (EditorLibraryPublishedVariantVO publishedVariantVO in sourceVariantVO.PublishedVariants)
                {
                    EditorLibraryPublishedVariantVO otherPublishedVariantVO = new EditorLibraryPublishedVariantVO
                    {
                        Name = publishedVariantVO.Name,
                        Type = publishedVariantVO.Type
                    };

                    otherSourceVariantVO.PublishedVariants.Add(otherPublishedVariantVO);
                }

                other.SourceVariants.Add(otherSourceVariantVO);
            }
        }
    }
}


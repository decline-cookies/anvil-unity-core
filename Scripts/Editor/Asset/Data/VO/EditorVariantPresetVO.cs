using System.Collections.Generic;
using Anvil.CSharp.Data;
using Anvil.UnityEditor.Data;

namespace Anvil.UnityEditor.Asset
{
    public class EditorVariantPresetVO : AbstractAnvilVO, IEditorSectionVO<EditorVariantPresetVO>
    {
        public string Name;
        public readonly List<EditorLibrarySourceVariantVO> SourceVariants;

        public bool IsBeingEdited
        {
            get;
            set;
        }

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
                EditorLibrarySourceVariantVO otherSourceVariantVO = new EditorLibrarySourceVariantVO();
                sourceVariantVO.CopyInto(otherSourceVariantVO);

                other.SourceVariants.Add(otherSourceVariantVO);
            }
        }
    }
}


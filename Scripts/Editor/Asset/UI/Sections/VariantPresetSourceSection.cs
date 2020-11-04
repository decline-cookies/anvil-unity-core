using System.Collections.Generic;
using Anvil.UnityEditor.IMGUI;

namespace Anvil.UnityEditor.Asset
{
    public class VariantPresetSourceSection : AbstractAnvilIMGUISection<EditorLibrarySourceVariantVO>
    {
        private const int DEFAULT_INDEX = -1;
        private const string CONTROL_SOURCE_VARIANT_NAME = "CN_SOURCE_VARIANT_NAME";

        public VariantPresetSourceSection(List<EditorLibrarySourceVariantVO> listVOs)
            : base("Source Variants", DEFAULT_INDEX, CONTROL_SOURCE_VARIANT_NAME, listVOs)
        {
        }

        protected override void PreDraw()
        {

        }

        protected override void PostDraw()
        {

        }

        protected override void DrawVOInEditMode(EditorLibrarySourceVariantVO vo, int index, out bool shouldCancel, out bool shouldValidate)
        {
            shouldCancel = false;
            shouldValidate = false;
        }

        protected override void DrawVOInViewMode(EditorLibrarySourceVariantVO vo, int index)
        {

        }

        protected override bool ValidateVO(EditorLibrarySourceVariantVO vo)
        {
            //TODO: Actually validate
            return true;
        }

        protected override void HandleOnVOCreateComplete(EditorLibrarySourceVariantVO vo)
        {

        }

        protected override void HandleOnVOCreateStart(EditorLibrarySourceVariantVO vo)
        {

        }

        protected override void HandleOnVOCreateCancel(EditorLibrarySourceVariantVO vo)
        {

        }

        protected override void HandleOnVOEditStart(EditorLibrarySourceVariantVO vo)
        {

        }

        protected override void HandleOnVOEditCancel(EditorLibrarySourceVariantVO vo)
        {

        }

        protected override void HandleOnVOEditComplete(EditorLibrarySourceVariantVO vo)
        {

        }

        protected override void HandleOnVORemoved(EditorLibrarySourceVariantVO vo)
        {

        }
    }
}


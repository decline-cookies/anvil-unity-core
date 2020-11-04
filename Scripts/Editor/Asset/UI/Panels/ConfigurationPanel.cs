using Anvil.UnityEditor.IMGUI;
using UnityEditor;

namespace Anvil.UnityEditor.Asset
{
    public class ConfigurationPanel : AbstractAMEditorPanel
    {
        private const string TAB_NAME = "Configuration";

        private readonly LibraryCreationPathSection m_LibraryCreationPathSection;
        private readonly VariantPresetSection m_VariantPresetSection;

        public override string TabName
        {
            get => TAB_NAME;
        }

        public ConfigurationPanel()
        {
            m_LibraryCreationPathSection = CreateSection<LibraryCreationPathSection>();
            m_VariantPresetSection = CreateSection<VariantPresetSection>();
        }

        public override void Enable()
        {
            base.Enable();
        }

        public override void Draw()
        {
            AnvilIMGUI.Header("Configure Asset Management System");

            m_LibraryCreationPathSection.Draw();

            AnvilIMGUI.FieldSpacer();

            m_VariantPresetSection.Draw();

            base.Draw();
        }
    }
}


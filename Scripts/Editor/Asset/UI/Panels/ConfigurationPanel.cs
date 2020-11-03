using Anvil.UnityEditor.IMGUI;
using UnityEditor;

namespace Anvil.UnityEditor.Asset
{
    public class ConfigurationPanel : AbstractAMEditorPanel
    {
        private const string TAB_NAME = "Configuration";

        private readonly AMConfigVO m_ConfigVO;

        private readonly LibraryCreationPathSection m_LibraryCreationPathSection;
        private readonly VariantPresetSection m_VariantPresetSection;

        public override string TabName
        {
            get => TAB_NAME;
        }

        public ConfigurationPanel()
        {
            m_ConfigVO = AMController.Instance.AMConfigVO;

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

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            DrawLibraryCreationSection();

            EditorGUILayout.EndVertical();

            base.Draw();
        }

        private void DrawLibraryCreationSection()
        {
            AnvilIMGUI.Header("Library Creation");

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            m_LibraryCreationPathSection.Draw();

            EditorGUILayout.EndVertical();

            AnvilIMGUI.FieldSpacer();

            AnvilIMGUI.Header("Variant Presets");

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            m_VariantPresetSection.Draw();

            EditorGUILayout.EndVertical();
        }
    }
}


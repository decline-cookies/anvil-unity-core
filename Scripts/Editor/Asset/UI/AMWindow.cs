using UnityEditor;
using UnityEngine;

namespace Anvil.UnityEditor.Asset
{
    public class AMWindow : EditorWindow
    {
        private const string WINDOW_NAME = "Anvil Asset Management";
        private static readonly GUIContent s_TitleContent = new GUIContent(WINDOW_NAME);


        [MenuItem("Anvil/AssetManagement/Open Asset Management Window", false, 0)]
        private static void OpenAssetManagementWindow()
        {
            AMWindow window = GetWindow<AMWindow>();
            window.Show();
        }

        private int m_ActivePanelIndex;
        private AbstractAMEditorPanel[] m_Panels;
        private string[] m_PanelNames;
        private AbstractAMEditorPanel m_ActivePanel;

        private void OnEnable()
        {
            titleContent = s_TitleContent;

            m_Panels = new AbstractAMEditorPanel[]
            {
                new ConfigurationPanel(),
                new LibrariesPanel()
            };

            m_PanelNames = new string[m_Panels.Length];
            for (int i = 0; i < m_Panels.Length; ++i)
            {
                m_PanelNames[i] = m_Panels[i].TabName;
            }

            //TODO: Store last active panel in user settings
            m_ActivePanel = m_Panels[0];
            m_ActivePanel.Enable();
        }

        private void DrawTabs()
        {
            EditorGUI.BeginChangeCheck();
            m_ActivePanelIndex = GUILayout.Toolbar(m_ActivePanelIndex, m_PanelNames, GUILayout.ExpandWidth(true), GUILayout.MaxWidth(600));
            if (EditorGUI.EndChangeCheck())
            {
                m_ActivePanel?.Disable();
                m_ActivePanel = m_Panels[m_ActivePanelIndex];
                m_ActivePanel.Enable();
            }
        }

        private void OnGUI()
        {
            DrawTabs();

            m_ActivePanel.Draw();
        }
    }
}


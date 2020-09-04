using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Anvil.UnityEditor.Asset
{
    public class AssetManagementWindow : EditorWindow
    {
        private const string WINDOW_NAME = "Anvil Asset Management";
        private static readonly GUIContent s_TitleContent = new GUIContent(WINDOW_NAME);


        [MenuItem("Anvil/AssetManagement/Open Asset Management Window", false, 0)]
        private static void OpenAssetManagementWindow()
        {
            AssetManagementWindow window = GetWindow<AssetManagementWindow>();
            window.Show();
        }

        private int m_ActivePanelIndex;
        private AbstractAssetManagementEditorPanel[] m_Panels;
        private string[] m_PanelNames;

        private void OnEnable()
        {
            titleContent = s_TitleContent;

            m_Panels = new AbstractAssetManagementEditorPanel[]
            {
                new ConfigurationPanel(),
                new LibrariesPanel()
            };

            m_PanelNames = new string[m_Panels.Length];
            for (int i = 0; i < m_Panels.Length; ++i)
            {
                m_PanelNames[i] = m_Panels[i].TabName;
            }
        }

        private void DrawTabs()
        {
            m_ActivePanelIndex = GUILayout.Toolbar(m_ActivePanelIndex, m_PanelNames, GUILayout.ExpandWidth(true), GUILayout.MaxWidth(600));
        }

        private void OnGUI()
        {
            DrawTabs();

            m_Panels[m_ActivePanelIndex].Draw();
        }
    }
}


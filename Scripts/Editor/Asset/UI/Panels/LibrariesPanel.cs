using System.Collections.Generic;
using System.Linq;
using Anvil.UnityEditor.Data;
using Anvil.UnityEditor.IMGUI;
using Anvil.UnityEditor.Util;
using UnityEditor;
using UnityEngine;

namespace Anvil.UnityEditor.Asset
{
    public class LibrariesPanel : AbstractAMEditorPanel
    {
        private const string TAB_NAME = "Libraries";


        private readonly AMConfigVO m_ConfigVO;

        private EditorLibraryVO m_EditorLibraryVO;


        private string[] m_Locations;
        private int m_LocationIndex;

        public override string TabName
        {
            get => TAB_NAME;
        }

        public LibrariesPanel()
        {
            m_ConfigVO = AMController.Instance.AMConfigVO;
        }

        public override void Enable()
        {
            m_Locations = m_ConfigVO.LibraryCreationPaths
                                    .Select(o => $"{o.Name} - ({AnvilEditorUtil.ConvertToEditorSafeSlash(o.Path.AssetsRelativePath)})")
                                    .ToArray();
            m_LocationIndex = m_ConfigVO.DefaultLibraryCreationPathIndex;

            base.Enable();
        }

        public override void Draw()
        {
            AnvilIMGUI.Header("Create New Libraries");

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            if (m_EditorLibraryVO == null)
            {
                EditorGUILayout.HelpBox("No Libraries, please create some.", MessageType.Warning);
                if (AnvilIMGUI.SmallButton("New"))
                {
                    m_EditorLibraryVO = new EditorLibraryVO();
                }
            }
            else
            {
                AnvilIMGUI.Header("New Library");

                //TODO: Location
                m_LocationIndex = EditorGUILayout.Popup("Location:", m_LocationIndex, m_Locations);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Library Name:", GUILayout.MaxWidth(150));
                AnvilIMGUI.TightLabel(AnvilEditorAMConstants.KEYWORD_LIBRARY, AnvilIMGUIConstants.STYLE_LABEL_RIGHT_JUSTIFIED);
                m_EditorLibraryVO.Name = EditorGUILayout.TextField(m_EditorLibraryVO.Name, GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();

                AnvilIMGUI.FieldSpacer();

                AnvilIMGUI.Header("Variants");

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                List<EditorLibrarySourceVariantVO> toBeRemoved = new List<EditorLibrarySourceVariantVO>();

                foreach (EditorLibrarySourceVariantVO variantVO in m_EditorLibraryVO.Variants)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Variant Name:", GUILayout.MaxWidth(150));
                    AnvilIMGUI.TightLabel(AnvilEditorAMConstants.KEYWORD_VARIANT, AnvilIMGUIConstants.STYLE_LABEL_RIGHT_JUSTIFIED);
                    variantVO.Name = EditorGUILayout.TextField(variantVO.Name, GUILayout.ExpandWidth(true));
                    if (AnvilIMGUI.SmallButton("Remove"))
                    {
                        toBeRemoved.Add(variantVO);
                    }
                    EditorGUILayout.EndHorizontal();
                }

                foreach (EditorLibrarySourceVariantVO variantVO in toBeRemoved)
                {
                    m_EditorLibraryVO.Variants.Remove(variantVO);
                }
                toBeRemoved.Clear();

                if (AnvilIMGUI.SmallButton("New"))
                {
                    m_EditorLibraryVO.Variants.Add(new EditorLibrarySourceVariantVO());
                }


                EditorGUILayout.EndVertical();




                EditorGUILayout.BeginHorizontal();
                if (AnvilIMGUI.SmallButton("Cancel"))
                {
                    m_EditorLibraryVO = null;
                }
                if (AnvilIMGUI.SmallButton("Save"))
                {
                    //TODO: Actually save it
                    m_EditorLibraryVO = null;
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.Separator();

            base.Draw();
        }
    }
}


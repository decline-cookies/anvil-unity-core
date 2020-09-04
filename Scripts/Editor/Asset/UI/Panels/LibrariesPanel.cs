using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Anvil.UnityEditor.Asset
{
    public class LibrariesPanel : AbstractAssetManagementEditorPanel
    {
        private const string TAB_NAME = "Libraries";

        private EditorLibraryVO m_EditorLibraryVO;

        private string[] m_Options;
        private int m_Index;

        public override string TabName
        {
            get => TAB_NAME;
        }

        public LibrariesPanel()
        {
            m_Options = new []
            {
                "Assets",
                "SomeOtherLocationInner",
                "Test"
            };
            m_Index = 0;
        }

        public override void Draw()
        {
            base.Draw();

            EditorGUILayout.LabelField("Create New Libraries", EditorStyles.boldLabel);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            if (m_EditorLibraryVO == null)
            {
                if (GUILayout.Button("Create", GUILayout.Width(100)))
                {
                    m_EditorLibraryVO = new EditorLibraryVO();
                }
            }
            else
            {
                EditorGUILayout.LabelField("New Library", EditorStyles.boldLabel);

                //TODO: Location
                m_Index = EditorGUILayout.Popup("Location:", m_Index, m_Options);

                EditorGUILayout.BeginHorizontal();
                m_EditorLibraryVO.Name = EditorGUILayout.TextField("Library Name:", m_EditorLibraryVO.Name, GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.LabelField("Variants", EditorStyles.boldLabel);

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                List<EditorLibraryVariantVO> toBeRemoved = new List<EditorLibraryVariantVO>();

                foreach (EditorLibraryVariantVO variantVO in m_EditorLibraryVO.Variants)
                {
                    EditorGUILayout.BeginHorizontal();
                    variantVO.Name = EditorGUILayout.TextField("Variant Name:", variantVO.Name, GUILayout.ExpandWidth(true));
                    if (GUILayout.Button("Remove", EditorStyles.miniButton, GUILayout.Width(60)))
                    {
                        toBeRemoved.Add(variantVO);
                    }
                    EditorGUILayout.EndHorizontal();
                }

                foreach (EditorLibraryVariantVO variantVO in toBeRemoved)
                {
                    m_EditorLibraryVO.Variants.Remove(variantVO);
                }
                toBeRemoved.Clear();

                if (GUILayout.Button("New", EditorStyles.miniButton, GUILayout.Width(60)))
                {
                    m_EditorLibraryVO.Variants.Add(new EditorLibraryVariantVO());
                }


                EditorGUILayout.EndVertical();




                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Cancel", GUILayout.Width(100)))
                {
                    m_EditorLibraryVO = null;
                }
                if (GUILayout.Button("Save", GUILayout.Width(100)))
                {
                    //TODO: Actually save it
                    m_EditorLibraryVO = null;
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();


            EditorGUILayout.Separator();
        }
    }
}


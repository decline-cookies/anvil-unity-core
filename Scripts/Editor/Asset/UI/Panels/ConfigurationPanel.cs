using System.Collections.Generic;
using System.IO;
using System.Linq;
using Anvil.UnityEditor.IMGUI;
using Anvil.UnityEditor.Util;
using UnityEditor;
using UnityEngine;

namespace Anvil.UnityEditor.Asset
{
    public class ConfigurationPanel : AbstractAssetManagementEditorPanel
    {
        private const string TAB_NAME = "Configuration";

        private string[] m_LibraryCreationNames;
        private readonly AssetManagementConfigVO m_ConfigVO;

        private LibraryCreationPathVO m_PendingLibraryCreationPathVO;

        public override string TabName
        {
            get => TAB_NAME;
        }

        public ConfigurationPanel()
        {
            m_ConfigVO = AssetManagementController.Instance.AssetManagementConfigVO;
            UpdateLibraryCreationPaths(m_ConfigVO.LibraryCreationPaths);
        }

        public override void Draw()
        {
            base.Draw();

            Header("Configure Asset Management System");

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            DrawLibraryCreationSection();

            EditorGUILayout.EndVertical();
        }

        private void DrawLibraryCreationSection()
        {
            Header("Library Creation");

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            DrawDefaultLibraryCreationPath();
            DrawLibraryCreationPaths();
        }

        private void DrawDefaultLibraryCreationPath()
        {
            if (m_ConfigVO.LibraryCreationPaths.Count == 0)
            {
                EditorGUILayout.HelpBox("No Library Creation Paths, please create some.", MessageType.Warning);
                return;
            }

            EditorGUILayout.LabelField("Default Library Creation Path:");

            EditorGUILayout.BeginHorizontal();

            m_ConfigVO.DefaultLibraryCreationPathIndex = EditorGUILayout.Popup(m_ConfigVO.DefaultLibraryCreationPathIndex,
                                                                               m_LibraryCreationNames,
                                                                               GUILayout.Width(200));

            LibraryCreationPathVO defaultCreationPathVO = m_ConfigVO.LibraryCreationPaths[m_ConfigVO.DefaultLibraryCreationPathIndex];

            EditorGUILayout.LabelField($"Path: Assets/{defaultCreationPathVO.Path.AssetsRelativePath}");

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();
        }

        private void DrawLibraryCreationPaths()
        {
            EditorGUILayout.LabelField("Library Creation Paths:");

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            foreach (LibraryCreationPathVO pathVO in m_ConfigVO.LibraryCreationPaths)
            {
                DrawLibraryCreationPathVO(pathVO);
            }
            DrawLibraryCreationPathVO(m_PendingLibraryCreationPathVO);

            EditorGUILayout.Separator();

            if (SmallButton("New"))
            {
                m_PendingLibraryCreationPathVO = new LibraryCreationPathVO
                {
                    IsBeingEdited = true
                };
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();
        }

        private void DrawLibraryCreationPathVO(LibraryCreationPathVO pathVO)
        {
            if (pathVO == null)
            {
                return;
            }

            EditorGUILayout.BeginHorizontal();
            if (pathVO.IsBeingEdited)
            {
                DrawLibraryCreationPathVOInEditMode(pathVO);
            }
            else
            {
                DrawLibraryCreationPathVOInViewMode(pathVO);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawLibraryCreationPathVOInEditMode(LibraryCreationPathVO pathVO)
        {
            bool shouldValidate = false;
            bool shouldCancel = false;
            bool shouldTab = false;

            TightLabel("Name:", EditorStyles.boldLabel);
            pathVO.Name = KeyboardTextField("NameField",
                                            pathVO.Name,
                                            ref shouldValidate,
                                            ref shouldCancel,
                                            ref shouldTab,
                                            GUILayout.ExpandWidth(true));
            FieldSpacer();


            TightLabel("Path:", EditorStyles.boldLabel);
            TightLabel("Assets/", AnvilIMGUIConstants.STYLE_LABEL_RIGHT_JUSTIFIED);
            pathVO.Path.AssetsRelativePath = KeyboardTextField("PathField",
                                                          pathVO.Path.AssetsRelativePath,
                                                          ref shouldValidate,
                                                          ref shouldCancel,
                                                          ref shouldTab,
                                                          GUILayout.ExpandWidth(true));
            FieldSpacer();


            if (SmallButton("Cancel"))
            {
                shouldCancel = true;
            }

            if (SmallButton("Done"))
            {
                shouldValidate = true;
            }

            if (shouldCancel)
            {
                Debug.Log("CANCEL");
                pathVO.IsBeingEdited = false;
                m_PendingLibraryCreationPathVO = null;

            }
            else if (shouldValidate)
            {
                Debug.Log("VALIDATE");
                pathVO.IsBeingEdited = false;
                if (pathVO == m_PendingLibraryCreationPathVO)
                {
                    m_ConfigVO.LibraryCreationPaths.Add(pathVO);
                    m_PendingLibraryCreationPathVO = null;
                }
                UpdateLibraryCreationPaths(m_ConfigVO.LibraryCreationPaths);
            }
            else if (shouldTab)
            {
                Debug.Log($"TAB - Currently on {GUI.GetNameOfFocusedControl()}");

                Debug.Log($"Next Name {GUI.GetNameOfFocusedControl()}");
            }
        }

        private void DrawLibraryCreationPathVOInViewMode(LibraryCreationPathVO pathVO)
        {
            TightLabel("Name:", EditorStyles.boldLabel);
            EditorGUILayout.LabelField(pathVO.Name, GUILayout.ExpandWidth(true));
            FieldSpacer();

            TightLabel("Path:", EditorStyles.boldLabel);
            TightLabel($"Assets{Path.DirectorySeparatorChar}", AnvilIMGUIConstants.STYLE_LABEL_RIGHT_JUSTIFIED);
            EditorGUILayout.LabelField(pathVO.Path.AssetsRelativePath, GUILayout.ExpandWidth(true));
            FieldSpacer();

            SmallButtonSpacer();

            if (SmallButton("Edit"))
            {
                pathVO.IsBeingEdited = true;
            }
        }

        private void UpdateLibraryCreationPaths(IEnumerable<LibraryCreationPathVO> libraryCreationPathVOs)
        {
            m_LibraryCreationNames = libraryCreationPathVOs.Select(o => o.Name)
                                                           .ToArray();
        }
    }
}


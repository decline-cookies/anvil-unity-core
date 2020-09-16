using System.Collections.Generic;
using System.IO;
using System.Linq;
using Anvil.UnityEditor.IMGUI;
using UnityEditor;
using UnityEngine;

namespace Anvil.UnityEditor.Asset
{
    public class ConfigurationPanel : AbstractAssetManagementEditorPanel
    {
        private const string TAB_NAME = "Configuration";
        private const int DEFAULT_INDEX = -1;
        private const string CONTROL_NAME_NAME = "CN_NAME";
        private const string CONTROL_NAME_PATH = "CN_PATH";

        private string[] m_LibraryCreationNames;
        private readonly AssetManagementConfigVO m_ConfigVO;

        private LibraryCreationPathVO m_NewLibraryCreationPathVO;
        private LibraryCreationPathVO m_EditLibraryCreationPathVO;
        private readonly LibraryCreationPathVO m_StoredLibraryCreationPathVO;

        private bool m_ShouldRemove;
        private int m_ShouldRemoveIndex;

        public override string TabName
        {
            get => TAB_NAME;
        }

        public ConfigurationPanel()
        {
            m_StoredLibraryCreationPathVO = new LibraryCreationPathVO();

            m_ConfigVO = AssetManagementController.Instance.AssetManagementConfigVO;
            UpdateLibraryCreationNames(m_ConfigVO.LibraryCreationPaths);
        }

        public override void Draw()
        {
            Header("Configure Asset Management System");

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            DrawLibraryCreationSection();

            EditorGUILayout.EndVertical();

            base.Draw();
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

            EditorGUI.BeginChangeCheck();
            m_ConfigVO.DefaultLibraryCreationPathIndex = EditorGUILayout.Popup(m_ConfigVO.DefaultLibraryCreationPathIndex,
                                                                               m_LibraryCreationNames,
                                                                               GUILayout.Width(200));
            if (EditorGUI.EndChangeCheck())
            {
                AssetManagementController.Instance.SaveConfigVO();
            }

            LibraryCreationPathVO defaultCreationPathVO = m_ConfigVO.LibraryCreationPaths[m_ConfigVO.DefaultLibraryCreationPathIndex];

            EditorGUILayout.LabelField($"Path: Assets/{defaultCreationPathVO.Path.AssetsRelativePath}");

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();
        }

        private void DrawLibraryCreationPaths()
        {
            EditorGUILayout.LabelField("Library Creation Paths:");

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            for (int i = 0; i < m_ConfigVO.LibraryCreationPaths.Count; ++i)
            {
                DrawLibraryCreationPathVO(m_ConfigVO.LibraryCreationPaths[i], i);
            }

            if (m_ShouldRemove)
            {
                m_ConfigVO.LibraryCreationPaths.RemoveAt(m_ShouldRemoveIndex);
                m_ShouldRemove = false;
                AssetManagementController.Instance.SaveConfigVO();
            }

            DrawLibraryCreationPathVO(m_NewLibraryCreationPathVO, DEFAULT_INDEX);

            EditorGUILayout.Separator();

            if (SmallButton("New"))
            {
                Cancel();

                m_NewLibraryCreationPathVO = new LibraryCreationPathVO
                {
                    IsBeingEdited = true,
                };

                m_NewLibraryCreationPathVO.CopyInto(m_StoredLibraryCreationPathVO);

                FocusControl($"{CONTROL_NAME_NAME}{DEFAULT_INDEX}", true);
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();
        }

        private void DrawLibraryCreationPathVO(LibraryCreationPathVO pathVO, int index)
        {
            if (pathVO == null)
            {
                return;
            }

            EditorGUILayout.BeginHorizontal();
            if (pathVO.IsBeingEdited)
            {
                DrawLibraryCreationPathVOInEditMode(pathVO, index);
            }
            else
            {
                DrawLibraryCreationPathVOInViewMode(pathVO, index);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawLibraryCreationPathVOInEditMode(LibraryCreationPathVO pathVO, int index)
        {
            bool shouldValidate = false;
            bool shouldCancel = false;

            TightLabel("Name:", EditorStyles.boldLabel);
            pathVO.Name = KeyboardTextField($"{CONTROL_NAME_NAME}{index}",
                                            pathVO.Name,
                                            ref shouldValidate,
                                            ref shouldCancel,
                                            GUILayout.ExpandWidth(true));
            FieldSpacer();


            TightLabel("Path:", EditorStyles.boldLabel);
            TightLabel("Assets/", AnvilIMGUIConstants.STYLE_LABEL_RIGHT_JUSTIFIED);
            pathVO.Path.AssetsRelativePath = KeyboardTextField($"{CONTROL_NAME_PATH}{index}",
                                                          pathVO.Path.AssetsRelativePath,
                                                          ref shouldValidate,
                                                          ref shouldCancel,
                                                          GUILayout.ExpandWidth(true));
            FieldSpacer();

            if (index >= 0)
            {
                if (SmallButton("Remove"))
                {
                    m_ShouldRemove = true;
                    m_ShouldRemoveIndex = index;
                }
            }
            else
            {
                SmallButtonSpacer();
            }

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
                Cancel();
            }
            else if (shouldValidate)
            {
                Validate();
            }
        }

        private void DrawLibraryCreationPathVOInViewMode(LibraryCreationPathVO pathVO, int index)
        {
            TightLabel("Name:", EditorStyles.boldLabel);
            EditorGUILayout.LabelField(pathVO.Name, GUILayout.ExpandWidth(true));
            FieldSpacer();

            TightLabel("Path:", EditorStyles.boldLabel);
            TightLabel($"Assets{Path.DirectorySeparatorChar}", AnvilIMGUIConstants.STYLE_LABEL_RIGHT_JUSTIFIED);
            EditorGUILayout.LabelField(pathVO.Path.AssetsRelativePath, GUILayout.ExpandWidth(true));
            FieldSpacer();

            SmallButtonSpacer();
            SmallButtonSpacer();

            if (SmallButton("Edit"))
            {
                //Cancel if anything else was being edited
                Cancel();

                m_EditLibraryCreationPathVO = pathVO;
                m_EditLibraryCreationPathVO.IsBeingEdited = true;

                //Store old values in case we cancel
                m_EditLibraryCreationPathVO.CopyInto(m_StoredLibraryCreationPathVO);

                FocusControl($"{CONTROL_NAME_NAME}{index}", true);
            }
        }

        private void UpdateLibraryCreationNames(IEnumerable<LibraryCreationPathVO> libraryCreationPathVOs)
        {
            m_LibraryCreationNames = libraryCreationPathVOs.Select(o => o.Name)
                                                           .ToArray();
        }

        private void Cancel()
        {
            m_NewLibraryCreationPathVO = null;

            if (m_EditLibraryCreationPathVO == null)
            {
                return;
            }

            m_EditLibraryCreationPathVO.IsBeingEdited = false;
            //Restore old values
            m_StoredLibraryCreationPathVO.CopyInto(m_EditLibraryCreationPathVO);

            m_EditLibraryCreationPathVO = null;
        }

        private void Validate()
        {
            if (m_NewLibraryCreationPathVO != null)
            {
                //TODO: Actually validate
                m_ConfigVO.LibraryCreationPaths.Add(m_NewLibraryCreationPathVO);
                m_NewLibraryCreationPathVO.IsBeingEdited = false;
                m_NewLibraryCreationPathVO = null;
            }
            else if (m_EditLibraryCreationPathVO != null)
            {
                //TODO: Actually validate
                m_EditLibraryCreationPathVO.IsBeingEdited = false;
                m_EditLibraryCreationPathVO = null;
            }

            UpdateLibraryCreationNames(m_ConfigVO.LibraryCreationPaths);
            AssetManagementController.Instance.SaveConfigVO();
        }

    }
}


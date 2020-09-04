﻿using System.Collections.Generic;
using System.Linq;
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

            EditorGUILayout.LabelField($"Path: Assets/{defaultCreationPathVO.AssetsRelativePath}");

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

                bool shouldValidate = false;
                bool shouldCancel = false;

                pathVO.Name = KeyboardTextField("NameField",
                                                "Name:",
                                                pathVO.Name,
                                                ref shouldValidate,
                                                ref shouldCancel,
                                                GUILayout.ExpandWidth(true));
                //TODO: Move out
                GUIStyle style = new GUIStyle(EditorStyles.label);
                style.alignment = TextAnchor.MiddleRight;
                //END TODO

                EditorGUILayout.LabelField("Path:", GUILayout.ExpandWidth(false), GUILayout.MinWidth(1));
                EditorGUILayout.LabelField("Assets/", style, GUILayout.ExpandWidth(false), GUILayout.MinWidth(1));

                pathVO.AssetsRelativePath = KeyboardTextField("PathField",
                                                string.Empty,
                                                pathVO.AssetsRelativePath,
                                                ref shouldValidate,
                                                ref shouldCancel,
                                                GUILayout.ExpandWidth(true));

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
            }
            else
            {
                EditorGUILayout.LabelField($"Name: {pathVO.Name}");
                EditorGUILayout.LabelField($"Path: Assets/{pathVO.AssetsRelativePath}");
                if (SmallButton("Edit"))
                {
                    pathVO.IsBeingEdited = true;
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void UpdateLibraryCreationPaths(IEnumerable<LibraryCreationPathVO> libraryCreationPathVOs)
        {
            m_LibraryCreationNames = libraryCreationPathVOs.Select(o => o.Name)
                                                           .ToArray();
        }
    }
}


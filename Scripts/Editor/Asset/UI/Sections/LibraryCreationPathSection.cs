using System.Collections.Generic;
using System.IO;
using System.Linq;
using Anvil.UnityEditor.IMGUI;
using UnityEditor;
using UnityEngine;

namespace Anvil.UnityEditor.Asset
{
    public class LibraryCreationPathSection : AbstractAnvilIMGUISection<EditorLibraryCreationPathVO>, IAnvilIMGUISection
    {
        private const int DEFAULT_INDEX = -1;
        private const string CONTROL_LIBRARY_CREATION_NAME = "CN_LIBRARY_CREATION_NAME";
        private const string CONTROL_LIBRARY_CREATION_PATH = "CN_LIBRARY_CREATION_PATH";

        private string[] m_LibraryCreationNames;
        private readonly AMConfigVO m_ConfigVO;

        public LibraryCreationPathSection()
        : base ("Library Creation Paths:",
                DEFAULT_INDEX,
                CONTROL_LIBRARY_CREATION_NAME,
                AMController.Instance.AMConfigVO.LibraryCreationPaths)
        {
            m_ConfigVO = AMController.Instance.AMConfigVO;
            UpdateLibraryCreationNames(m_ConfigVO.LibraryCreationPaths);
        }

        private void UpdateLibraryCreationNames(IEnumerable<EditorLibraryCreationPathVO> libraryCreationPathVOs)
        {
            m_LibraryCreationNames = libraryCreationPathVOs.Select(o => o.Name)
                                                           .ToArray();
        }

        protected override void PreDraw()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            AnvilIMGUI.Header("Library Creation");

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            DrawDefaultLibraryCreationPath();
        }

        protected override void PostDraw()
        {
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();
        }

        private void DrawDefaultLibraryCreationPath()
        {
            EditorGUILayout.LabelField("Default Library Creation Path:");

            if (m_ConfigVO.LibraryCreationPaths.Count == 0)
            {
                EditorGUILayout.HelpBox("No Library Creation Paths, please create some.", MessageType.Warning);
                return;
            }

            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();
            m_ConfigVO.DefaultLibraryCreationPathIndex = EditorGUILayout.Popup(m_ConfigVO.DefaultLibraryCreationPathIndex,
                                                                               m_LibraryCreationNames,
                                                                               GUILayout.Width(200));
            if (EditorGUI.EndChangeCheck())
            {
                AMController.Instance.SaveConfigVO();
            }

            EditorLibraryCreationPathVO defaultCreationPathVO = m_ConfigVO.LibraryCreationPaths[m_ConfigVO.DefaultLibraryCreationPathIndex];

            EditorGUILayout.LabelField($"Path: Assets/{defaultCreationPathVO.Path.AssetsRelativePath}");

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();
        }

        protected override void DrawVOInEditMode(EditorLibraryCreationPathVO vo, int index, out bool shouldCancel, out bool shouldValidate)
        {
            shouldValidate = false;
            shouldCancel = false;

            AnvilIMGUI.TightLabel("Name:", EditorStyles.boldLabel);
            vo.Name = AnvilIMGUI.KeyboardTextField($"{CONTROL_LIBRARY_CREATION_NAME}{index}",
                                            vo.Name,
                                            ref shouldValidate,
                                            ref shouldCancel,
                                            GUILayout.ExpandWidth(true));
            AnvilIMGUI.FieldSpacer();


            AnvilIMGUI.TightLabel("Path:", EditorStyles.boldLabel);
            AnvilIMGUI.TightLabel("Assets/", AnvilIMGUIConstants.STYLE_LABEL_RIGHT_JUSTIFIED);
            vo.Path.AssetsRelativePath = AnvilIMGUI.KeyboardTextField($"{CONTROL_LIBRARY_CREATION_PATH}{index}",
                                                                         vo.Path.AssetsRelativePath,
                                                                         ref shouldValidate,
                                                                         ref shouldCancel,
                                                                         GUILayout.ExpandWidth(true));
            AnvilIMGUI.FieldSpacer();

            if (index >= 0)
            {
                if (AnvilIMGUI.SmallButton("Remove"))
                {
                    Remove(index);
                }
            }
            else
            {
                AnvilIMGUI.SmallButtonSpacer();
            }

            if (AnvilIMGUI.SmallButton("Cancel"))
            {
                shouldCancel = true;
            }

            if (AnvilIMGUI.SmallButton("Done"))
            {
                shouldValidate = true;
            }
        }

        protected override void DrawVOInViewMode(EditorLibraryCreationPathVO vo, int index)
        {
            AnvilIMGUI.TightLabel("Name:", EditorStyles.boldLabel);
            EditorGUILayout.LabelField(vo.Name, GUILayout.ExpandWidth(true));
            AnvilIMGUI.FieldSpacer();

            AnvilIMGUI.TightLabel("Path:", EditorStyles.boldLabel);
            AnvilIMGUI.TightLabel($"Assets{Path.DirectorySeparatorChar}", AnvilIMGUIConstants.STYLE_LABEL_RIGHT_JUSTIFIED);
            EditorGUILayout.LabelField(vo.Path.AssetsRelativePath, GUILayout.ExpandWidth(true));
            AnvilIMGUI.FieldSpacer();

            AnvilIMGUI.SmallButtonSpacer();
            AnvilIMGUI.SmallButtonSpacer();
        }

        protected override bool ValidateVO(EditorLibraryCreationPathVO vo)
        {
            //TODO: Actually validate
            UpdateLibraryCreationNames(m_ConfigVO.LibraryCreationPaths);
            return true;
        }

        protected override void HandleOnVOCreateStart(EditorLibraryCreationPathVO vo)
        {

        }

        protected override void HandleOnVOCreateCancel(EditorLibraryCreationPathVO vo)
        {

        }

        protected override void HandleOnVOCreateComplete(EditorLibraryCreationPathVO vo)
        {
            AMController.Instance.SaveConfigVO();
        }

        protected override void HandleOnVOEditStart(EditorLibraryCreationPathVO vo)
        {

        }

        protected override void HandleOnVOEditCancel(EditorLibraryCreationPathVO vo)
        {

        }

        protected override void HandleOnVOEditComplete(EditorLibraryCreationPathVO vo)
        {
            AMController.Instance.SaveConfigVO();
        }

        protected override void HandleOnVORemoved(EditorLibraryCreationPathVO vo)
        {
            AMController.Instance.SaveConfigVO();
        }
    }
}

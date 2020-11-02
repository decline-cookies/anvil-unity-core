using System.Collections.Generic;
using System.IO;
using System.Linq;
using Anvil.UnityEditor.IMGUI;
using UnityEditor;
using UnityEngine;

namespace Anvil.UnityEditor.Asset
{
    public class ConfigurationPanel : AbstractAMEditorPanel
    {
        //TODO: Abstract a bunch of the common functionality

        private const string TAB_NAME = "Configuration";

        private const string CONTROL_VARIANT_PRESET_NAME = "CN_VARIANT_PRESET_NAME";

        private string[] m_VariantPresetNames;
        private readonly AMConfigVO m_ConfigVO;

        private EditorVariantPresetVO m_NewVariantPresetVO;
        private EditorVariantPresetVO m_EditVariantPresetVO;
        private readonly EditorVariantPresetVO m_StoredVariantPresetVO;

        private readonly LibraryCreationPathSection m_LibraryCreationPathSection;

        private bool m_ShouldRemoveVariantPreset;
        private int m_ShouldRemoveVariantPresetIndex;

        public override string TabName
        {
            get => TAB_NAME;
        }

        public ConfigurationPanel()
        {
            m_ConfigVO = AMController.Instance.AMConfigVO;

            m_LibraryCreationPathSection = CreateSection<LibraryCreationPathSection>();
            m_StoredVariantPresetVO = new EditorVariantPresetVO();
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

            AnvilIMGUI.FieldSpacer();

            DrawDefaultVariantPreset();
            DrawVariantPresets();

            EditorGUILayout.EndVertical();
        }

        private void DrawDefaultVariantPreset()
        {
            EditorGUILayout.LabelField("Default Variant Preset:");

            if (m_ConfigVO.VariantPresets.Count == 0)
            {
                EditorGUILayout.HelpBox("No Variant Presets, please create some.", MessageType.Warning);
                return;
            }

            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();
            m_ConfigVO.DefaultVariantPresetIndex = EditorGUILayout.Popup(m_ConfigVO.DefaultVariantPresetIndex,
                                                                         m_VariantPresetNames,
                                                                         GUILayout.Width(200));
            if (EditorGUI.EndChangeCheck())
            {
                AMController.Instance.SaveConfigVO();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();
        }

        private void DrawVariantPresets()
        {
            // EditorGUILayout.LabelField("Variant Presets:");
            //
            // EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            //
            // for (int i = 0; i < m_ConfigVO.VariantPresets.Count; ++i)
            // {
            //     DrawVariantPreset(m_ConfigVO.VariantPresets[i], i);
            // }
            //
            // if (m_ShouldRemoveVariantPreset)
            // {
            //     m_ConfigVO.VariantPresets.RemoveAt(m_ShouldRemoveVariantPresetIndex);
            //     m_ShouldRemoveLibraryCreation = false;
            //     AMController.Instance.SaveConfigVO();
            // }
            //
            // DrawVariantPreset(m_NewVariantPresetVO, DEFAULT_INDEX);
            //
            // EditorGUILayout.Separator();
            //
            // if (SmallButton("New"))
            // {
            //     Cancel();
            //
            //     m_NewVariantPresetVO = new EditorVariantPresetVO
            //     {
            //         IsBeingEdited = true,
            //     };
            //
            //     m_NewVariantPresetVO.CopyInto(m_StoredVariantPresetVO);
            //
            //     FocusControl($"{CONTROL_VARIANT_PRESET_NAME}{DEFAULT_INDEX}", true);
            // }
            //
            // EditorGUILayout.EndVertical();
        }

        private void DrawVariantPreset(EditorVariantPresetVO presetVO, int index)
        {
            if (presetVO == null)
            {
                return;
            }

            EditorGUILayout.BeginHorizontal();
            if (presetVO.IsBeingEdited)
            {
                DrawVariantPresetVOInEditMode(presetVO, index);
            }
            else
            {
                DrawVariantPresetVOInViewMode(presetVO, index);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawVariantPresetVOInEditMode(EditorVariantPresetVO presetVO, int index)
        {

        }

        private void DrawVariantPresetVOInViewMode(EditorVariantPresetVO presetVO, int index)
        {

        }



    }
}


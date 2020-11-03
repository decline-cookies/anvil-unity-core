using System.Collections.Generic;
using System.Linq;
using Anvil.UnityEditor.IMGUI;
using UnityEditor;
using UnityEngine;

namespace Anvil.UnityEditor.Asset
{
    public class VariantPresetSection : AbstractAnvilIMGUISection<EditorVariantPresetVO>, IAnvilIMGUISection
    {
        private const int DEFAULT_INDEX = -1;
        private const string CONTROL_VARIANT_PRESET_NAME = "CN_VARIANT_PRESET_NAME";

        private string[] m_VariantPresetNames;
        private readonly AMConfigVO m_ConfigVO;

        public VariantPresetSection()
        : base ("Variant Presets:",
                DEFAULT_INDEX,
                CONTROL_VARIANT_PRESET_NAME,
                AMController.Instance.AMConfigVO.VariantPresets)
        {
            m_ConfigVO = AMController.Instance.AMConfigVO;
            UpdateVariantPresets(m_ConfigVO.VariantPresets);
        }

        private void UpdateVariantPresets(IEnumerable<EditorVariantPresetVO> variantPresetVOs)
        {
            m_VariantPresetNames = variantPresetVOs.Select(o => o.Name)
                                                   .ToArray();
        }

        protected override void PreDraw()
        {
            DrawDefaultVariantPreset();
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

        protected override void DrawVOInEditMode(EditorVariantPresetVO vo, int index, out bool shouldCancel, out bool shouldValidate)
        {
            shouldCancel = false;
            shouldValidate = false;

            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            AnvilIMGUI.TightLabel("Name:", EditorStyles.boldLabel);
            vo.Name = AnvilIMGUI.KeyboardTextField($"{CONTROL_VARIANT_PRESET_NAME}{index}",
                                                   vo.Name,
                                                   ref shouldValidate,
                                                   ref shouldCancel,
                                                   GUILayout.Width(200.0f));

            GUILayout.FlexibleSpace();

            if (index >= 0)
            {
                if (AnvilIMGUI.SmallButton("Remove"))
                {
                    if (EditorUtility.DisplayDialog("Remove Preset",
                                                    "Are you sure you want to remove this preset? This cannot be undone.",
                                                    "Remove",
                                                    "Cancel"))
                    {
                        Remove(index);
                    }
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

            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Source:", EditorStyles.boldLabel);

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Published:", EditorStyles.boldLabel);
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        protected override void DrawVOInViewMode(EditorVariantPresetVO vo, int index)
        {
            //TODO: Actually Draw
        }

        protected override void Validate()
        {
            UpdateVariantPresets(m_ConfigVO.VariantPresets);
            AMController.Instance.SaveConfigVO();
        }

        protected override void HandleOnRemoveComplete()
        {
            AMController.Instance.SaveConfigVO();
        }
    }
}

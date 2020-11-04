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
        private readonly Dictionary<EditorVariantPresetVO, List<VariantPresetSourceSection>> m_VariantPresetSourceSections;
        private VariantPresetSourceSection m_NewPresetSourceSection;

        public VariantPresetSection()
        : base ("Variant Presets:",
                DEFAULT_INDEX,
                CONTROL_VARIANT_PRESET_NAME,
                AMController.Instance.AMConfigVO.VariantPresets)
        {
            m_ConfigVO = AMController.Instance.AMConfigVO;
            m_VariantPresetSourceSections = new Dictionary<EditorVariantPresetVO, List<VariantPresetSourceSection>>();

            UpdateVariantPresets(m_ConfigVO.VariantPresets);
        }

        private void UpdateVariantPresets(IEnumerable<EditorVariantPresetVO> variantPresetVOs)
        {
            m_VariantPresetNames = variantPresetVOs.Select(o => o.Name)
                                                   .ToArray();
        }

        protected override void PreDraw()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            AnvilIMGUI.Header("Variant Presets");

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            DrawDefaultVariantPreset();
        }

        protected override void PostDraw()
        {
            EditorGUILayout.EndVertical();
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

            List<VariantPresetSourceSection> sourceSections = GetOrCreateSourceSectionsForVO(vo);

            foreach (VariantPresetSourceSection sourceSection in sourceSections)
            {
                sourceSection.Draw();
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        protected override void DrawVOInViewMode(EditorVariantPresetVO vo, int index)
        {
            //TODO: Actually Draw
        }

        protected override bool ValidateVO(EditorVariantPresetVO vo)
        {
            //TODO: Actually validate
            UpdateVariantPresets(m_ConfigVO.VariantPresets);
            return true;
        }

        protected override void HandleOnVOCreateStart(EditorVariantPresetVO vo)
        {
            List<VariantPresetSourceSection> sourceSections = GetOrCreateSourceSectionsForVO(vo);

            m_NewPresetSourceSection = new VariantPresetSourceSection(vo.SourceVariants);
            sourceSections.Add(m_NewPresetSourceSection);
        }

        protected override void HandleOnVOCreateCancel(EditorVariantPresetVO vo)
        {
            List<VariantPresetSourceSection> sourceSections = GetOrCreateSourceSectionsForVO(vo);
            sourceSections.Remove(m_NewPresetSourceSection);
            m_NewPresetSourceSection = null;
        }

        protected override void HandleOnVOCreateComplete(EditorVariantPresetVO vo)
        {
            m_NewPresetSourceSection = null;
            AMController.Instance.SaveConfigVO();
        }

        protected override void HandleOnVOEditStart(EditorVariantPresetVO vo)
        {

        }

        protected override void HandleOnVOEditCancel(EditorVariantPresetVO vo)
        {

        }

        protected override void HandleOnVOEditComplete(EditorVariantPresetVO vo)
        {
            AMController.Instance.SaveConfigVO();
        }

        protected override void HandleOnVORemoved(EditorVariantPresetVO vo)
        {
            m_VariantPresetSourceSections.Remove(vo);
            AMController.Instance.SaveConfigVO();
        }

        private List<VariantPresetSourceSection> GetOrCreateSourceSectionsForVO(EditorVariantPresetVO vo)
        {
            if (!m_VariantPresetSourceSections.TryGetValue(vo, out List<VariantPresetSourceSection> sourceSections))
            {
                sourceSections = new List<VariantPresetSourceSection>();
                m_VariantPresetSourceSections.Add(vo, sourceSections);
            }

            return sourceSections;
        }
    }
}

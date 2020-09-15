using Anvil.UnityEditor.IMGUI;
using UnityEditor;
using UnityEngine;

namespace Anvil.UnityEditor.Asset
{
    public abstract class AbstractAssetManagementEditorPanel
    {
        public abstract string TabName { get; }

        public virtual void Draw()
        {

        }

        protected void Header(string header)
        {
            EditorGUILayout.LabelField(header, EditorStyles.boldLabel);
            EditorGUILayout.Separator();
        }

        protected bool SmallButton(string label)
        {
            return GUILayout.Button(label, EditorStyles.miniButton, GUILayout.Width(60));
        }

        protected void FieldSpacer()
        {
            EditorGUILayout.Space(10, false);
        }

        protected void SmallButtonSpacer()
        {
            bool isEnabled = GUI.enabled;
            Color color = GUI.color;

            GUI.enabled = false;
            GUI.color = AnvilIMGUIConstants.TRANSPARENT;

            GUILayout.Button(string.Empty, EditorStyles.miniButton, GUILayout.Width(60));

            GUI.color = color;
            GUI.enabled = isEnabled;
        }

        protected void TightLabel(string label, GUIStyle style)
        {
            GUIContent content = new GUIContent(label);
            Vector2 size = EditorStyles.label.CalcSize(content);
            EditorGUILayout.LabelField(content, style, GUILayout.Width(size.x), GUILayout.ExpandWidth(false));
        }

        protected string KeyboardTextField(string fieldName,
                                           string text,
                                           ref bool shouldValidate,
                                           ref bool shouldCancel,
                                           ref bool shouldTab,
                                           params GUILayoutOption[] options)
        {
            // GUI.SetNextControlName(fieldName);
            string outputText = EditorGUILayout.TextField(text, options);

            // Event evt = Event.current;
            // if (evt.isKey && GUI.GetNameOfFocusedControl() == fieldName)
            // {
            //     switch (evt.keyCode)
            //     {
            //         case KeyCode.Return:
            //         case KeyCode.KeypadEnter:
            //             shouldValidate = true;
            //             break;
            //         case KeyCode.Escape:
            //             shouldCancel = true;
            //             break;
            //         case KeyCode.Tab:
            //             shouldTab = true;
            //             break;
            //     }
            //
            //     Event.current.Use();
            // }

            return outputText;
        }
    }
}


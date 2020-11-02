using UnityEditor;
using UnityEngine;

namespace Anvil.UnityEditor.IMGUI
{
    public static class AnvilIMGUI
    {
        public static void Header(string header)
        {
            EditorGUILayout.LabelField(header, EditorStyles.boldLabel);
            EditorGUILayout.Separator();
        }

        public static bool SmallButton(string label)
        {
            return GUILayout.Button(label, EditorStyles.miniButton, GUILayout.Width(60));
        }

        public static void FieldSpacer()
        {
            EditorGUILayout.Space(10, false);
        }

        public static void SmallButtonSpacer()
        {
            bool isEnabled = GUI.enabled;
            Color color = GUI.color;

            GUI.enabled = false;
            GUI.color = AnvilIMGUIConstants.TRANSPARENT;

            GUILayout.Button(string.Empty, EditorStyles.miniButton, GUILayout.Width(60));

            GUI.color = color;
            GUI.enabled = isEnabled;
        }

        public static void TightLabel(string label, GUIStyle style)
        {
            GUIContent content = new GUIContent(label);
            Vector2 size = EditorStyles.label.CalcSize(content);
            EditorGUILayout.LabelField(content, style, GUILayout.Width(size.x), GUILayout.ExpandWidth(false));
        }

        public static string KeyboardTextField(string controlName,
                                           string text,
                                           ref bool shouldValidate,
                                           ref bool shouldCancel,
                                           params GUILayoutOption[] options)
        {
            GUI.SetNextControlName(controlName);
            string outputText = EditorGUILayout.TextField(text, options);

            Event evt = Event.current;
            if (evt.isKey && GUI.GetNameOfFocusedControl() == controlName)
            {
                switch (evt.keyCode)
                {
                    case KeyCode.Return:
                    case KeyCode.KeypadEnter:
                        shouldValidate = true;
                        Event.current.Use();
                        break;
                    case KeyCode.Escape:
                        shouldCancel = true;
                        Event.current.Use();
                        break;
                }
            }

            return outputText;
        }
    }
}


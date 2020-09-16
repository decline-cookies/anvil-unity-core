using Anvil.UnityEditor.IMGUI;
using UnityEditor;
using UnityEngine;

namespace Anvil.UnityEditor.Asset
{
    public abstract class AbstractAMEditorPanel
    {
        public abstract string TabName { get; }

        private bool m_ShouldFocus;
        private bool m_IsShouldFocusText;
        private string m_ShouldFocusName;

        public virtual void Enable()
        {

        }

        public virtual void Disable()
        {

        }

        public virtual void Draw()
        {
            FocusControlIfNeeded();
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

        protected string KeyboardTextField(string controlName,
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

        protected void FocusControl(string controlName, bool isText)
        {
            m_ShouldFocus = true;
            m_ShouldFocusName = controlName;
            m_IsShouldFocusText = isText;
        }

        private void FocusControlIfNeeded()
        {
            if (!m_ShouldFocus || Event.current.type != EventType.Repaint)
            {
                return;
            }

            if (m_IsShouldFocusText)
            {
                EditorGUI.FocusTextInControl(m_ShouldFocusName);
            }
            else
            {
                GUI.FocusControl(m_ShouldFocusName);
            }

            m_ShouldFocus = false;
        }
    }
}


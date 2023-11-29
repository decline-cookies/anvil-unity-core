using UnityEditor;
using UnityEngine;

namespace Anvil.Unity.Editor.UI
{
    /// <summary>
    /// Helper class to show an Input dialog in the editor.
    /// </summary>
    public class EditorInputDialog : EditorWindow
    {
        private const string CONTROL_NAME_INPUT_TEXT = "EditorInputDialog_INPUT_TEXT";

        /// <summary>
        /// Shows the Input Dialog
        /// </summary>
        /// <param name="title">The title of the Dialog window</param>
        /// <param name="description">The description text to communicate what the user should do</param>
        /// <param name="inputTextLabel">A label to describe the input text</param>
        /// <param name="okBtnText">What the "OK" button should read</param>
        /// <param name="cancelBtnText">What the "Cancel" button should read</param>
        /// <param name="defaultInputText">The default text in the input box if needed</param>
        /// <returns>
        /// The <see cref="EditorInputDialogResult"/>
        /// </returns>
        public static EditorInputDialogResult Show(
            string title,
            string description,
            string inputTextLabel,
            string okBtnText,
            string cancelBtnText,
            string defaultInputText = null)
        {
            Vector2 maxPos = GUIUtility.GUIToScreenPoint(new Vector2(Screen.width, Screen.height));

            EditorInputDialog window = CreateInstance<EditorInputDialog>();
            window.Init(
                maxPos,
                title,
                description,
                inputTextLabel,
                okBtnText,
                cancelBtnText,
                defaultInputText);
            window.ShowModal();

            return window.Result;
        }

        private string m_Description;
        private string m_InputTextLabel;
        private string m_OKBtnText;
        private string m_CancelBtnText;
        private Vector2 m_MaxScreenPosition;

        private bool m_HasInitializedPosition;
        private string m_InputText;

        public EditorInputDialogResult Result { get; private set; }

        private void Init(
            Vector2 maxPos,
            string titleText,
            string description,
            string inputTextLabel,
            string okBtnText,
            string cancelBtnText,
            string defaultInputText = null)
        {
            m_MaxScreenPosition = maxPos;
            titleContent = new GUIContent(titleText);
            m_Description = description;
            m_InputTextLabel = inputTextLabel;
            m_OKBtnText = okBtnText;
            m_CancelBtnText = cancelBtnText;
            m_InputText = defaultInputText ?? string.Empty;
        }

        private void OnGUI()
        {
            Event currentEvent = Event.current;
            CheckKeyboard(currentEvent);

            Rect rect = EditorGUILayout.BeginVertical();

            EditorGUILayout.Space(12);
            EditorGUILayout.LabelField(m_Description);

            EditorGUILayout.Space(8);
            GUI.SetNextControlName(CONTROL_NAME_INPUT_TEXT);
            m_InputText = EditorGUILayout.TextField(m_InputTextLabel, m_InputText);
            GUI.FocusControl(CONTROL_NAME_INPUT_TEXT);
            EditorGUILayout.Space(12);

            Rect controlRect = EditorGUILayout.GetControlRect();
            controlRect.width *= 0.5f;
            if (GUI.Button(controlRect, m_OKBtnText))
            {
                Complete(EditorInputDialogResult.ResultAction.Ok);
            }

            controlRect.x += controlRect.width;
            if (GUI.Button(controlRect, m_CancelBtnText))
            {
                Complete(EditorInputDialogResult.ResultAction.Cancel);
            }

            EditorGUILayout.Space(8);
            EditorGUILayout.EndVertical();

            if (rect.width != 0 && minSize != rect.size)
            {
                minSize = maxSize = rect.size;
            }

            InitializePosition(currentEvent);
        }

        private void CheckKeyboard(Event currentEvent)
        {
            if (currentEvent.type != EventType.KeyDown)
            {
                return;
            }


            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (currentEvent.keyCode)
            {
                case KeyCode.Escape:
                    currentEvent.Use();
                    Complete(EditorInputDialogResult.ResultAction.Cancel);
                    break;

                case KeyCode.Return:
                case KeyCode.KeypadEnter:
                    currentEvent.Use();
                    Complete(EditorInputDialogResult.ResultAction.Ok);
                    break;
            }
        }

        private void InitializePosition(Event currentEvent)
        {
            if (m_HasInitializedPosition || currentEvent.type != EventType.Layout)
            {
                return;
            }
            m_HasInitializedPosition = true;

            Vector2 mousePos = GUIUtility.GUIToScreenPoint(currentEvent.mousePosition);
            mousePos.x += 32;
            if (mousePos.x + position.width > m_MaxScreenPosition.x)
            {
                mousePos.x -= position.width + 64;
            }
            if (mousePos.y + position.height > m_MaxScreenPosition.y)
            {
                mousePos.y = m_MaxScreenPosition.y - position.height;
            }

            position = new Rect(mousePos.x, mousePos.y, position.width, position.height);

            Focus();
        }

        private void Complete(EditorInputDialogResult.ResultAction resultAction)
        {
            Result = new EditorInputDialogResult(resultAction, m_InputText);
            Close();
        }
    }
}
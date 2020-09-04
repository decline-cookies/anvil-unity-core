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

        protected void TightLabel(string label)
        {
            GUIContent content = new GUIContent(label);
            Vector2 size = EditorStyles.label.CalcSize(content);
            EditorGUILayout.LabelField(content, GUILayout.Width(size.x), GUILayout.ExpandWidth(false));
        }
    }
}


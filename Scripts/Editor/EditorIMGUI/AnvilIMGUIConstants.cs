using System.IO;
using UnityEditor;
using UnityEngine;

namespace Anvil.UnityEditor.IMGUI
{
    public static class AnvilIMGUIConstants
    {
        public static readonly Color TRANSPARENT = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        public static readonly GUIStyle STYLE_LABEL_RIGHT_JUSTIFIED = new GUIStyle(EditorStyles.label)
        {
            alignment = TextAnchor.MiddleRight
        };
    }
}


using System;
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

        protected T CreateSection<T>()
            where T : IAnvilIMGUISection
        {
            IAnvilIMGUISection section = Activator.CreateInstance<T>();
            section.OnFocusControl += Section_OnFocusControl;

            return (T)section;
        }

        private void Section_OnFocusControl(string controlName, bool isText)
        {
            FocusControl(controlName, isText);
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


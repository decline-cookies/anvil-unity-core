using System;
using System.Collections.Generic;
using Anvil.UnityEditor.Data;
using UnityEditor;

namespace Anvil.UnityEditor.IMGUI
{
    public abstract class AbstractAnvilIMGUISection<T>
        where T : class, IEditorSectionVO<T>, new()
    {
        public event Action<string, bool> OnFocusControl;

        private bool m_ShouldRemove;
        private int m_ShouldRemoveIndex;

        private T m_NewVO;
        private T m_EditVO;
        private readonly T m_StoredVO;

        private readonly string m_Title;
        private readonly int m_DefaultIndex;
        private readonly string m_FocusControlName;
        private readonly List<T> m_ListVOs;

        protected AbstractAnvilIMGUISection(string title,
                                            int defaultIndex,
                                            string focusControlName,
                                            List<T> listVOs)
        {
            m_Title = title;
            m_DefaultIndex = defaultIndex;
            m_FocusControlName = focusControlName;
            m_ListVOs = listVOs;

            m_StoredVO = new T();
        }

        protected void Remove(int index)
        {
            m_ShouldRemove = true;
            m_ShouldRemoveIndex = index;
        }

        public void Draw()
        {
            PreDraw();

            EditorGUILayout.LabelField(m_Title);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            for (int i = 0; i < m_ListVOs.Count; ++i)
            {
                DrawVO(m_ListVOs[i], i);
            }

            if (m_ShouldRemove)
            {
                m_ListVOs.RemoveAt(m_ShouldRemoveIndex);
                m_ShouldRemove = false;
                HandleOnRemoveComplete();
            }

            DrawVO(m_NewVO, m_DefaultIndex);

            EditorGUILayout.Separator();

            if (AnvilIMGUI.SmallButton("New"))
            {
                Cancel();

                m_NewVO = new T
                {
                    IsBeingEdited = true
                };

                m_NewVO.CopyInto(m_StoredVO);

                OnFocusControl?.Invoke($"{m_FocusControlName}{m_DefaultIndex}", true);
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawVO(T vo, int index)
        {
            if (vo == null)
            {
                return;
            }

            EditorGUILayout.BeginHorizontal();
            if (vo.IsBeingEdited)
            {
                BaseDrawVOInEditMode(vo, index);
            }
            else
            {
                BaseDrawVOInViewMode(vo, index);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void BaseDrawVOInEditMode(T vo, int index)
        {
            DrawVOInEditMode(vo, index, out bool shouldCancel, out bool shouldValidate);

            if (shouldCancel)
            {
                Cancel();
            }
            else if (shouldValidate)
            {
                BaseValidate();
            }
        }

        private void BaseDrawVOInViewMode(T vo, int index)
        {
            DrawVOInViewMode(vo, index);

            if (AnvilIMGUI.SmallButton("Edit"))
            {
                //Cancel if anything else was being edited
                Cancel();

                m_EditVO = vo;
                m_EditVO.IsBeingEdited = true;

                //Store old values in case we cancel
                m_EditVO.CopyInto(m_StoredVO);

                OnFocusControl?.Invoke($"{m_FocusControlName}{index}", true);
            }
        }

        private void BaseValidate()
        {
            if (m_NewVO != null)
            {
                //TODO: Actually validate
                m_ListVOs.Add(m_NewVO);
                m_NewVO.IsBeingEdited = false;
                m_NewVO = null;
            }
            else if (m_EditVO != null)
            {
                //TODO: Actually validate
                m_EditVO.IsBeingEdited = false;
                m_EditVO = null;
            }

            Validate();
        }

        private void Cancel()
        {
            m_NewVO = null;

            if (m_EditVO == null)
            {
                return;
            }

            m_EditVO.IsBeingEdited = false;
            //Restore old values
            m_StoredVO.CopyInto(m_EditVO);

            m_EditVO = null;
        }

        protected abstract void PreDraw();
        protected abstract void Validate();
        protected abstract void HandleOnRemoveComplete();
        protected abstract void DrawVOInEditMode(T vo, int index, out bool shouldCancel, out bool shouldValidate);
        protected abstract void DrawVOInViewMode(T vo, int index);
    }
}


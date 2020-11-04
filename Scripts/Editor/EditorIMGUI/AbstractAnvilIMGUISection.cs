using System;
using System.Collections.Generic;
using Anvil.UnityEditor.Data;
using UnityEditor;
using UnityEngine;

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
                T removedVO = m_ListVOs[m_ShouldRemoveIndex];
                m_ListVOs.RemoveAt(m_ShouldRemoveIndex);
                m_ShouldRemove = false;
                HandleOnVORemoved(removedVO);
            }

            DrawVO(m_NewVO, m_DefaultIndex);

            EditorGUILayout.Separator();


            GUI.enabled = m_NewVO == null && m_EditVO == null;
            if (AnvilIMGUI.SmallButton("New"))
            {
                Cancel();

                m_NewVO = new T
                {
                    IsBeingEdited = true
                };

                m_NewVO.CopyInto(m_StoredVO);

                HandleOnVOCreateStart(m_NewVO);

                OnFocusControl?.Invoke($"{m_FocusControlName}{m_DefaultIndex}", true);
            }
            GUI.enabled = true;

            EditorGUILayout.EndVertical();

            PostDraw();
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
                Validate();
            }
        }

        private void BaseDrawVOInViewMode(T vo, int index)
        {
            DrawVOInViewMode(vo, index);

            GUI.enabled = m_NewVO == null && m_EditVO == null;
            if (AnvilIMGUI.SmallButton("Edit"))
            {
                //Cancel if anything else was being edited
                Cancel();

                m_EditVO = vo;
                m_EditVO.IsBeingEdited = true;

                //Store old values in case we cancel
                m_EditVO.CopyInto(m_StoredVO);

                HandleOnVOEditStart(m_EditVO);

                OnFocusControl?.Invoke($"{m_FocusControlName}{index}", true);
            }
            GUI.enabled = true;
        }

        private void Validate()
        {
            if (m_NewVO != null && ValidateVO(m_NewVO))
            {
                m_ListVOs.Add(m_NewVO);
                m_NewVO.IsBeingEdited = false;
                HandleOnVOCreateComplete(m_NewVO);
                m_NewVO = null;
            }
            else if (m_EditVO != null && ValidateVO(m_EditVO))
            {
                m_EditVO.IsBeingEdited = false;
                HandleOnVOEditComplete(m_EditVO);
                m_EditVO = null;
            }
        }

        private void Cancel()
        {
            if (m_NewVO != null)
            {
                HandleOnVOCreateCancel(m_NewVO);
                m_NewVO = null;
            }

            if (m_EditVO == null)
            {
                return;
            }

            m_EditVO.IsBeingEdited = false;
            //Restore old values
            m_StoredVO.CopyInto(m_EditVO);

            HandleOnVOEditCancel(m_EditVO);
            m_EditVO = null;
        }

        protected abstract void PreDraw();
        protected abstract void PostDraw();
        protected abstract void DrawVOInEditMode(T vo, int index, out bool shouldCancel, out bool shouldValidate);
        protected abstract void DrawVOInViewMode(T vo, int index);
        protected abstract bool ValidateVO(T vo);
        protected abstract void HandleOnVOCreateComplete(T vo);
        protected abstract void HandleOnVOCreateStart(T vo);
        protected abstract void HandleOnVOCreateCancel(T vo);
        protected abstract void HandleOnVOEditStart(T vo);
        protected abstract void HandleOnVOEditCancel(T vo);
        protected abstract void HandleOnVOEditComplete(T vo);
        protected abstract void HandleOnVORemoved(T vo);
    }
}


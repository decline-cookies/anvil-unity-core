using System;
using Anvil.CSharp.Content;
using Anvil.Unity.Asset;
using UnityEngine;
using UnityEngine.Assertions;

namespace Anvil.Unity.Content
{
    /// <summary>
    /// A more specific <see cref="AbstractContentController"/> specific to Unity where the corresponding Content is
    /// a <see cref="AbstractUnityContent"/>
    /// </summary>
    /// <typeparam name="TContent">A generic constrained to <see cref="AbstractUnityContent"/>. Allows the
    /// <see cref="AbstractContentController.Content"/> to be strongly typed to the actual Content class.</typeparam>
    public abstract class AbstractUnityContentController<TContent> : AbstractContentController<TContent>
        where TContent : AbstractUnityContent
    {
        //TODO: #142 TEMP - Remove once contentLoadingID is deprecated
        private LoadResourceCommand<GameObject> m_LoadResourceCommand;
        private string m_TempContentLoadingID;

        protected AbstractUnityContentController(string contentGroupID)
            :base(contentGroupID) { }

        [Obsolete]
        protected AbstractUnityContentController(string contentGroupID, string contentLoadingID)
            : base(contentGroupID)
        {
            m_TempContentLoadingID = contentLoadingID;
        }

        protected override void DisposeSelf()
        {
            m_LoadResourceCommand?.Dispose();
            m_LoadResourceCommand = null;

            base.DisposeSelf();
        }

        // TODO: #142 TEMP - Remove once contentLoadingID is deprecated
        protected override void Load()
        {
            // TODO: #142 TEMP - Remove once contentLoadingID is deprecated
            if (m_TempContentLoadingID != null)
            {
                Assert.IsTrue(m_LoadResourceCommand == null);
                m_LoadResourceCommand = new LoadResourceCommand<GameObject>(m_TempContentLoadingID);
                m_LoadResourceCommand.OnComplete += LoadResourceCommand_OnComplete;
                m_LoadResourceCommand.Execute();
            }
            else
            {
                Logger.Error("Load must be overridden with content loaded and then LoadComplete called when complete. In the future this will be an abstract method");
            }
        }

        // TODO: #142 TEMP - Remove once contentLoadingID is deprecated
        private void LoadResourceCommand_OnComplete(LoadResourceCommand<GameObject> sender)
        {
            GameObject instance = m_LoadResourceCommand.CreateInstance();
            m_LoadResourceCommand.Dispose();
            m_LoadResourceCommand = null;

            //TODO: Properly sanitize the name with a Regex via util method
            instance.name = instance.name.Replace("(Clone)", string.Empty);
            TContent content = instance.GetComponent<TContent>();

            LoadComplete(content);
        }
    }
}

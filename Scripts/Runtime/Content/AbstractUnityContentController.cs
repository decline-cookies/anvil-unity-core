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
        //TODO: Remove later on
        private LoadResourceCommand<GameObject> m_LoadResourceCommand;

        protected AbstractUnityContentController(string contentGroupID, string contentLoadingID)
            : base(contentGroupID, contentLoadingID) { }

        protected override void DisposeSelf()
        {
            m_LoadResourceCommand?.Dispose();
            m_LoadResourceCommand = null;

            base.DisposeSelf();
        }

        protected override void Load()
        {
            //TODO: Need to load all the required assets.
            //TODO: Need to load the actual prefab or scene.
            //TODO: Need to allow the user to insert their own logic in here.

            //For now we'll just assume it's a prefab and we're Resources.Loading it.
            //TODO: Support addressables

            Assert.IsTrue(m_LoadResourceCommand == null);

            m_LoadResourceCommand = new LoadResourceCommand<GameObject>(m_ContentLoadingID);
            m_LoadResourceCommand.OnComplete += LoadResourceCommand_OnComplete;
            m_LoadResourceCommand.Execute();
        }

        private void LoadResourceCommand_OnComplete(LoadResourceCommand<GameObject> sender)
        {
            GameObject instance = m_LoadResourceCommand.CreateInstance();
            //TODO: Properly sanitize the name with a Regex via util method
            instance.name = instance.name.Replace("(Clone)", string.Empty);
            Content = instance.GetComponent<TContent>();
            Content.Controller = this;

            m_LoadResourceCommand = null;

            LoadComplete();
        }
    }
}
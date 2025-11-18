using System;
using System.Collections.Generic;
using Anvil.CSharp.Content;
using Anvil.CSharp.Logging;
using Anvil.Unity.Asset;
using Anvil.Unity.Core;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

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

        private readonly List<Scene> m_LoadedContentScenes = new List<Scene>();


        protected AbstractUnityContentController(string contentGroupID)
            :base(contentGroupID) { }

        [Obsolete("Implementations should handle their own loading in `Load()`; Calling LoadComplete(contentInstance) when ready.")]
        protected AbstractUnityContentController(string contentGroupID, string contentLoadingID)
            : base(contentGroupID)
        {
            m_TempContentLoadingID = contentLoadingID;
        }

        protected override void DisposeSelf()
        {
            UnloadAllContentScenesAsync()
                .GetAwaiter()
                .GetResult();

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

            instance.RemoveCloneSuffix();
            TContent content = instance.GetComponent<TContent>();

            LoadComplete(content);
        }

        /// <summary>
        /// Async load a given instance of <see cref="TContent"/> from a <see cref="Resources"/> path.
        /// </summary>
        /// <param name="path">The path to the resource.</param>
        /// <returns>An async task that, when complete, provides an instance to <see cref="TContent"/>.</returns>
        protected async UniTask<TContent> LoadContentAsResourceAsync(string path)
        {
            TContent template = (TContent)await Resources.LoadAsync<TContent>(path);
            TContent content = GameObject.Instantiate(template);
            content.RemoveCloneSuffix();

            return content;
        }

        /// <summary>
        /// Async load a given scene and fetch the <see cref="TContent"/> instance from one of the scene's <see cref="GameObject"/>s.
        /// </summary>
        /// <param name="sceneName">The scene name to load.</param>
        /// <param name="mode">The scene loading mode to use.</param>
        /// <param name="setSceneActive">
        /// Whether to set the loaded scene as active. This parameter has no affect and true is implied when
        /// <see cref="mode"/>==<see cref="LoadSceneMode.Single"/>.
        /// If a scene is set to active it controls the environment (skybox, baked light, etc..) <see cref="SceneManager.SetActiveScene"/>.
        /// </param>
        /// <returns>An async task that, when complete, provides an instance to <see cref="TContent"/> from the scene.</returns>
        /// <exception cref="Exception">Thrown if an instance of <see cref="TContent"/> can't be found in the scene.</exception>
        /// <remarks>Loaded scenes will be automatically unloaded when the content is disposed.</remarks>
        protected async UniTask<TContent> LoadContentAsSceneAsync(
            string sceneName,
            LoadSceneMode mode = LoadSceneMode.Single,
            bool setSceneActive = true)
        {
            await SceneManager.LoadSceneAsync(sceneName, mode);

            // If the scene wasn't loaded it'll be invalid. This should never happen because we just loaded the scene
            // with the same path above.
            Scene scene = SceneManager.GetSceneByName(sceneName);
            Debug.Assert(scene.IsValid());
            m_LoadedContentScenes.Add(scene);

            if (setSceneActive)
            {
                // Set the scene as active so it controls the environment (skybox, baked light, etc..)
                SceneManager.SetActiveScene(scene);
            }

            if(!scene.TryFindObjectByType(out TContent content, true))
            {
                throw new Exception($"{typeof(TContent).GetReadableName()} is missing from loaded scene. Scene:{sceneName}");
            }

            return content;
        }

        private async UniTask UnloadAllContentScenesAsync()
        {
            await m_LoadedContentScenes
                .Select(scene => scene.isLoaded ? SceneManager.UnloadSceneAsync(scene).ToUniTask() : UniTask.CompletedTask);

            m_LoadedContentScenes.Clear();
        }
    }
}

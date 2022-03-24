using Anvil.CSharp.DelayedExecution;
using UnityEngine;

namespace Anvil.Unity.DelayedExecution
{
    /// <summary>
    /// A specific <see cref="AbstractUpdateSource"/> for Unity.
    /// </summary>
    /// <typeparam name="T">Unity requires a MonoBehaviour for it's Update functions, this generic is of type
    /// <see cref="AbstractUnityUpdateSourceMonoBehaviour"/></typeparam>
    public abstract class AbstractUnityUpdateSource<T> : AbstractUpdateSource where T : AbstractUnityUpdateSourceMonoBehaviour
    {
        private GameObject m_UpdateSourceGameObject;

        protected AbstractUnityUpdateSource()
        {
            m_UpdateSourceGameObject = new GameObject($"[UpdateSource - {typeof(T)}]");
            GameObject.DontDestroyOnLoad(m_UpdateSourceGameObject);
            m_UpdateSourceGameObject.transform.SetParent(UnityUpdateSourceGameObjectManager.UpdateSourceRoot);

            T updateSource = m_UpdateSourceGameObject.AddComponent<T>();
            updateSource.SetOnUpdateDispatchFunction(DispatchOnUpdateEvent);
            updateSource.OnDisposing += UpdateSource_OnDisposing;
        }

        protected override void DisposeSelf()
        {
            if (m_UpdateSourceGameObject != null)
            {
                m_UpdateSourceGameObject.GetComponent<T>().OnDisposing -= UpdateSource_OnDisposing;
                GameObject.Destroy(m_UpdateSourceGameObject);
                m_UpdateSourceGameObject = null;
            }

            base.DisposeSelf();
        }

        private void UpdateSource_OnDisposing()
        {
            // If the game object was disposed as part of Unity exiting play mode
            // then having the MonoBehaviour version disposed is ok because the rest 
            // of the application is being torn down anyway.
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            {
                // Calling dispose explicitly allows this instance to be removed from 
                // UpdateHandleSourcesManager. This is important when DomainReload is disabled.
                Dispose();
                return;
            }
#endif

            // This usually happens when m_UpdateSourceGameObject or one of its parents
            // - Is accidentally destroyed by another process desroying GameObjects in the hierary.
            // - GameObject.DontDestroyOnLoad was not applied to the game object for some reason.
            Logger.Error($"{typeof(T)} was destroyed before its owner {this.GetType()}. Dispose must be called on the owner.");
        }
    }

    /// <summary>
    /// Internal management class for Update Source Game Objects
    /// </summary>
    internal static class UnityUpdateSourceGameObjectManager
    {
        private const string UPDATE_SOURCES_GAME_OBJECT_NAME = "[UpdateSources]";
        private static Transform s_UpdateSourceRoot;

        /// <summary>
        /// Returns the Update Sources root Game Object for parenting purposes.
        /// </summary>
        public static Transform UpdateSourceRoot
        {
            get
            {
                if (s_UpdateSourceRoot == null)
                {
                    GameObject gameObject = new GameObject(UPDATE_SOURCES_GAME_OBJECT_NAME);
                    GameObject.DontDestroyOnLoad(gameObject);
                    s_UpdateSourceRoot = gameObject.transform;
                }

                return s_UpdateSourceRoot;
            }
        }
    }
}


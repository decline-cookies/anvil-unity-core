using Anvil.CSharp.DelayedExecution;
using UnityEngine;

namespace Anvil.Unity.DelayedExecution
{
    /// <summary>
    /// A specific <see cref="AbstractUpdateSource"/> for Unity.
    /// </summary>
    /// <typeparam name="T">Unity requires a MonoBehaviour for it's Update functions, this generic is of type
    /// <see cref="AbstractUnityUpdateSourceMonoBehaviour"/></typeparam>
    public abstract class AbstractUnityUpdateSource<T> : AbstractUpdateSource where T:AbstractUnityUpdateSourceMonoBehaviour
    {
        protected AbstractUnityUpdateSource()
        {
            GameObject gameObject = new GameObject($"[UpdateSource - {typeof(T)}]");
            GameObject.DontDestroyOnLoad(gameObject);
            gameObject.transform.SetParent(UnityUpdateSourceGameObjectManager.UpdateSourceRoot);
            T updateSource = gameObject.AddComponent<T>();
            updateSource.SetOnUpdateDispatchFunction(DispatchOnUpdateEvent);
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


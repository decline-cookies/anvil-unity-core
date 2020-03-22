using Anvil.CSharp.DelayedExecution;
using UnityEngine;

namespace Anvil.Unity.DelayedExecution
{
    public abstract class AbstractUnityUpdateSource<T> : AbstractUpdateSource where T:AbstractUnityUpdateSourceMonoBehaviour
    {
        protected override void Init()
        {
            GameObject gameObject = new GameObject($"[UpdateSource - {typeof(T)}]");
            GameObject.DontDestroyOnLoad(gameObject);
            gameObject.transform.SetParent(UnityUpdateSourceGameObjectManager.UpdateSourceRoot);
            T updateSource = gameObject.AddComponent<T>();
            updateSource.SetOnUpdateDispatchFunction(DispatchOnUpdateEvent);
        }
    }
    
    internal static class UnityUpdateSourceGameObjectManager
    {
        private const string UPDATE_SOURCES_GAME_OBJECT_NAME = "[UpdateSources]";
        private static Transform s_UpdateSourceRoot;
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


using Anvil.CSharp.DelayedExecution;
using UnityEngine;

namespace Anvil.Unity.DelayedExecution
{
    public abstract class AbstractUnityUpdateSource<T> : AbstractUpdateSource where T:AbstractUnityUpdateSourceMonoBehaviour
    {
        private static Transform s_UpdateSourceRoot;

        private static Transform UpdateSourceRoot
        {
            get
            {
                if (s_UpdateSourceRoot == null)
                {
                    GameObject gameObject = new GameObject("[UpdateSources]");
                    GameObject.DontDestroyOnLoad(gameObject);
                    s_UpdateSourceRoot = gameObject.transform;
                }

                return s_UpdateSourceRoot;
            }
        }
        protected override void Initialize()
        {
            GameObject gameObject = new GameObject($"[UpdateSource - {typeof(T)}]");
            gameObject.transform.SetParent(UpdateSourceRoot);
            GameObject.DontDestroyOnLoad(gameObject);
            T updateSource = gameObject.AddComponent<T>();
            updateSource.SetOnUpdateDispatchFunction(DispatchOnUpdateEvent);
        }
    }
}


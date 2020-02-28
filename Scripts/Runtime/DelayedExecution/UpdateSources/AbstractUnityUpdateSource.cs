using Anvil.CSharp.DelayedExecution;
using UnityEngine;

namespace Anvil.Unity.DelayedExecution
{
    public abstract class AbstractUnityUpdateSource<T> : AbstractUpdateSource where T:AbstractUnityUpdateSourceMonoBehaviour
    {
        protected override void Initialize()
        {
            GameObject gameObject = new GameObject($"[UpdateSource - MonoBehaviour - {typeof(T)}]");
            GameObject.DontDestroyOnLoad(gameObject);
            T updateSource = gameObject.AddComponent<T>();
            updateSource.SetOnUpdateDispatchFunction(DispatchOnUpdateEvent);
        }
    }
}


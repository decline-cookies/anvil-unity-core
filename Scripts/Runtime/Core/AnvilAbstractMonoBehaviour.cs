using System;
using UnityEngine;

namespace Anvil.Unity.Core
{
    public abstract class AnvilAbstractMonoBehaviour : MonoBehaviour, IDisposable
    {
        public bool IsDisposed { get; private set; }
        public bool IsGameObjectDestroyed { get; private set; }

        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            IsDisposed = true;
            DisposeSelf();

            if (IsGameObjectDestroyed)
            {
                return;
            }
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            if (IsGameObjectDestroyed)
            {
                return;
            }
            IsGameObjectDestroyed = true;
            Dispose();
        }

        protected virtual void DisposeSelf()
        {
            
        }
    }
}


using System;
using Anvil.Unity.Util;
using UnityEngine;

namespace Anvil.Unity.Core
{
    public abstract class AbstractAnvilMonoBehaviour : MonoBehaviour, IDisposable
    {
        public bool IsDisposed { get; private set; }
        public bool IsGameObjectDestroyed { get; private set; }

        protected virtual void Awake()
        {
#if DEBUG
            MonoBehaviourUtil.EnforceEditorExposedFieldsSet(this);
#endif
        }

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


using System;
using Anvil.Unity.Util;
using UnityEngine;

namespace Anvil.Unity.Core
{
    /// <summary>
    /// The base class for any <see cref="MonoBehaviour"/> in the Anvil Framework
    /// Provides nice linking between the <see cref="GameObject"/> and this class so that if one is Destroyed/Disposed,
    /// the other is as well. This prevents lingering GameObjects in the Unity Hierarchy or classes that have had
    /// their GameObject's destroyed.
    /// </summary>
    public abstract class AbstractAnvilMonoBehaviour : MonoBehaviour, IDisposable
    {
        /// <summary>
        /// Allows an instance to be queried to know if <see cref="Dispose"/> has been called yet or not.
        /// </summary>
        public bool IsDisposed { get; private set; }
        
        /// <summary>
        /// Allows an instance to be queried to know if its <see cref="GameObject"/> has been destroyed or not.
        /// </summary>
        public bool IsGameObjectDestroyed { get; private set; }

        protected virtual void Awake()
        {
#if DEBUG
            MonoBehaviourUtil.EnforceEditorExposedFieldsSet(this);
#endif
        }
        
        /// <summary>
        /// <inheritdoc cref="IDisposable.Dispose"/>
        /// Will early return if <see cref="IsDisposed"/> is true.
        /// Calls the virtual method <see cref="DisposeSelf"/> for inherited classes to override.
        /// Will Destroy the attached <see cref="GameObject"/> if it has not been destroyed yet.
        /// </summary>
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
        
        /// <summary>
        /// Override to implement specific Dispose logic.
        /// </summary>
        protected virtual void DisposeSelf()
        {
            
        }
    }
}


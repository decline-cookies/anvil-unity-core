using System;
using UnityEngine;

namespace Anvil.Unity.Core
{
    /// <summary>
    /// The base class for any <see cref="MonoBehaviour"/> in the Anvil Framework
    /// Provides nice linking between the <see cref="GameObject"/> and this class so that if one is Destroyed/Disposed,
    /// the other is as well. This prevents lingering GameObjects in the Unity Hierarchy or classes that have had
    /// their GameObjects destroyed.
    /// </summary>
    public abstract class AbstractAnvilMonoBehaviour : MonoBehaviour, IDisposable
    {
        /// <summary>
        /// Allows an instance to be queried to know if <see cref="Dispose"/> has been called yet or not and if
        /// the instance has been completely disposed. All <see cref="DisposeSelf"/> functions down the inheritance
        /// chain have been called.
        /// </summary>
        public bool IsDisposed { get; private set; }
        
        /// <summary>
        /// Allows an instance to be queried to know if <see cref="Dispose"/> has been called yet or not and if
        /// the instance is currently disposing.
        /// </summary>
        public bool IsDisposing { get; private set; }

        protected virtual void Awake()
        {
#if DEBUG
            MonoBehaviourUtil.EnforceEditorExposedFieldsSet(this);
#endif
        }
        
        /// <summary>
        /// <inheritdoc cref="IDisposable.Dispose"/>
        /// Will early return if <see cref="IsDisposed"/> or <see cref="IsDisposing"/> is true.
        /// Calls the virtual method <see cref="DisposeSelf"/> for inherited classes to override.
        /// Will Destroy the attached <see cref="GameObject"/> if it has not been destroyed yet.
        /// </summary>
        public void Dispose()
        {
            if (IsDisposing || IsDisposed)
            {
                return;
            }

            IsDisposing = true;
            DisposeSelf();
            Destroy(gameObject);
            IsDisposing = false;
            IsDisposed = true;
        }
        
        private void OnDestroy()
        {
            if (IsDisposing || IsDisposed)
            {
                return;
            }
            
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


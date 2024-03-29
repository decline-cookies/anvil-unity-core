﻿using System;
using Anvil.CSharp.Logging;
using Anvil.CSharp.Core;
using UnityEngine;
using Logger = Anvil.CSharp.Logging.Logger;

namespace Anvil.Unity.Core
{
    /// <summary>
    /// The base class for any <see cref="MonoBehaviour"/> in the Anvil Framework
    /// Provides nice linking between the <see cref="GameObject"/> and this class so that if one is Destroyed/Disposed,
    /// the other is as well. This prevents lingering GameObjects in the Unity Hierarchy or classes that have had
    /// their GameObjects destroyed.
    /// </summary>
    public abstract class AbstractAnvilMonoBehaviour : MonoBehaviour, IAnvilDisposable
    {
        /// <summary>
        /// <inheritdoc cref="IAnvilDisposable.IsDisposed"/>
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// <inheritdoc cref="IAnvilDisposable.IsDisposing"/>
        /// </summary>
        public bool IsDisposing { get; private set; }

        private Logger? m_Logger;

        /// <summary>
        /// Returns a <see cref="Logger"/> for this instance to emit log messages with.
        /// Lazy instantiated.
        /// </summary>
        protected Logger Logger
        {
            get => m_Logger ?? (m_Logger = Log.GetLogger(this)).Value;
            set => m_Logger = value;
        }

        protected virtual void Awake()
        {
#if DEBUG
            MonoBehaviourUtil.EnforceEditorExposedFieldReferencesSet(this);
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
            Dispose(true);
        }

        private void Dispose(bool shouldDestroyGameObject)
        {
            if (IsDisposing || IsDisposed)
            {
                return;
            }

            IsDisposing = true;
            DisposeSelf();

            if (shouldDestroyGameObject)
            {
#if !UNITY_EDITOR
                Destroy(gameObject);
#else
                //If we're running in the Editor via the ExecuteAlways attribute, we're not allowed to call Destroy
                if (!Application.isPlaying)
                {
                    DestroyImmediate(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
#endif
            }


            IsDisposing = false;
            IsDisposed = true;
        }

        private void OnDestroy()
        {
            if (IsDisposing
             || IsDisposed)
            {
                return;
            }

            Dispose(false);
        }

        /// <summary>
        /// Override to implement specific Dispose logic.
        /// </summary>
        protected virtual void DisposeSelf() { }
    }
}
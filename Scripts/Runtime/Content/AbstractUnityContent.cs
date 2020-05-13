﻿using Anvil.CSharp.Content;
using Anvil.Unity.Core;

namespace Anvil.Unity.Content
{
    /// <summary>
    /// Convenience class to allow <see cref="AbstractUnityContentController"/> corresponding to <see cref="AbstractUnityContent"/> to be specific
    /// and strongly typed.
    /// </summary>
    /// <typeparam name="TController">A specific implementation of <see cref="AbstractUnityContent"/>.</typeparam>
    public abstract class AbstractUnityContent<TController> : AbstractUnityContent
        where TController : AbstractContentController
    {
        /// <summary>
        /// Gets/Sets the corresponding Controller as strongly typed version of <see cref="AbstractUnityContentController"/>
        /// </summary>
        public new TController Controller { get; internal set; }
    }
    
    /// <summary>
    /// A more specific implementation of <see cref="IContent"/> for usage with Unity where the Content is a
    /// <see cref="AbstractAnvilMonoBehaviour"/>
    /// </summary>
    public abstract class AbstractUnityContent : AbstractAnvilMonoBehaviour, IContent
    {
        /// <summary>
        /// <inheritdoc cref="IContent.Controller"/>
        /// </summary>
        public AbstractContentController Controller { get; internal set; }


        protected override void DisposeSelf()
        {
            Controller?.Dispose();
            Controller = null;

            base.DisposeSelf();
        }
    }
}


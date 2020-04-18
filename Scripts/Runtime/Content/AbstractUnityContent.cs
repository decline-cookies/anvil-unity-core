using System;
using Anvil.CSharp.Content;
using Anvil.Unity.Core;

namespace Anvil.Unity.Content
{
    /// <summary>
    /// A more specific implementation of <see cref="IContent"/> for usage with Unity where the Content is a
    /// <see cref="AbstractAnvilMonoBehaviour"/>
    /// </summary>
    public abstract class AbstractUnityContent : AbstractAnvilMonoBehaviour, IContent
    {
        /// <summary>
        /// <inheritdoc cref="IContent.OnContentDisposing"/>
        /// </summary>
        public event Action OnContentDisposing;
        
        private bool m_IsContentDisposing;

        protected override void DisposeSelf()
        {
            if (m_IsContentDisposing)
            {
                return;
            }
            m_IsContentDisposing = true;
            
            OnContentDisposing?.Invoke();
            OnContentDisposing = null;

            base.DisposeSelf();
        }
    }
}


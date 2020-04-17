using System;
using Anvil.CSharp.Content;
using Anvil.Unity.Core;

namespace Anvil.Unity.Content
{
    // public abstract class AbstractUnityContent<T> : AbstractUnityContent 
    //     where T : AbstractContentController
    // {
    //     public new T Controller => (T)base.Controller;
    // }

    public abstract class AbstractUnityContent : AbstractAnvilMonoBehaviour, IContent
    {
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


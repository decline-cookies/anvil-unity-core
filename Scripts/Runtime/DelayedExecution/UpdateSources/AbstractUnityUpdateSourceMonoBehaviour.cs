using System;
using Anvil.Unity.Core;

namespace Anvil.Unity.DelayedExecution
{
    /// <summary>
    /// A base <see cref="AbstractAnvilMonoBehaviour"/> that provides functionality for deriving classes to
    /// issue OnUpdate events to a <see cref="AbstractUnityUpdateSource{T}"/>
    /// </summary>
    public class AbstractUnityUpdateSourceMonoBehaviour : AbstractAnvilMonoBehaviour
    {
        private Action m_OnUpdateDispatchFunction;

        protected override void DisposeSelf()
        {
            m_OnUpdateDispatchFunction = null;
            base.DisposeSelf();
        }
        
        /// <summary>
        /// Sets the function to call when OnUpdate from this MonoBehaviour is fired.
        /// </summary>
        /// <param name="onUpdateDispatchFunction"></param>
        public void SetOnUpdateDispatchFunction(Action onUpdateDispatchFunction)
        {
            m_OnUpdateDispatchFunction = onUpdateDispatchFunction;
        }

        protected void TriggerOnUpdateDispatchFunction()
        {
            m_OnUpdateDispatchFunction?.Invoke();
        }
    }
}


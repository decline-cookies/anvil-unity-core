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
        /// <summary>
        /// Dispatches when the <see cref="AbstractUnityUpdateSourceMonoBehaviour"/> is being disposed.
        /// </summary>
        internal event Action OnDisposing;

        private Action m_OnUpdateDispatchFunction;

        protected override void DisposeSelf()
        {
            OnDisposing?.Invoke();
            OnDisposing = null;

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

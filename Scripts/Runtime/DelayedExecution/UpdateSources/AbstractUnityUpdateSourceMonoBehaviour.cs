using System;
using Anvil.Unity.Core;

namespace Anvil.Unity.DelayedExecution
{
    public class AbstractUnityUpdateSourceMonoBehaviour : AnvilAbstractMonoBehaviour
    {
        private Action m_OnUpdateDispatchFunction;

        protected override void DisposeSelf()
        {
            m_OnUpdateDispatchFunction = null;
            base.DisposeSelf();
        }

        public void SetOnUpdateDispatchFunction(Action onUpdateDispatchFunction)
        {
            m_OnUpdateDispatchFunction = onUpdateDispatchFunction;
        }

        public void TriggerOnUpdateDispatchFunction()
        {
            m_OnUpdateDispatchFunction?.Invoke();
        }
    }
}


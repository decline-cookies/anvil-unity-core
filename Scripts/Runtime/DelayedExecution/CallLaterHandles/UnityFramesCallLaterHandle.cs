using System;
using System.Collections.Generic;
using Anvil.CSharp.DelayedExecution;
using UnityEngine;

namespace Anvil.Unity.DelayedExecution
{
    public class UnityFramesCallLaterHandle : AbstractCallLaterHandle
    {
        public static UnityFramesCallLaterHandle Create(uint framesToWait, Action callback)
        {
            return new UnityFramesCallLaterHandle(framesToWait, callback);
        }
        
        private static readonly List<Type> s_ValidUpdateSourceTypes = new List<Type>()
        {
            typeof(UnityUpdateSource),
            typeof(UnityLateUpdateSource)
        };
        

        private readonly uint m_FramesToWait;
        private uint m_FramesWaited;

        private UnityFramesCallLaterHandle(uint framesToWait, Action callback) : base(callback)
        {
            m_FramesToWait = framesToWait;
            m_FramesWaited = 0;
        }

        protected override void HandleOnUpdate()
        {
            m_FramesWaited++;
            if (m_FramesWaited >= m_FramesToWait)
            {
                Complete();
            }
        }

        protected override List<Type> GetValidUpdateSourceTypes()
        {
            return s_ValidUpdateSourceTypes;
        }
    }
}


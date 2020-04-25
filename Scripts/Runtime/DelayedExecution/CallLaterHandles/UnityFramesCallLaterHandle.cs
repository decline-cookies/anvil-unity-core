using System;
using System.Collections.Generic;
using Anvil.CSharp.DelayedExecution;
using UnityEngine;

namespace Anvil.Unity.DelayedExecution
{
    /// <summary>
    /// Allows for calling a function after some amount of frames as calculated by number
    /// of times a Unity OnUpdate or OnLateUpdate are called.
    /// </summary>
    public class UnityFramesCallLaterHandle : AbstractCallLaterHandle
    {
        /// <summary>
        /// Convenience method for creation of a <see cref="UnityFramesCallLaterHandle"/>
        /// </summary>
        /// <param name="framesToWait">The amount of frames to wait before calling the function.</param>
        /// <param name="callback">The function to call.</param>
        /// <returns>The instance of the <see cref="UnityFramesCallLaterHandle"/></returns>
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


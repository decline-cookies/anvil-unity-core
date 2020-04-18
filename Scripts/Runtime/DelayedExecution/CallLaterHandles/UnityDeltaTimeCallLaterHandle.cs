using System;
using System.Collections.Generic;
using Anvil.CSharp.DelayedExecution;
using UnityEngine;

namespace Anvil.Unity.DelayedExecution
{
    /// <summary>
    /// Allows for calling a function after some amount of <see cref="Time.deltaTime"/>
    /// </summary>
    public class UnityDeltaTimeCallLaterHandle : AbstractCallLaterHandle
    {
        /// <summary>
        /// Convenience method for creation of a <see cref="UnityDeltaTimeCallLaterHandle"/>
        /// </summary>
        /// <param name="secondsToWait">The amount of seconds to wait before calling the function.</param>
        /// <param name="callback">The function to call.</param>
        /// <returns>The instance of the <see cref="UnityDeltaTimeCallLaterHandle"/></returns>
        public static UnityDeltaTimeCallLaterHandle Create(float secondsToWait, Action callback)
        {
            return new UnityDeltaTimeCallLaterHandle(secondsToWait, callback);
        }
        
        private static readonly List<Type> s_ValidUpdateSourceTypes = new List<Type>()
        {
            typeof(UnityUpdateSource),
            typeof(UnityLateUpdateSource)
        };
        
        private readonly float m_SecondsToWait;
        private float m_SecondsWaited;
        
        private UnityDeltaTimeCallLaterHandle(float secondsToWait, Action callback) : base(callback)
        {
            m_SecondsToWait = secondsToWait;
            m_SecondsWaited = 0.0f;
        }

        protected override void HandleOnUpdate()
        {
            m_SecondsWaited += Time.deltaTime;
            if (m_SecondsWaited >= m_SecondsToWait)
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


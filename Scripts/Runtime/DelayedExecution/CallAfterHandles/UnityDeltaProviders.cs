using Anvil.CSharp.DelayedExecution;
using UnityEngine;

namespace Anvil.Unity.DelayedExecution
{
    /// <summary>
    /// Commonly used <see cref="DeltaProvider"/> functions for use with Unity.
    /// </summary>
    public static class UnityDeltaProviders
    {
        /// <summary>
        /// Returns 1 every time. A counter for frames. Usually paired with an <see cref="UnityUpdateSource"/>.
        /// </summary>
        public static readonly DeltaProvider Frames = () => 1;

        /// <summary>
        /// Returns <see cref="Time.deltaTime"/>. Usually paired with an <see cref="UnityUpdateSource"/>.
        /// </summary>
        public static readonly DeltaProvider DeltaTime = () => Time.deltaTime;

        /// <summary>
        /// Returns <see cref="Time.unscaledDeltaTime"/>. Usually paired with an <see cref="UnityUpdateSource"/>.
        /// </summary>
        public static readonly DeltaProvider UnscaledDeltaTime = () => Time.unscaledDeltaTime;

        /// <summary>
        /// Returns <see cref="Time.fixedDeltaTime"/>. Usually paired with an <see cref="UnityFixedUpdateSource"/>.
        /// </summary>
        public static readonly DeltaProvider FixedDeltaTime = () => Time.fixedDeltaTime;

        /// <summary>
        /// Returns <see cref="Time.fixedUnscaledDeltaTime"/>. Usually paired with an <see cref="UnityFixedUpdateSource"/>.
        /// </summary>
        public static readonly DeltaProvider UnscaledFixedDeltaTime = () => Time.fixedUnscaledDeltaTime;
    }
}

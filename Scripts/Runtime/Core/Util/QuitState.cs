using System;
using UnityEngine;

namespace Anvil.Unity.Core
{
    /// <summary>
    /// Tracks and notifies the quit state of the application.
    /// Adds the ability to poll whether the application is currently quitting vs just listening to
    /// <see cref="Application.quitting"/>.
    /// </summary>
    public static class QuitState
    {
        /// <summary>
        /// A quit event that is invoked when <see cref="Application.quitting"/> is invoked but is guaranteed to happen
        /// after <see cref="IsQuitting"/> is true.
        /// (See remarks on <see cref="IsQuitting"/> for details.)
        /// </summary>
        public static event Action OnQuitting;

        private static bool s_IsInitialized;
        /// <summary>
        /// True if the application is currently in the process of quitting.
        /// (IE: The <see cref="Application.quitting"/> has been invoked.)
        /// </summary>
        /// <remarks>
        /// A best effort is made to be the first listener on the <see cref="Application.quitting"/> event but:
        ///  - C# doesn't guarantee that event handlers will be executed in the order of subscription.
        ///    (Although in practice that's how it currently behaves.)
        ///  - It is possible to listen to the event before this class does.
        ///
        /// This means that <see cref="IsQuitting"/> may not be true if you are checking it in the stack of another
        /// quitting event handler.
        ///
        /// If you want to be absolutely sure that <see cref="IsQuitting"/> reflects the correct state then subscribe
        /// to <see cref="QuitState.OnQuitting"/> which is called after the boolean has flipped.
        ///
        /// Otherwise, subscribing to <see cref="Application.quitting"/> after the
        /// <see cref="RuntimeInitializeLoadType.SubsystemRegistration"/> phase is also safe.
        /// </remarks>
        public static bool IsQuitting { get; private set; }

        static QuitState()
        {
            Init();
        }

        // Force initialization if the static constructor didn't get triggered yet.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            if (s_IsInitialized)
            {
                return;
            }

            Application.quitting += Application_quitting;
        }

        private static void Application_quitting()
        {
            IsQuitting = true;
            OnQuitting?.Invoke();
        }
    }
}
using Anvil.CSharp.Core;
using UnityEngine;

namespace Anvil.Unity.Core
{
    /// <summary>
    /// An instance that monitors the quit state of the application.
    /// Instantiate as soon as possible and query <see cref="isQuitting"/> when required.
    /// </summary>
    public class QuitStateMonitor : AbstractAnvilBase
    {
        /// <summary>
        /// True if the application is currently in the process of quitting.
        /// (IE: The <see cref="Application.quitting"/> has been invoked.)
        /// </summary>
        public bool isQuitting { get; private set; }

        /// <summary>
        /// Creates an instance of the state monitor.
        /// </summary>
        public QuitStateMonitor()
        {
            Application.quitting += Application_quitting;
        }

        protected override void DisposeSelf()
        {
            Application.quitting -= Application_quitting;

            base.DisposeSelf();
        }

        private void Application_quitting()
        {
            isQuitting = true;
        }
    }
}
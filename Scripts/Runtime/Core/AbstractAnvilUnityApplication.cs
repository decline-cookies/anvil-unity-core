using System;
using UnityEngine;

namespace Anvil.Unity.Core
{
    /// <summary>
    /// An implementation of <see cref="IAnvilApplication"/> for a Unity application.
    /// </summary>
    public abstract class AbstractAnvilUnityApplication : AbstractAnvilMonoBehaviour
    {
        /// <summary>
        /// Dispatches when the application is paused via <see cref="MonoBehaviour.OnApplicationPause(true)"/>
        /// </summary>
        public event Action OnAppPaused;
        /// <summary>
        /// Dispatches when the application is resumed via <see cref="MonoBehaviour.OnApplicationPause(false)"/>
        /// </summary>
        public event Action OnAppResumed;
        /// <summary>
        /// Dispatches when the application gains focus via <see cref="MonoBehaviour.OnApplicationFocus(true)"/>
        /// </summary>
        public event Action OnAppGainedFocus;
        /// <summary>
        /// Dispatches when the application loses focus via <see cref="MonoBehaviour.OnApplicationFocus(false)"/>
        /// </summary>
        public event Action OnAppLostFocus;
        /// <summary>
        /// Dispatches when the application is quitting via <see cref="QuitState.OnQuitting"/>. This will not dispatch
        /// if the application is force quit or crashes. The application cannot cancel the quit process at the time this
        /// dispatches. <see cref="Application.wantsToQuit"/> if the quit process needs to be cancelled.
        /// </summary>
        public event Action OnAppQuitting
        {
            add => QuitState.OnQuitting += value;
            remove => QuitState.OnQuitting -= value;
        }

        /// <inheritdoc cref="QuitState.IsQuitting"/>
        public bool IsQuitting
        {
            get => QuitState.IsQuitting;
        }

        /// <summary>
        /// An array of GameObjects that are linked to the application that shouldn't be destroyed on a
        /// non-additive scene load.
        /// </summary>
        [SerializeField] public GameObject[] DontDestroyOnLoadGameObjects;


        //Sealing to ensure inheriting classes can't override this behaviour
        protected sealed override void Awake()
        {
            base.Awake();

            InitDontDestroyOnLoadGameObjects();
            Init();
        }

        protected override void DisposeSelf()
        {
            OnAppPaused = null;
            OnAppResumed = null;
            OnAppGainedFocus = null;
            OnAppLostFocus = null;

            base.DisposeSelf();
        }

        private void InitDontDestroyOnLoadGameObjects()
        {
            //Ensure this application is protected
            DontDestroyOnLoad(gameObject);
            foreach (GameObject gameObject in DontDestroyOnLoadGameObjects)
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        /// <summary>
        /// The entry point for the application to perform setup and begin.
        /// </summary>
        protected abstract void Init();

        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                OnAppGainedFocus?.Invoke();
            }
            else
            {
                OnAppLostFocus?.Invoke();
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                OnAppPaused?.Invoke();
            }
            else
            {
                OnAppResumed?.Invoke();
            }
        }
    }
}
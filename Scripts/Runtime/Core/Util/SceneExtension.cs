using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

namespace Anvil.Unity.Core
{
    /// <summary>
    /// A collection of extension methods for working with <see cref="Scene"/>s
    /// </summary>
    public static class SceneExtension
    {
        /// <summary>
        /// Get a component from any of the root game objects in the given scene.
        /// </summary>
        /// <param name="scene">The scene to search.</param>
        /// <param name="includeInactive">Whether to include inactive game objects in the search.</param>
        /// <typeparam name="T">The component type to search for.</typeparam>
        /// <returns>The found component instance, or null if not found.</returns>
        public static T FindObjectByTypeAtRoot<T>(this Scene scene, bool includeInactive = false) where T : Component
        {
            T component = null;
            List<GameObject> rootGameObjects = ListPool<GameObject>.Get();
            scene.GetRootGameObjects(rootGameObjects);
            foreach (GameObject go in rootGameObjects)
            {
                if ((go.activeSelf || includeInactive) && go.TryGetComponent(out component))
                {
                    break;
                }
            }
            ListPool<GameObject>.Release(rootGameObjects);
            return component;
        }

        /// <summary>
        /// Try to get a component from any of the root game objects in the given scene.
        /// </summary>
        /// <param name="scene">The scene to search.</param>
        /// <param name="component">The found component instance.</param>
        /// <param name="includeInactive">Whether to include inactive game objects in the search.</param>
        /// <typeparam name="T">The component type to search for.</typeparam>
        /// <returns>True if the component is found.</returns>
        public static bool TryFindObjectByTypeAtRoot<T>(this Scene scene, out T component, bool includeInactive = false) where T : Component
        {
            component = scene.FindObjectByTypeAtRoot<T>(includeInactive);
            return component != null;
        }

        /// <summary>
        /// Get a component from any game object in the given scene, including children.
        /// </summary>
        /// <param name="scene">The scene to search.</param>
        /// <param name="includeInactive">Whether to include inactive game objects in the search.</param>
        /// <typeparam name="T">The component type to search for.</typeparam>
        /// <returns>The found component instance, or null if not found.</returns>
        public static T FindObjectByType<T>(this Scene scene, bool includeInactive = false) where T : Component
        {
            T component = null;
            List<GameObject> rootGameObjects = ListPool<GameObject>.Get();
            scene.GetRootGameObjects(rootGameObjects);
            foreach (GameObject go in rootGameObjects)
            {
                component = go.GetComponentInChildren<T>(includeInactive);
                if (component != null)
                {
                    break;
                }
            }
            ListPool<GameObject>.Release(rootGameObjects);
            return component;
        }

        /// <summary>
        /// Try to get a component from any game object in the given scene, including children.
        /// </summary>
        /// <param name="scene">The scene to search.</param>
        /// <param name="component">The found component instance.</param>
        /// <param name="includeInactive">Whether to include inactive game objects in the search.</param>
        /// <typeparam name="T">The component type to search for.</typeparam>
        /// <returns>True if the component is found.</returns>
        public static bool TryFindObjectByType<T>(this Scene scene, out T component, bool includeInactive = false) where T : Component
        {
            component = scene.FindObjectByType<T>(includeInactive);
            return component != null;
        }
    }
}

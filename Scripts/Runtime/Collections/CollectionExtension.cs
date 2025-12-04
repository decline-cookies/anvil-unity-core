using System.Collections.Generic;
using UnityEngine;

namespace Anvil.Unity.Collections
{
    /// <summary>
    /// Extension methods for use with <see cref="ICollection{T}"/> instances.
    /// Adds Unity specific extensions to supplement what's already provided by
    /// <see cref="Anvil.CSharp.Collections.CollectionExtension"/>.
    /// </summary>
    public static class CollectionExtension
    {
        /// <summary>
        /// Destroy all elements of a <see cref="ICollection{T}"/> and then clear the collection.
        /// </summary>
        /// <param name="collection">The <see cref="ICollection{T}"/> to operate on.</param>
        /// <typeparam name="T">The element type</typeparam>
        /// <remarks>
        /// This method will only clear the <see cref="collection"/> if <see cref="ICollection{T}.IsReadOnly"/> is false.
        /// </remarks>
        public static void DestroyAllAndTryClear<T>(this ICollection<T> collection) where T : Object
        {
            foreach (T item in collection)
            {
                Component.Destroy(item);
            }

            if (!collection.IsReadOnly)
            {
                collection.Clear();
            }
        }

        /// <summary>
        /// Destroy the <see cref="GameObject"/> hosting each element of a <see cref="ICollection{T}"/> and then clear
        /// the collection.
        /// </summary>
        /// <param name="collection">The <see cref="ICollection{T}"/> to operate on.</param>
        /// <typeparam name="T">The element type</typeparam>
        /// <remarks>
        /// This method will only clear the <see cref="collection"/> if <see cref="ICollection{T}.IsReadOnly"/> is false.
        /// </remarks>
        public static void DestroyAllGameObjectsAndTryClear<T>(this ICollection<T> collection) where T : Component
        {
            foreach (T item in collection)
            {
                Component.Destroy(item.gameObject);
            }

            if (!collection.IsReadOnly)
            {
                collection.Clear();
            }
        }
    }
}

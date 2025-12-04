using System.Collections.Generic;
using Anvil.CSharp.Collections;
using UnityEngine;

namespace Anvil.Unity.Collections
{
    /// <summary>
    /// Extension methods for use with <see cref="IList{T}"/> instances.
    /// Adds Unity specific extensions to supplement what's already provided by
    /// <see cref="Anvil.CSharp.Collections.ListExtension"/>.
    /// </summary>
    public static class ListExtension
    {
        /// <summary>
        /// Shuffle the elements in a list using the Fisher-Yates algorithm.
        /// </summary>
        /// <param name="collection">The collection to shuffle the elements of</param>
        public static void Shuffle<T>(this IList<T> collection)
        {
            collection.Shuffle(Random.Range);
        }
    }
}

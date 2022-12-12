using UnityEngine;

namespace Anvil.Unity.Core
{
    /// <summary>
    /// A collection of extension methods for working with <see cref="Transform"/>
    /// </summary>
    public static class TransformExtension
    {
        /// <summary>
        /// Sets the world scale on a transform.
        /// </summary>
        /// <param name="transform">The transform to set the scale on.</param>
        /// <param name="scale">The desired world scale.</param>
        /// <remarks>Taken from: http://answers.unity.com/answers/1343678/view.html</remarks>
        public static void SetLossyScale(this Transform transform, Vector3 scale)
        {
            transform.localScale = Vector3.one;
            transform.localScale = new Vector3 (
                scale.x/transform.lossyScale.x,
                scale.y/transform.lossyScale.y,
                scale.z/transform.lossyScale.z);
        }
    }
}
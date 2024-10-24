using UnityEngine;

namespace Anvil.Unity.Core
{
    /// <summary>
    /// A collection of extension methods for working with <see cref="Transform"/>
    /// </summary>
    public static class TransformExtension
    {
        /// <summary>
        /// Resets the transform's local-space values to default (position zero, rotation zero, scale one).
        /// </summary>
        /// <param name="transform">The transform to reset.</param>
        public static void Reset(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        /// <summary>
        /// Sets the world scale on a transform.
        /// </summary>
        /// <param name="transform">The transform to set the scale on.</param>
        /// <param name="scale">The desired world scale.</param>
        /// <remarks>Taken from: http://answers.unity.com/answers/1343678/view.html</remarks>
        public static void SetLossyScale(this Transform transform, Vector3 scale)
        {
            transform.localScale = Vector3.one;
            transform.localScale = new Vector3(
                scale.x / transform.lossyScale.x,
                scale.y / transform.lossyScale.y,
                scale.z / transform.lossyScale.z);
        }

        /// <inheritdoc cref="ScaleAroundPoint(UnityEngine.Transform,UnityEngine.Vector3,UnityEngine.Vector3)"/>
        public static void ScaleAroundPoint(this Transform transform, float scale, Vector3 worldSpacePivot)
        {
            transform.ScaleAroundPoint(Vector3.one * scale, worldSpacePivot);
        }

        /// <summary>
        /// Scale the transform around a world-space pivot.
        /// </summary>
        /// <param name="transform">The transform to scale.</param>
        /// <param name="scale">The scale factor applied to the transform.</param>
        /// <param name="worldSpacePivot">The pivot point on which the scale is centered.</param>
        public static void ScaleAroundPoint(this Transform transform, Vector3 scale, Vector3 worldSpacePivot)
        {
            Vector3 pivotToPos = transform.position - worldSpacePivot;
            Vector3 localScale = transform.localScale;
            localScale.Scale(scale);
            transform.localScale = localScale;
            transform.localPosition += Vector3.Scale(pivotToPos, scale - Vector3.one);
        }
    }
}

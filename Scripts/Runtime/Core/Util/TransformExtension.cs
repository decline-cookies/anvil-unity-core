using System.Runtime.CompilerServices;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ConvertLocalPointFrom(this Transform targetTransform, Transform sourceTransform, Vector3 localPoint)
        {
            Vector3 point = sourceTransform.TransformPoint(localPoint);
            return targetTransform.InverseTransformPoint(point);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ConvertLocalVectorFrom(this Transform targetTransform, Transform sourceTransform, Vector3 localVector)
        {
            Vector3 vector = sourceTransform.TransformVector(localVector);
            return targetTransform.InverseTransformVector(vector);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ConvertLocalDirectionFrom(this Transform targetTransform, Transform sourceTransform, Vector3 localDirection)
        {
            Vector3 direction = sourceTransform.TransformDirection(localDirection);
            return targetTransform.InverseTransformDirection(direction);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion ConvertLocalRotationFrom(this Transform targetTransform, Transform sourceTransform, Quaternion localRotation)
        {
            Quaternion rotation = (sourceTransform.rotation * localRotation);
            return Quaternion.Inverse(targetTransform.rotation) * rotation;
        }
    }
}
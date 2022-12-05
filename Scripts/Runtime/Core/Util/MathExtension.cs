using System.Runtime.CompilerServices;
using Anvil.CSharp.Mathematics;
using Unity.Mathematics;

namespace Anvil.Unity.Core
{
    /// <summary>
    /// A collection of extension methods for working with types under the <see cref="Unity.Mathematics"/> namespace.
    /// (float3, int2, etc..)
    /// </summary>
    public static class MathExtension
    {
        /// <summary>
        /// Get the inverse of a <see cref="float3"/>.
        /// </summary>
        /// <remarks>
        /// Any components that are 0 will invert to <see cref="float.NaN"/>.
        /// Use <see cref="GetInverseSafe" /> if this is a possibility.
        /// </remarks>
        /// <param name="value">The value to get the inverse of.</param>
        /// <returns>The inverted value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 GetInverse(this float3 value)
        {
            return 1f / value;
        }

        /// <summary>
        /// Get the inverse of a <see cref="float3"/> with any <see cref="float.NaN"/> or infinite components set to 0
        /// </summary>
        /// <param name="value">The value to get the inverse of.</param>
        /// <returns>The inverted value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 GetInverseSafe(this float3 value)
        {
            float3 inverse = value.GetInverse();
            return math.select(inverse, float3.zero, math.isinf(inverse) | math.isnan(inverse));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsApproximately(this float a, float b)
        {
            return math.abs(a - b) < MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2 IsApproximately(this float2 a, float2 b)
        {
            return math.abs(a - b) < MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool3 IsApproximately(this float3 a, float3 b)
        {
            return math.abs(a - b) < MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool4 IsApproximately(this float4 a, float4 b)
        {
            return math.abs(a - b) < MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool3x3 IsApproximately(this float3x3 a, float3x3 b)
        {
            return new bool3x3(
                a.c0.IsApproximately(b.c0),
                a.c1.IsApproximately(b.c1),
                a.c2.IsApproximately(b.c2)
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool4x4 IsApproximately(this float4x4 a, float4x4 b)
        {
            return new bool4x4(
                a.c0.IsApproximately(b.c0),
                a.c1.IsApproximately(b.c1),
                a.c2.IsApproximately(b.c2),
                a.c3.IsApproximately(b.c3)
            );
        }

        /// <summary>
        /// Converts the components of a value to infinite quantities where:
        /// >0 = <see cref="float.PositiveInfinity"/>
        /// <0 = <see cref="float.NegativeInfinity"/>
        /// 0 = 0
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A signed infinite value.</returns>
        public static float3 ToSignedInfinite(this float3 value)
        {
            return math.select(math.sign(value) * new float3(float.PositiveInfinity), value, value == 0);
        }
    }
}
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
        /// Get the inverse of a <see cref="float3"/> with any <see cref="float.NaN"/> or infinite components set to 0.
        /// </summary>
        /// <param name="value">The value to get the inverse of.</param>
        /// <returns>The inverted value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 GetInverseSafe(this float3 value)
        {
            float3 inverse = value.GetInverse();
            return math.select(inverse, float3.zero, math.isinf(inverse) | math.isnan(inverse));
        }

        /// <summary>
        /// Checks if two <see cref="float"/> values are approximately equal according to the
        /// <see cref="MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE"/>.
        /// </summary>
        /// <param name="a">The <see cref="float"/> to compare.</param>
        /// <param name="b">The <see cref="float"/> to compare.</param>
        /// <returns>
        /// True if the delta between <see cref="a"/> and <see cref="b"/> is less than
        /// <see cref="MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE"/>.
        /// </returns>
        /// <remarks>
        /// Resolves infinite and NaN comparisons as false with improved performance vs <see cref="IsApproximatelySafe"/>.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsApproximately(this float a, float b)
        {
            return math.abs(a - b) < MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE;
        }

        /// <summary>
        /// Checks if two <see cref="float2"/> values are approximately equal according to the
        /// <see cref="MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE"/>.
        /// </summary>
        /// <param name="a">The <see cref="float2"/> to compare.</param>
        /// <param name="b">The <see cref="float2"/> to compare.</param>
        /// <returns>
        /// True (component-wise) if the delta between <see cref="a"/> and <see cref="b"/> is less than
        /// <see cref="MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE"/>.
        /// </returns>
        /// <remarks>
        /// Resolves infinite and NaN comparisons as false with improved performance vs <see cref="IsApproximatelySafe"/>.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2 IsApproximately(this float2 a, float2 b)
        {
            return math.abs(a - b) < MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE;
        }

        /// <summary>
        /// Checks if two <see cref="float3"/> values are approximately equal according to the
        /// <see cref="MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE"/>.
        /// </summary>
        /// <param name="a">The <see cref="float3"/> to compare.</param>
        /// <param name="b">The <see cref="float3"/> to compare.</param>
        /// <returns>
        /// True (component-wise) if the delta between <see cref="a"/> and <see cref="b"/> is less than
        /// <see cref="MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE"/>.
        /// </returns>
        /// <remarks>
        /// Resolves infinite and NaN comparisons as false with improved performance vs <see cref="IsApproximatelySafe"/>.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool3 IsApproximately(this float3 a, float3 b)
        {
            return math.abs(a - b) < MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE;
        }

        /// <summary>
        /// Checks if two <see cref="float4"/> values are approximately equal according to the
        /// <see cref="MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE"/>.
        /// </summary>
        /// <param name="a">The <see cref="float4"/> to compare.</param>
        /// <param name="b">The <see cref="float4"/> to compare.</param>
        /// <returns>
        /// True (component-wise) if the delta between <see cref="a"/> and <see cref="b"/> is less than
        /// <see cref="MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE"/>.
        /// </returns>
        /// <remarks>
        /// Resolves infinite and NaN comparisons as false with improved performance vs <see cref="IsApproximatelySafe"/>.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool4 IsApproximately(this float4 a, float4 b)
        {
            return math.abs(a - b) < MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE;
        }

        /// <summary>
        /// Checks if two <see cref="float3x3"/> values are approximately equal according to the
        /// <see cref="MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE"/>.
        /// </summary>
        /// <param name="a">The <see cref="float3x3"/> to compare.</param>
        /// <param name="b">The <see cref="float3x3"/> to compare.</param>
        /// <returns>
        /// True (component-wise) if the delta between <see cref="a"/> and <see cref="b"/> is less than
        /// <see cref="MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE"/>.
        /// </returns>
        /// <remarks>
        /// Resolves infinite and NaN comparisons as false with improved performance vs <see cref="IsApproximatelySafe"/>.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool3x3 IsApproximately(this float3x3 a, float3x3 b)
        {
            return new bool3x3(
                a.c0.IsApproximately(b.c0),
                a.c1.IsApproximately(b.c1),
                a.c2.IsApproximately(b.c2)
            );
        }

        /// <summary>
        /// Checks if two <see cref="float4x4"/> values are approximately equal according to the
        /// <see cref="MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE"/>.
        /// </summary>
        /// <param name="a">The <see cref="float4x4"/> to compare.</param>
        /// <param name="b">The <see cref="float4x4"/> to compare.</param>
        /// <returns>
        /// True (component-wise) if the delta between <see cref="a"/> and <see cref="b"/> is less than
        /// <see cref="MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE"/>.
        /// </returns>
        /// <remarks>
        /// Resolves infinite and NaN comparisons as false with improved performance vs <see cref="IsApproximatelySafe"/>.
        /// </remarks>
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
        /// Checks if two <see cref="float"/> values are approximately equal according to the
        /// <see cref="MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE"/>.
        /// </summary>
        /// <param name="a">The <see cref="float"/> to compare.</param>
        /// <param name="b">The <see cref="float"/> to compare.</param>
        /// <returns>
        /// True if the delta between <see cref="a"/> and <see cref="b"/> is less than
        /// <see cref="MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE"/>.
        /// </returns>
        /// <remarks>
        /// Resolves infinite and NaN comparisons as true with the same logic as <see cref="IsEqualOrNaN"/> at
        /// additional cost vs <see cref="IsApproximately"/>.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsApproximatelySafe(this float a, float b)
        {
            return (math.abs(a - b) < MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE)
                   | (math.isnan(a) & math.isnan(b))
                   | (math.isinf(a) & math.isinf(b) & a == b);
        }

        /// <summary>
        /// Checks if two <see cref="float2"/> values are approximately equal according to the
        /// <see cref="MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE"/>.
        /// </summary>
        /// <param name="a">The <see cref="float2"/> to compare.</param>
        /// <param name="b">The <see cref="float2"/> to compare.</param>
        /// <returns>
        /// True (component-wise) if the delta between <see cref="a"/> and <see cref="b"/> is less than
        /// <see cref="MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE"/>.
        /// </returns>
        /// <remarks>
        /// Resolves infinite and NaN comparisons as true with the same logic as <see cref="IsEqualOrNaN"/> at
        /// additional cost vs <see cref="IsApproximately"/>.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2 IsApproximatelySafe(this float2 a, float2 b)
        {
            return (math.abs(a - b) < MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE)
                   | (math.isnan(a) & math.isnan(b))
                   | (math.isinf(a) & math.isinf(b) & a == b);
        }

        /// <summary>
        /// Checks if two <see cref="float3"/> values are approximately equal according to the
        /// <see cref="MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE"/>.
        /// </summary>
        /// <param name="a">The <see cref="float3"/> to compare.</param>
        /// <param name="b">The <see cref="float3"/> to compare.</param>
        /// <returns>
        /// True (component-wise) if the delta between <see cref="a"/> and <see cref="b"/> is less than
        /// <see cref="MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE"/>.
        /// </returns>
        /// <remarks>
        /// Resolves infinite and NaN comparisons as true with the same logic as <see cref="IsEqualOrNaN"/> at
        /// additional cost vs <see cref="IsApproximately"/>.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool3 IsApproximatelySafe(this float3 a, float3 b)
        {
            return (math.abs(a - b) < MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE)
                   | (math.isnan(a) & math.isnan(b))
                   | (math.isinf(a) & math.isinf(b)  & a == b);
        }

        /// <summary>
        /// Checks if two <see cref="float4"/> values are approximately equal according to the
        /// <see cref="MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE"/>.
        /// </summary>
        /// <param name="a">The <see cref="float4"/> to compare.</param>
        /// <param name="b">The <see cref="float4"/> to compare.</param>
        /// <returns>
        /// True (component-wise) if the delta between <see cref="a"/> and <see cref="b"/> is less than
        /// <see cref="MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE"/>.
        /// </returns>
        /// <remarks>
        /// Resolves infinite and NaN comparisons as true with the same logic as <see cref="IsEqualOrNaN"/> at
        /// additional cost vs <see cref="IsApproximately"/>.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool4 IsApproximatelySafe(this float4 a, float4 b)
        {
            return math.abs(a - b) < MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE
                   | (math.isnan(a) & math.isnan(b))
                   | (math.isinf(a) & math.isinf(b) & a == b);
        }

        /// <summary>
        /// Checks if two <see cref="float3x3"/> values are approximately equal according to the
        /// <see cref="MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE"/>.
        /// </summary>
        /// <param name="a">The <see cref="float3x3"/> to compare.</param>
        /// <param name="b">The <see cref="float3x3"/> to compare.</param>
        /// <returns>
        /// True (component-wise) if the delta between <see cref="a"/> and <see cref="b"/> is less than
        /// <see cref="MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE"/>.
        /// </returns>
        /// <remarks>
        /// Resolves infinite and NaN comparisons as true with the same logic as <see cref="IsEqualOrNaN"/> at
        /// additional cost vs <see cref="IsApproximately"/>.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool3x3 IsApproximatelySafe(this float3x3 a, float3x3 b)
        {
            return new bool3x3(
                a.c0.IsApproximatelySafe(b.c0),
                a.c1.IsApproximatelySafe(b.c1),
                a.c2.IsApproximatelySafe(b.c2)
            );
        }

        /// <summary>
        /// Checks if two <see cref="float4x4"/> values are approximately equal according to the
        /// <see cref="MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE"/>.
        /// </summary>
        /// <param name="a">The <see cref="float4x4"/> to compare.</param>
        /// <param name="b">The <see cref="float4x4"/> to compare.</param>
        /// <returns>
        /// True (component-wise) if the delta between <see cref="a"/> and <see cref="b"/> is less than
        /// <see cref="MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE"/>.
        /// </returns>
        /// <remarks>
        /// Resolves infinite and NaN comparisons as true with the same logic as <see cref="IsEqualOrNaN"/> at
        /// additional cost vs <see cref="IsApproximately"/>.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool4x4 IsApproximatelySafe(this float4x4 a, float4x4 b)
        {
            return new bool4x4(
                a.c0.IsApproximatelySafe(b.c0),
                a.c1.IsApproximatelySafe(b.c1),
                a.c2.IsApproximatelySafe(b.c2),
                a.c3.IsApproximatelySafe(b.c3)
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

        /// <summary>
        /// Compares two <see cref="float"/>s and returns true if the components are equal.
        /// Unlike <see cref="float.Equals"/> this method considers <see cref="float.NaN"/> == <see cref="float.NaN"/>
        /// to be true.
        /// </summary>
        /// <param name="a">The <see cref="float"/> to compare.</param>
        /// <param name="b">The <see cref="float"/> to compare.</param>
        /// <returns>True if the values are equal or both <see cref="float.NaN"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEqualOrNaN(this float a, float b)
        {
            return a == b | (math.isnan(a) & math.isnan(b));
        }

        /// <summary>
        /// Compares two <see cref="float2"/>s and returns true if the components are equal.
        /// Unlike <see cref="float2.Equals"/> this method considers <see cref="float.NaN"/> == <see cref="float.NaN"/>
        /// to be true.
        /// </summary>
        /// <param name="a">The <see cref="float2"/> to compare.</param>
        /// <param name="b">The <see cref="float2"/> to compare.</param>
        /// <returns>True (component-wise) if the values are equal or both <see cref="float.NaN"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2 IsEqualOrNaN(this float2 a, float2 b)
        {
            return a == b | (math.isnan(a) & math.isnan(b));
        }

        /// <summary>
        /// Compares two <see cref="float3"/>s and returns true if the components are equal.
        /// Unlike <see cref="float3.Equals"/> this method considers <see cref="float.NaN"/> == <see cref="float.NaN"/>
        /// to be true.
        /// </summary>
        /// <param name="a">The <see cref="float3"/> to compare.</param>
        /// <param name="b">The <see cref="float3"/> to compare.</param>
        /// <returns>True (component-wise) if the values are equal or both <see cref="float.NaN"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool3 IsEqualOrNaN(this float3 a, float3 b)
        {
            return a == b | (math.isnan(a) & math.isnan(b));
        }

        /// <summary>
        /// Compares two <see cref="float4"/>s and returns true if the components are equal.
        /// Unlike <see cref="float4.Equals"/> this method considers <see cref="float.NaN"/> == <see cref="float.NaN"/>
        /// to be true.
        /// </summary>
        /// <param name="a">The <see cref="float4"/> to compare.</param>
        /// <param name="b">The <see cref="float4"/> to compare.</param>
        /// <returns>True (component-wise) if the values are equal or both <see cref="float.NaN"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool4 IsEqualOrNaN(this float4 a, float4 b)
        {
            return a == b | (math.isnan(a) & math.isnan(b));
        }

        /// <summary>
        /// Compares two <see cref="float3x3"/>s and returns true if the components are equal.
        /// Unlike <see cref="float3x3.Equals"/> this method considers <see cref="float.NaN"/> == <see cref="float.NaN"/>
        /// to be true.
        /// </summary>
        /// <param name="a">The <see cref="float3x3"/> to compare.</param>
        /// <param name="b">The <see cref="float3x3"/> to compare.</param>
        /// <returns>True (component-wise) if the values are equal or both <see cref="float.NaN"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool3x3 IsEqualOrNaN(this float3x3 a, float3x3 b)
        {
            return a == b
                   | new bool3x3(
                       math.isnan(a.c0) & math.isnan(b.c0),
                       math.isnan(a.c1) & math.isnan(b.c1),
                       math.isnan(a.c2) & math.isnan(b.c2));
        }

        /// <summary>
        /// Compares two <see cref="float4x4"/>s and returns true if the components are equal.
        /// Unlike <see cref="float4x4.Equals"/> this method considers <see cref="float.NaN"/> == <see cref="float.NaN"/>
        /// to be true.
        /// </summary>
        /// <param name="a">The <see cref="float4x4"/> to compare.</param>
        /// <param name="b">The <see cref="float4x4"/> to compare.</param>
        /// <returns>True (component-wise) if the values are equal or both <see cref="float.NaN"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool4x4 IsEqualOrNaN(this float4x4 a, float4x4 b)
        {
            return a == b
                   | new bool4x4(
                       math.isnan(a.c0) & math.isnan(b.c0),
                       math.isnan(a.c1) & math.isnan(b.c1),
                       math.isnan(a.c2) & math.isnan(b.c2),
                       math.isnan(a.c3) & math.isnan(b.c3));
        }
    }
}
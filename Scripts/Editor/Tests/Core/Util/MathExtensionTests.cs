using Anvil.CSharp.Mathematics;
using Anvil.Unity.Core;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;

namespace Anvil.Unity.Tests
{
    public static class MathExtensionTests
    {
        private static int EqualityWithNaN(float3 a, float3 b)
        {
            return math.all(a.IsEqualOrNaN(b)) ? 0 : 1;
        }

        // ----- GetInverse ----- //
        [Test]
        public static void GetInverseTest()
        {
            Assert.That(nameof(GetInverseTest), Does.StartWith(nameof(MathExtension.GetInverse)));

            Assert.That(new float3(1f).GetInverse(), Is.EqualTo(new float3(1f)));
            Assert.That(new float3(2f).GetInverse(), Is.EqualTo(new float3(0.5f)));

            Assert.That(new float3(-1f).GetInverse(), Is.EqualTo(new float3(-1f)));
            Assert.That(new float3(-2f).GetInverse(), Is.EqualTo(new float3(-0.5f)));

            Assert.That(new float3(7f, -2f, 0f).GetInverse(), Is.EqualTo(new float3(1f/7f, -0.5f, float.PositiveInfinity)));

            Assert.That(float3.zero.GetInverse(), Is.EqualTo(new float3(float.PositiveInfinity)));
            Assert.That(new float3(float.PositiveInfinity, float.NegativeInfinity, float.NaN).GetInverse(), Is.EqualTo(new float3(0, 0, float.NaN)).Using<float3>(EqualityWithNaN));
        }

        // ----- GetInverseSafe ----- //
        [Test]
        public static void GetInverseSafeTest()
        {
            Assert.That(nameof(GetInverseSafeTest), Does.StartWith(nameof(MathExtension.GetInverseSafe)));

            Assert.That(new float3(1f).GetInverseSafe(), Is.EqualTo(new float3(1f)));
            Assert.That(new float3(2f).GetInverseSafe(), Is.EqualTo(new float3(0.5f)));

            Assert.That(new float3(-1f).GetInverseSafe(), Is.EqualTo(new float3(-1f)));
            Assert.That(new float3(-2f).GetInverseSafe(), Is.EqualTo(new float3(-0.5f)));

            Assert.That(new float3(7f, -2f, 0f).GetInverseSafe(), Is.EqualTo(new float3(1f/7f, -0.5f, 0f)));

            Assert.That(float3.zero.GetInverseSafe(), Is.EqualTo(float3.zero));
            Assert.That(new float3(float.PositiveInfinity, float.NegativeInfinity, float.NaN).GetInverseSafe(), Is.EqualTo(new float3(0, 0, 0f)));
        }

        // ----- IsApproximately ----- //
        [Test]
        public static void IsApproximatelyTest_float()
        {
            Assert.That(nameof(IsApproximatelyTest_float), Does.StartWith(nameof(MathExtension.IsApproximately)));

            float one = 1f;
            float infinity = float.PositiveInfinity;
            float negativeInfinity = float.NegativeInfinity;
            float naN = float.NaN;

            Assert.That(one.IsApproximately(one), Is.True);
            Assert.That((-one).IsApproximately(-one), Is.True);

            Assert.That(one.IsApproximately(one - float.Epsilon), Is.True);
            Assert.That(one.IsApproximately(one + float.Epsilon), Is.True);

            Assert.That(one.IsApproximately(one - (float.Epsilon+MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE)), Is.False);
            Assert.That(one.IsApproximately(one + (float.Epsilon+MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE)), Is.False);

            Assert.That(0f.IsApproximately(0f), Is.True);

            Assert.That(infinity.IsApproximately(infinity), Is.False);
            Assert.That(infinity.IsApproximately(one), Is.False);
            Assert.That(infinity.IsApproximately(negativeInfinity), Is.EqualTo(false));

            Assert.That(negativeInfinity.IsApproximately(negativeInfinity), Is.False);
            Assert.That(negativeInfinity.IsApproximately(one), Is.False);

            Assert.That(naN.IsApproximately(naN), Is.False);
            Assert.That(naN.IsApproximately(one), Is.False);
        }

        [Test]
        public static void IsApproximatelyTest_float2()
        {
            Assert.That(nameof(IsApproximatelyTest_float2), Does.StartWith(nameof(MathExtension.IsApproximately)));

            float2 one = new float2(1f);
            float2 infinity_negativeInfinity = new float2(float.PositiveInfinity, float.NegativeInfinity);
            float2 naN = new float2(float.NaN);
            float2 five_naN = new float2(5f, float.NaN);

            Assert.That(one.IsApproximately(one), Is.EqualTo(new bool2(true)));
            Assert.That((-one).IsApproximately(-one), Is.EqualTo(new bool2(true)));

            Assert.That(new float2(-1f, 1f).IsApproximately(one), Is.EqualTo(new bool2(false, true)));

            Assert.That(one.IsApproximately(one - new float2(float.Epsilon)), Is.EqualTo(new bool2(true)));
            Assert.That(one.IsApproximately(one + new float2(float.Epsilon)), Is.EqualTo(new bool2(true)));

            Assert.That(one.IsApproximately(one - new float2(float.Epsilon+MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE)), Is.EqualTo(new bool2(false)));
            Assert.That(one.IsApproximately(one + new float2(float.Epsilon+MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE)), Is.EqualTo(new bool2(false)));

            Assert.That(float2.zero.IsApproximately(float2.zero), Is.EqualTo(new bool2(true)));

            Assert.That(infinity_negativeInfinity.IsApproximately(infinity_negativeInfinity), Is.EqualTo(new bool2(false)));
            Assert.That(infinity_negativeInfinity.IsApproximately(one), Is.EqualTo(new bool2(false)));
            Assert.That(infinity_negativeInfinity.IsApproximately(-infinity_negativeInfinity), Is.EqualTo(new bool2(false)));

            Assert.That(naN.IsApproximately(naN), Is.EqualTo(new bool2(false)));
            Assert.That(naN.IsApproximately(one), Is.EqualTo(new bool2(false)));

            Assert.That(five_naN.IsApproximately(five_naN), Is.EqualTo(new bool2(true, false)));
        }

        [Test]
        public static void IsApproximatelyTest_float3()
        {
            Assert.That(nameof(IsApproximatelyTest_float3), Does.StartWith(nameof(MathExtension.IsApproximately)));

            float3 one = new float3(1f);
            float3 infinity_negativeInfinity_NaN = new float3(float.PositiveInfinity, float.NegativeInfinity, float.NaN);
            float3 infinity_NaN_three = new float3(float.PositiveInfinity, float.NaN, 3f);

            Assert.That(one.IsApproximately(one), Is.EqualTo(new bool3(true)));
            Assert.That((-one).IsApproximately(-one), Is.EqualTo(new bool3(true)));

            Assert.That(new float3(-1f, 1f, -1f).IsApproximately(one), Is.EqualTo(new bool3(false, true, false)));

            Assert.That(one.IsApproximately(one - new float3(float.Epsilon)), Is.EqualTo(new bool3(true)));
            Assert.That(one.IsApproximately(one + new float3(float.Epsilon)), Is.EqualTo(new bool3(true)));

            Assert.That(one.IsApproximately(one - new float3(float.Epsilon+MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE)), Is.EqualTo(new bool3(false)));
            Assert.That(one.IsApproximately(one + new float3(float.Epsilon+MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE)), Is.EqualTo(new bool3(false)));

            Assert.That(float3.zero.IsApproximately(float3.zero), Is.EqualTo(new bool3(true)));

            Assert.That(infinity_negativeInfinity_NaN.IsApproximately(infinity_negativeInfinity_NaN), Is.EqualTo(new bool3(false)));
            Assert.That(infinity_negativeInfinity_NaN.IsApproximately(one), Is.EqualTo(new bool3(false)));
            Assert.That(infinity_negativeInfinity_NaN.IsApproximately(-infinity_negativeInfinity_NaN), Is.EqualTo(new bool3(false)));

            Assert.That(infinity_NaN_three.IsApproximately(infinity_NaN_three), Is.EqualTo(new bool3(false, false, true)));
        }

        [Test]
        public static void IsApproximatelyTest_float4()
        {
            Assert.That(nameof(IsApproximatelyTest_float4), Does.StartWith(nameof(MathExtension.IsApproximately)));

            float4 one = new float4(1f);
            float4 infinity_negativeInfinity_NaN = new float4(float.PositiveInfinity, float.NegativeInfinity, float.NaN, float.NaN);

            Assert.That(one.IsApproximately(one), Is.EqualTo(new bool4(true)));
            Assert.That((-one).IsApproximately(-one), Is.EqualTo(new bool4(true)));

            Assert.That(new float4(-1f, 1f, -1f, 1f).IsApproximately(one), Is.EqualTo(new bool4(false, true, false, true)));

            Assert.That(one.IsApproximately(one - new float4(float.Epsilon)), Is.EqualTo(new bool4(true)));
            Assert.That(one.IsApproximately(one + new float4(float.Epsilon)), Is.EqualTo(new bool4(true)));

            Assert.That(one.IsApproximately(one - new float4(float.Epsilon+MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE)), Is.EqualTo(new bool4(false)));
            Assert.That(one.IsApproximately(one + new float4(float.Epsilon+MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE)), Is.EqualTo(new bool4(false)));

            Assert.That(float4.zero.IsApproximately(float4.zero), Is.EqualTo(new bool4(true)));

            Assert.That(infinity_negativeInfinity_NaN.IsApproximately(infinity_negativeInfinity_NaN), Is.EqualTo(new bool4(false)));
            Assert.That(infinity_negativeInfinity_NaN.IsApproximately(one), Is.EqualTo(new bool4(false)));
            Assert.That(infinity_negativeInfinity_NaN.IsApproximately(-infinity_negativeInfinity_NaN), Is.EqualTo(new bool4(false)));
        }

        [Test]
        public static void IsApproximatelyTest_float3x3()
        {
            Assert.That(nameof(IsApproximatelyTest_float3x3), Does.StartWith(nameof(MathExtension.IsApproximately)));

            float3x3 one = new float3x3(1f);
            float3x3 infinity_negativeInfinity_NaN_column = new float3x3(float.PositiveInfinity, float.NegativeInfinity, float.NaN);
            float3x3 infinity_negativeInfinity_NaN_row = new float3x3(
                new float3(float.PositiveInfinity, float.NegativeInfinity, float.NaN),
                new float3(float.PositiveInfinity, float.NegativeInfinity, float.NaN),
                new float3(float.PositiveInfinity, float.NegativeInfinity, float.NaN)
                );

            Assert.That(one.IsApproximately(one), Is.EqualTo(new bool3x3(true)));
            Assert.That((-one).IsApproximately(-one), Is.EqualTo(new bool3x3(true)));

            Assert.That(new float3x3(-1f, 1f, -1f).IsApproximately(one), Is.EqualTo(new bool3x3(false, true, false)));

            Assert.That(one.IsApproximately(one - new float3x3(float.Epsilon)), Is.EqualTo(new bool3x3(true)));
            Assert.That(one.IsApproximately(one + new float3x3(float.Epsilon)), Is.EqualTo(new bool3x3(true)));

            Assert.That(one.IsApproximately(one - new float3x3(float.Epsilon+MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE)), Is.EqualTo(new bool3x3(false)));
            Assert.That(one.IsApproximately(one + new float3x3(float.Epsilon+MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE)), Is.EqualTo(new bool3x3(false)));

            Assert.That(float3x3.zero.IsApproximately(float3x3.zero), Is.EqualTo(new bool3x3(true)));

            Assert.That(infinity_negativeInfinity_NaN_column.IsApproximately(infinity_negativeInfinity_NaN_column), Is.EqualTo(new bool3x3(false)));
            Assert.That(infinity_negativeInfinity_NaN_column.IsApproximately(one), Is.EqualTo(new bool3x3(false)));
            Assert.That(infinity_negativeInfinity_NaN_column.IsApproximately(-infinity_negativeInfinity_NaN_column), Is.EqualTo(new bool3x3(false)));

            Assert.That(infinity_negativeInfinity_NaN_row.IsApproximately(infinity_negativeInfinity_NaN_row), Is.EqualTo(new bool3x3(false)));
            Assert.That(infinity_negativeInfinity_NaN_row.IsApproximately(one), Is.EqualTo(new bool3x3(false)));
        }

        [Test]
        public static void IsApproximatelyTest_float4x4()
        {
            Assert.That(nameof(IsApproximatelyTest_float4x4), Does.StartWith(nameof(MathExtension.IsApproximately)));

            float4x4 one = new float4x4(1f);
            float4x4 infinity_negativeInfinity_NaN_column = new float4x4(float.PositiveInfinity, float.NegativeInfinity, float.NaN, float.NaN);
            float4x4 infinity_negativeInfinity_NaN_row = new float4x4(
                new float4(float.PositiveInfinity, float.NegativeInfinity, float.NaN, float.NaN),
                new float4(float.PositiveInfinity, float.NegativeInfinity, float.NaN, float.NaN),
                new float4(float.PositiveInfinity, float.NegativeInfinity, float.NaN, float.NaN),
                new float4(float.PositiveInfinity, float.NegativeInfinity, float.NaN, float.NaN)
            );

            Assert.That(one.IsApproximately(one), Is.EqualTo(new bool4x4(true)));
            Assert.That((-one).IsApproximately(-one), Is.EqualTo(new bool4x4(true)));

            Assert.That(new float4x4(-1f, 1f, -1f, 1f).IsApproximately(one), Is.EqualTo(new bool4x4(false, true, false, true)));

            Assert.That(one.IsApproximately(one - new float4x4(float.Epsilon)), Is.EqualTo(new bool4x4(true)));
            Assert.That(one.IsApproximately(one + new float4x4(float.Epsilon)), Is.EqualTo(new bool4x4(true)));

            Assert.That(one.IsApproximately(one - new float4x4(float.Epsilon+MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE)), Is.EqualTo(new bool4x4(false)));
            Assert.That(one.IsApproximately(one + new float4x4(float.Epsilon+MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE)), Is.EqualTo(new bool4x4(false)));

            Assert.That(float4x4.zero.IsApproximately(float4x4.zero), Is.EqualTo(new bool4x4(true)));

            Assert.That(infinity_negativeInfinity_NaN_column.IsApproximately(infinity_negativeInfinity_NaN_column), Is.EqualTo(new bool4x4(false)));
            Assert.That(infinity_negativeInfinity_NaN_column.IsApproximately(one), Is.EqualTo(new bool4x4(false)));
            Assert.That(infinity_negativeInfinity_NaN_column.IsApproximately(-infinity_negativeInfinity_NaN_column), Is.EqualTo(new bool4x4(false)));

            Assert.That(infinity_negativeInfinity_NaN_row.IsApproximately(infinity_negativeInfinity_NaN_row), Is.EqualTo(new bool4x4(false)));
            Assert.That(infinity_negativeInfinity_NaN_row.IsApproximately(one), Is.EqualTo(new bool4x4(false)));
        }

        // ----- IsApproximatelySafe ----- //
        [Test]
        public static void IsApproximatelySafeTest_float()
        {
            Assert.That(nameof(IsApproximatelySafeTest_float), Does.StartWith(nameof(MathExtension.IsApproximatelySafe)));

            float one = 1f;
            float infinity = float.PositiveInfinity;
            float negativeInfinity = float.NegativeInfinity;
            float naN = float.NaN;

            Assert.That(one.IsApproximatelySafe(one), Is.True);
            Assert.That((-one).IsApproximatelySafe(-one), Is.True);

            Assert.That(one.IsApproximatelySafe(one - float.Epsilon), Is.True);
            Assert.That(one.IsApproximatelySafe(one + float.Epsilon), Is.True);

            Assert.That(one.IsApproximatelySafe(one - (float.Epsilon+MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE)), Is.False);
            Assert.That(one.IsApproximatelySafe(one + (float.Epsilon+MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE)), Is.False);

            Assert.That(0f.IsApproximatelySafe(0f), Is.True);

            Assert.That(infinity.IsApproximatelySafe(infinity), Is.True);
            Assert.That(infinity.IsApproximatelySafe(one), Is.False);
            Assert.That(negativeInfinity.IsApproximatelySafe(negativeInfinity), Is.True);
            Assert.That(negativeInfinity.IsApproximatelySafe(one), Is.False);
            Assert.That(infinity.IsApproximatelySafe(negativeInfinity), Is.False);

            Assert.That(naN.IsApproximatelySafe(naN), Is.True);
            Assert.That(naN.IsApproximatelySafe(one), Is.False);
        }

        [Test]
        public static void IsApproximatelySafeTest_float2()
        {
            Assert.That(nameof(IsApproximatelySafeTest_float2), Does.StartWith(nameof(MathExtension.IsApproximatelySafe)));

            float2 one = new float2(1f);
            float2 infinity_negativeInfinity = new float2(float.PositiveInfinity, float.NegativeInfinity);
            float2 naN = new float2(float.NaN);
            float2 five_naN = new float2(5f, float.NaN);

            Assert.That(one.IsApproximatelySafe(one), Is.EqualTo(new bool2(true)));
            Assert.That((-one).IsApproximatelySafe(-one), Is.EqualTo(new bool2(true)));

            Assert.That(new float2(-1f, 1f).IsApproximatelySafe(one), Is.EqualTo(new bool2(false, true)));

            Assert.That(one.IsApproximatelySafe(one - new float2(float.Epsilon)), Is.EqualTo(new bool2(true)));
            Assert.That(one.IsApproximatelySafe(one + new float2(float.Epsilon)), Is.EqualTo(new bool2(true)));

            Assert.That(one.IsApproximatelySafe(one - new float2(float.Epsilon+MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE)), Is.EqualTo(new bool2(false)));
            Assert.That(one.IsApproximatelySafe(one + new float2(float.Epsilon+MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE)), Is.EqualTo(new bool2(false)));

            Assert.That(float2.zero.IsApproximatelySafe(float2.zero), Is.EqualTo(new bool2(true)));

            Assert.That(infinity_negativeInfinity.IsApproximatelySafe(infinity_negativeInfinity), Is.EqualTo(new bool2(true)));
            Assert.That(infinity_negativeInfinity.IsApproximatelySafe(one), Is.EqualTo(new bool2(false)));
            Assert.That(infinity_negativeInfinity.IsApproximatelySafe(-infinity_negativeInfinity), Is.EqualTo(new bool2(false)));

            Assert.That(naN.IsApproximatelySafe(naN), Is.EqualTo(new bool2(true)));
            Assert.That(naN.IsApproximatelySafe(one), Is.EqualTo(new bool2(false)));

            Assert.That(five_naN.IsApproximatelySafe(five_naN), Is.EqualTo(new bool2(true)));
        }

        [Test]
        public static void IsApproximatelySafeTest_float3()
        {
            Assert.That(nameof(IsApproximatelySafeTest_float3), Does.StartWith(nameof(MathExtension.IsApproximatelySafe)));

            float3 one = new float3(1f);
            float3 infinity_negativeInfinity_NaN = new float3(float.PositiveInfinity, float.NegativeInfinity, float.NaN);
            float3 infinity_NaN_three = new float3(float.PositiveInfinity, float.NaN, 3f);

            Assert.That(one.IsApproximatelySafe(one), Is.EqualTo(new bool3(true)));
            Assert.That((-one).IsApproximatelySafe(-one), Is.EqualTo(new bool3(true)));

            Assert.That(new float3(-1f, 1f, -1f).IsApproximatelySafe(one), Is.EqualTo(new bool3(false, true, false)));

            Assert.That(one.IsApproximatelySafe(one - new float3(float.Epsilon)), Is.EqualTo(new bool3(true)));
            Assert.That(one.IsApproximatelySafe(one + new float3(float.Epsilon)), Is.EqualTo(new bool3(true)));

            Assert.That(one.IsApproximatelySafe(one - new float3(float.Epsilon + MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE)), Is.EqualTo(new bool3(false)));
            Assert.That(one.IsApproximatelySafe(one + new float3(float.Epsilon + MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE)), Is.EqualTo(new bool3(false)));

            Assert.That(float3.zero.IsApproximatelySafe(float3.zero), Is.EqualTo(new bool3(true)));

            Assert.That(infinity_negativeInfinity_NaN.IsApproximatelySafe(infinity_negativeInfinity_NaN), Is.EqualTo(new bool3(true)));
            Assert.That(infinity_negativeInfinity_NaN.IsApproximatelySafe(one), Is.EqualTo(new bool3(false)));
            Assert.That(infinity_negativeInfinity_NaN.IsApproximatelySafe(-infinity_negativeInfinity_NaN), Is.EqualTo(new bool3(false, false, true)));

            Assert.That(infinity_NaN_three.IsApproximatelySafe(infinity_NaN_three), Is.EqualTo(new bool3(true)));
        }

        [Test]
        public static void IsApproximatelySafeTest_float4()
        {
            Assert.That(nameof(IsApproximatelySafeTest_float4), Does.StartWith(nameof(MathExtension.IsApproximatelySafe)));

            float4 one = new float4(1f);
            float4 infinity_negativeInfinity_NaN_three = new float4(float.PositiveInfinity, float.NegativeInfinity, float.NaN, 3f);

            Assert.That(one.IsApproximatelySafe(one), Is.EqualTo(new bool4(true)));
            Assert.That((-one).IsApproximatelySafe(-one), Is.EqualTo(new bool4(true)));

            Assert.That(new float4(-1f, 1f, -1f, 1f).IsApproximatelySafe(one), Is.EqualTo(new bool4(false, true, false, true)));

            Assert.That(one.IsApproximatelySafe(one - new float4(float.Epsilon)), Is.EqualTo(new bool4(true)));
            Assert.That(one.IsApproximatelySafe(one + new float4(float.Epsilon)), Is.EqualTo(new bool4(true)));

            Assert.That(one.IsApproximatelySafe(one - new float4(float.Epsilon+MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE)), Is.EqualTo(new bool4(false)));
            Assert.That(one.IsApproximatelySafe(one + new float4(float.Epsilon+MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE)), Is.EqualTo(new bool4(false)));

            Assert.That(float4.zero.IsApproximatelySafe(float4.zero), Is.EqualTo(new bool4(true)));

            Assert.That(infinity_negativeInfinity_NaN_three.IsApproximatelySafe(infinity_negativeInfinity_NaN_three), Is.EqualTo(new bool4(true)));
            Assert.That(infinity_negativeInfinity_NaN_three.IsApproximatelySafe(one), Is.EqualTo(new bool4(false)));
            Assert.That(infinity_negativeInfinity_NaN_three.IsApproximatelySafe(-infinity_negativeInfinity_NaN_three), Is.EqualTo(new bool4(false, false, true, false)));
        }

        [Test]
        public static void IsApproximatelySafeTest_float3x3()
        {
            Assert.That(nameof(IsApproximatelySafeTest_float3x3), Does.StartWith(nameof(MathExtension.IsApproximatelySafe)));

            float3x3 one = new float3x3(1f);
            float3x3 infinity_negativeInfinity_NaN_column = new float3x3(float.PositiveInfinity, float.NegativeInfinity, float.NaN);
            float3x3 infinity_negativeInfinity_NaN_row = new float3x3(
                new float3(float.PositiveInfinity, float.NegativeInfinity, float.NaN),
                new float3(float.PositiveInfinity, float.NegativeInfinity, float.NaN),
                new float3(float.PositiveInfinity, float.NegativeInfinity, float.NaN)
                );

            Assert.That(one.IsApproximatelySafe(one), Is.EqualTo(new bool3x3(true)));
            Assert.That((-one).IsApproximatelySafe(-one), Is.EqualTo(new bool3x3(true)));

            Assert.That(new float3x3(-1f, 1f, -1f).IsApproximatelySafe(one), Is.EqualTo(new bool3x3(false, true, false)));

            Assert.That(one.IsApproximatelySafe(one - new float3x3(float.Epsilon)), Is.EqualTo(new bool3x3(true)));
            Assert.That(one.IsApproximatelySafe(one + new float3x3(float.Epsilon)), Is.EqualTo(new bool3x3(true)));

            Assert.That(one.IsApproximatelySafe(one - new float3x3(float.Epsilon+MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE)), Is.EqualTo(new bool3x3(false)));
            Assert.That(one.IsApproximatelySafe(one + new float3x3(float.Epsilon+MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE)), Is.EqualTo(new bool3x3(false)));

            Assert.That(float3x3.zero.IsApproximatelySafe(float3x3.zero), Is.EqualTo(new bool3x3(true)));

            Assert.That(infinity_negativeInfinity_NaN_column.IsApproximatelySafe(infinity_negativeInfinity_NaN_column), Is.EqualTo(new bool3x3(true)));
            Assert.That(infinity_negativeInfinity_NaN_column.IsApproximatelySafe(one), Is.EqualTo(new bool3x3(false)));
            Assert.That(infinity_negativeInfinity_NaN_column.IsApproximatelySafe(-infinity_negativeInfinity_NaN_column), Is.EqualTo(new bool3x3(false, false, true)));

            Assert.That(infinity_negativeInfinity_NaN_row.IsApproximatelySafe(infinity_negativeInfinity_NaN_row), Is.EqualTo(new bool3x3(true)));
            Assert.That(infinity_negativeInfinity_NaN_row.IsApproximatelySafe(one), Is.EqualTo(new bool3x3(false)));
        }

        [Test]
        public static void IsApproximatelySafeTest_float4x4()
        {
            Assert.That(nameof(IsApproximatelySafeTest_float4x4), Does.StartWith(nameof(MathExtension.IsApproximatelySafe)));

            float4x4 one = new float4x4(1f);
            float4x4 infinity_negativeInfinity_NaN_three_column = new float4x4(float.PositiveInfinity, float.NegativeInfinity, float.NaN, 3f);
            float4x4 infinity_negativeInfinity_NaN_three_row = new float4x4(
                new float4(float.PositiveInfinity, float.NegativeInfinity, float.NaN, 3f),
                new float4(float.PositiveInfinity, float.NegativeInfinity, float.NaN, 3f),
                new float4(float.PositiveInfinity, float.NegativeInfinity, float.NaN, 3f),
                new float4(float.PositiveInfinity, float.NegativeInfinity, float.NaN, 3f)
            );

            Assert.That(one.IsApproximatelySafe(one), Is.EqualTo(new bool4x4(true)));
            Assert.That((-one).IsApproximatelySafe(-one), Is.EqualTo(new bool4x4(true)));

            Assert.That(new float4x4(-1f, 1f, -1f, 1f).IsApproximatelySafe(one), Is.EqualTo(new bool4x4(false, true, false, true)));

            Assert.That(one.IsApproximatelySafe(one - new float4x4(float.Epsilon)), Is.EqualTo(new bool4x4(true)));
            Assert.That(one.IsApproximatelySafe(one + new float4x4(float.Epsilon)), Is.EqualTo(new bool4x4(true)));

            Assert.That(one.IsApproximatelySafe(one - new float4x4(float.Epsilon+MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE)), Is.EqualTo(new bool4x4(false)));
            Assert.That(one.IsApproximatelySafe(one + new float4x4(float.Epsilon+MathUtil.FLOATING_POINT_EQUALITY_TOLERANCE)), Is.EqualTo(new bool4x4(false)));

            Assert.That(float4x4.zero.IsApproximatelySafe(float4x4.zero), Is.EqualTo(new bool4x4(true)));

            Assert.That(infinity_negativeInfinity_NaN_three_column.IsApproximatelySafe(infinity_negativeInfinity_NaN_three_column), Is.EqualTo(new bool4x4(true)));
            Assert.That(infinity_negativeInfinity_NaN_three_column.IsApproximatelySafe(one), Is.EqualTo(new bool4x4(false)));
            Assert.That(infinity_negativeInfinity_NaN_three_column.IsApproximatelySafe(-infinity_negativeInfinity_NaN_three_column), Is.EqualTo(new bool4x4(false, false, true, false)));

            Assert.That(infinity_negativeInfinity_NaN_three_row.IsApproximatelySafe(infinity_negativeInfinity_NaN_three_row), Is.EqualTo(new bool4x4(true)));
            Assert.That(infinity_negativeInfinity_NaN_three_row.IsApproximatelySafe(one), Is.EqualTo(new bool4x4(false)));
        }

        // ----- ToSignedInfinite ----- //
        [Test]
        public static void ToSignedInfiniteTest()
        {
            Assert.That(nameof(ToSignedInfiniteTest), Does.StartWith(nameof(MathExtension.ToSignedInfinite)));

            float3 zero = float3.zero;
            float3 one = new float3(1f);
            float3 one_negativeOne_zero = new float3(1f, -1, 0);
            float3 nan = new float3(float.NaN);

            float3 infinity = new float3(float.PositiveInfinity);
            float3 negativeInfinity = new float3(float.NegativeInfinity);
            float3 infinity_negativeInfinity_zero = new float3(float.PositiveInfinity, float.NegativeInfinity, 0f);

            Assert.That(zero.ToSignedInfinite(), Is.EqualTo(zero));
            Assert.That(one.ToSignedInfinite(), Is.EqualTo(infinity));
            Assert.That(-one.ToSignedInfinite(), Is.EqualTo(negativeInfinity));
            Assert.That(one_negativeOne_zero.ToSignedInfinite(), Is.EqualTo(infinity_negativeInfinity_zero));

            Assert.That(nan.ToSignedInfinite(), Is.EqualTo(nan).Using<float3>(EqualityWithNaN));

            Assert.That(infinity.ToSignedInfinite(), Is.EqualTo(infinity));
            Assert.That(negativeInfinity.ToSignedInfinite(), Is.EqualTo(negativeInfinity));
            Assert.That(infinity_negativeInfinity_zero.ToSignedInfinite(), Is.EqualTo(infinity_negativeInfinity_zero));
        }

        // ----- IsEqualOrNaN ----- //
        [Test]
        public static void IsEqualOrNaNTest_float()
        {
            Assert.That(nameof(IsEqualOrNaNTest_float), Does.StartWith(nameof(MathExtension.IsEqualOrNaN)));

            float zero = 0f;
            float one = 1f;
            float nan = float.NaN;
            float infinity = float.PositiveInfinity;

            Assert.That(zero.IsEqualOrNaN(zero), Is.True);
            Assert.That(zero.IsEqualOrNaN(one), Is.False);

            Assert.That(one.IsEqualOrNaN(one), Is.True);
            Assert.That(one.IsEqualOrNaN(zero), Is.False);

            Assert.That(one.IsEqualOrNaN(-one), Is.False);
            Assert.That((-one).IsEqualOrNaN(-one), Is.True);

            Assert.That(infinity.IsEqualOrNaN(infinity), Is.True);
            Assert.That(one.IsEqualOrNaN(zero), Is.False);

            Assert.That(nan.IsEqualOrNaN(nan), Is.True);
            Assert.That(nan.IsEqualOrNaN(zero), Is.False);
            Assert.That(nan.IsEqualOrNaN(infinity), Is.False);
        }

        [Test]
        public static void IsEqualOrNaNTest_float2()
        {
            Assert.That(nameof(IsEqualOrNaNTest_float2), Does.StartWith(nameof(MathExtension.IsEqualOrNaN)));

            float2 zero = float2.zero;
            float2 one = new float2(1f);
            float2 one_negativeOne = new float2(1f, -1);
            float2 nan = new float2(float.NaN);
            float2 nan_negativeOne = new float2(float.NaN, -1);
            float2 infinity_negativeInfinity = new float2(float.PositiveInfinity, float.NegativeInfinity);

            Assert.That(zero.IsEqualOrNaN(zero), Is.EqualTo(new bool2(true)));
            Assert.That(zero.IsEqualOrNaN(one), Is.EqualTo(new bool2(false)));

            Assert.That(one.IsEqualOrNaN(one), Is.EqualTo(new bool2(true)));
            Assert.That(one.IsEqualOrNaN(zero), Is.EqualTo(new bool2(false)));

            Assert.That(infinity_negativeInfinity.IsEqualOrNaN(infinity_negativeInfinity), Is.EqualTo(new bool2(true)));
            Assert.That(infinity_negativeInfinity.IsEqualOrNaN(zero), Is.EqualTo(new bool2(false)));

            Assert.That(one_negativeOne.IsEqualOrNaN(one_negativeOne), Is.EqualTo(new bool2(true)));
            Assert.That(one_negativeOne.IsEqualOrNaN(one), Is.EqualTo(new bool2(true, false)));

            Assert.That(nan.IsEqualOrNaN(nan), Is.EqualTo(new bool2(true)));
            Assert.That(nan.IsEqualOrNaN(zero), Is.EqualTo(new bool2(false)));

            Assert.That(nan_negativeOne.IsEqualOrNaN(nan_negativeOne), Is.EqualTo(new bool2(true)));
            Assert.That(nan_negativeOne.IsEqualOrNaN(-nan_negativeOne), Is.EqualTo(new bool2(true, false)));
            Assert.That(nan_negativeOne.IsEqualOrNaN(-one), Is.EqualTo(new bool2(false, true)));
        }

        [Test]
        public static void IsEqualOrNaNTest_float3()
        {
            Assert.That(nameof(IsEqualOrNaNTest_float3), Does.StartWith(nameof(MathExtension.IsEqualOrNaN)));

            float3 zero = float3.zero;
            float3 one = new float3(1f);
            float3 one_negativeOne_zero = new float3(1f, -1, 0);
            float3 nan = new float3(float.NaN);
            float3 infinity_negativeInfinity = new float3(float.PositiveInfinity, float.NegativeInfinity, float.NegativeInfinity);
            float3 nan_negativeOne_zero = new float3(float.NaN, -1, 0);

            Assert.That(zero.IsEqualOrNaN(zero), Is.EqualTo(new bool3(true)));
            Assert.That(zero.IsEqualOrNaN(one), Is.EqualTo(new bool3(false)));

            Assert.That(one.IsEqualOrNaN(one), Is.EqualTo(new bool3(true)));
            Assert.That(one.IsEqualOrNaN(zero), Is.EqualTo(new bool3(false)));

            Assert.That(infinity_negativeInfinity.IsEqualOrNaN(infinity_negativeInfinity), Is.EqualTo(new bool3(true)));
            Assert.That(infinity_negativeInfinity.IsEqualOrNaN(zero), Is.EqualTo(new bool3(false)));

            Assert.That(one_negativeOne_zero.IsEqualOrNaN(one_negativeOne_zero), Is.EqualTo(new bool3(true)));
            Assert.That(one_negativeOne_zero.IsEqualOrNaN(zero), Is.EqualTo(new bool3(false, false, true)));

            Assert.That(nan.IsEqualOrNaN(nan), Is.EqualTo(new bool3(true)));
            Assert.That(nan.IsEqualOrNaN(zero), Is.EqualTo(new bool3(false)));

            Assert.That(nan_negativeOne_zero.IsEqualOrNaN(nan_negativeOne_zero), Is.EqualTo(new bool3(true)));
            Assert.That(nan_negativeOne_zero.IsEqualOrNaN(-nan_negativeOne_zero), Is.EqualTo(new bool3(true, false, true)));
            Assert.That(nan_negativeOne_zero.IsEqualOrNaN(zero), Is.EqualTo(new bool3(false, false, true)));
        }

        [Test]
        public static void IsEqualOrNaNTest_float4()
        {
            Assert.That(nameof(IsEqualOrNaNTest_float4), Does.StartWith(nameof(MathExtension.IsEqualOrNaN)));

            float4 zero = float4.zero;
            float4 one = new float4(1f);
            float4 one_negativeOne_zero = new float4(1f, -1, 0, 0);
            float4 nan = new float4(float.NaN);
            float4 infinity_negativeInfinity = new float4(float.PositiveInfinity, float.PositiveInfinity, float.NegativeInfinity, float.NegativeInfinity);
            float4 nan_negativeOne_zero = new float4(float.NaN, -1, 0, 0);

            Assert.That(zero.IsEqualOrNaN(zero), Is.EqualTo(new bool4(true)));
            Assert.That(zero.IsEqualOrNaN(one), Is.EqualTo(new bool4(false)));

            Assert.That(one.IsEqualOrNaN(one), Is.EqualTo(new bool4(true)));
            Assert.That(one.IsEqualOrNaN(zero), Is.EqualTo(new bool4(false)));

            Assert.That(infinity_negativeInfinity.IsEqualOrNaN(infinity_negativeInfinity), Is.EqualTo(new bool4(true)));
            Assert.That(infinity_negativeInfinity.IsEqualOrNaN(zero), Is.EqualTo(new bool4(false)));

            Assert.That(one_negativeOne_zero.IsEqualOrNaN(one_negativeOne_zero), Is.EqualTo(new bool4(true)));
            Assert.That(one_negativeOne_zero.IsEqualOrNaN(zero), Is.EqualTo(new bool4(false, false, true, true)));

            Assert.That(nan.IsEqualOrNaN(nan), Is.EqualTo(new bool4(true)));
            Assert.That(nan.IsEqualOrNaN(zero), Is.EqualTo(new bool4(false)));

            Assert.That(nan_negativeOne_zero.IsEqualOrNaN(nan_negativeOne_zero), Is.EqualTo(new bool4(true)));
            Assert.That(nan_negativeOne_zero.IsEqualOrNaN(-nan_negativeOne_zero), Is.EqualTo(new bool4(true, false, true, true)));
            Assert.That(nan_negativeOne_zero.IsEqualOrNaN(zero), Is.EqualTo(new bool4(false, false, true, true)));
        }

        [Test]
        public static void IsEqualOrNaNTest_float3x3()
        {
            Assert.That(nameof(IsEqualOrNaNTest_float3x3), Does.StartWith(nameof(MathExtension.IsEqualOrNaN)));

            float3x3 zero = float3x3.zero;
            float3x3 one = new float3x3(1f);
            float3x3 one_negativeOne_zero = new float3x3(1f, -1, 0);
            float3x3 nan = new float3x3(float.NaN);
            float3x3 infinity_negativeInfinity = new float3x3(float.PositiveInfinity, float.NegativeInfinity, float.NegativeInfinity);
            float3x3 nan_negativeOne_zero_columns = new float3x3(float.NaN, -1, 0);
            float3x3 nan_negativeOne_zero_rows = new float3x3(
                new float3(float.NaN, -1, 0),
                new float3(float.NaN, -1, 0),
                new float3(float.NaN, -1, 0));

            Assert.That(zero.IsEqualOrNaN(zero), Is.EqualTo(new bool3x3(true)));
            Assert.That(zero.IsEqualOrNaN(one), Is.EqualTo(new bool3x3(false)));

            Assert.That(one.IsEqualOrNaN(one), Is.EqualTo(new bool3x3(true)));
            Assert.That(one.IsEqualOrNaN(zero), Is.EqualTo(new bool3x3(false)));

            Assert.That(infinity_negativeInfinity.IsEqualOrNaN(infinity_negativeInfinity), Is.EqualTo(new bool3x3(true)));
            Assert.That(infinity_negativeInfinity.IsEqualOrNaN(zero), Is.EqualTo(new bool3x3(false)));

            Assert.That(one_negativeOne_zero.IsEqualOrNaN(one_negativeOne_zero), Is.EqualTo(new bool3x3(true)));
            Assert.That(one_negativeOne_zero.IsEqualOrNaN(zero), Is.EqualTo(new bool3x3(false, false, true)));

            Assert.That(nan.IsEqualOrNaN(nan), Is.EqualTo(new bool3x3(true)));
            Assert.That(nan.IsEqualOrNaN(zero), Is.EqualTo(new bool3x3(false)));

            Assert.That(nan_negativeOne_zero_columns.IsEqualOrNaN(nan_negativeOne_zero_columns), Is.EqualTo(new bool3x3(true)));
            Assert.That(nan_negativeOne_zero_columns.IsEqualOrNaN(-nan_negativeOne_zero_columns), Is.EqualTo(new bool3x3(true, false, true)));
            Assert.That(nan_negativeOne_zero_rows.IsEqualOrNaN(nan_negativeOne_zero_rows), Is.EqualTo(new bool3x3(true)));
            Assert.That(
                nan_negativeOne_zero_rows.IsEqualOrNaN(-nan_negativeOne_zero_rows),
                Is.EqualTo(new bool3x3(
                    new bool3(true, false, true),
                    new bool3(true, false, true),
                    new bool3(true, false, true)))
                );
            Assert.That(nan_negativeOne_zero_columns.IsEqualOrNaN(zero), Is.EqualTo(new bool3x3(false, false, true)));
        }

        [Test]
        public static void IsEqualOrNaNTest_float4x4()
        {
            Assert.That(nameof(IsEqualOrNaNTest_float4x4), Does.StartWith(nameof(MathExtension.IsEqualOrNaN)));

            float4x4 zero = float4x4.zero;
            float4x4 one = new float4x4(1f);
            float4x4 one_negativeOne_zero = new float4x4(1f, -1, 0, 0);
            float4x4 nan = new float4x4(float.NaN);
            float4x4 infinity_negativeInfinity = new float4x4(float.PositiveInfinity, float.PositiveInfinity, float.NegativeInfinity, float.NegativeInfinity);
            float4x4 nan_negativeOne_zero_columns = new float4x4(float.NaN, -1, 0, 0);
            float4x4 nan_negativeOne_zero_rows = new float4x4(
                new float4(float.NaN, -1, 0, 0),
                new float4(float.NaN, -1, 0, 0),
                new float4(float.NaN, -1, 0, 0),
                new float4(float.NaN, -1, 0, 0));

            Assert.That(zero.IsEqualOrNaN(zero), Is.EqualTo(new bool4x4(true)));
            Assert.That(zero.IsEqualOrNaN(one), Is.EqualTo(new bool4x4(false)));

            Assert.That(one.IsEqualOrNaN(one), Is.EqualTo(new bool4x4(true)));
            Assert.That(one.IsEqualOrNaN(zero), Is.EqualTo(new bool4x4(false)));

            Assert.That(infinity_negativeInfinity.IsEqualOrNaN(infinity_negativeInfinity), Is.EqualTo(new bool4x4(true)));
            Assert.That(infinity_negativeInfinity.IsEqualOrNaN(zero), Is.EqualTo(new bool4x4(false)));

            Assert.That(one_negativeOne_zero.IsEqualOrNaN(one_negativeOne_zero), Is.EqualTo(new bool4x4(true)));
            Assert.That(one_negativeOne_zero.IsEqualOrNaN(zero), Is.EqualTo(new bool4x4(false, false, true, true)));

            Assert.That(nan.IsEqualOrNaN(nan), Is.EqualTo(new bool4x4(true)));
            Assert.That(nan.IsEqualOrNaN(zero), Is.EqualTo(new bool4x4(false)));

            Assert.That(nan_negativeOne_zero_columns.IsEqualOrNaN(nan_negativeOne_zero_columns), Is.EqualTo(new bool4x4(true)));
            Assert.That(nan_negativeOne_zero_columns.IsEqualOrNaN(-nan_negativeOne_zero_columns), Is.EqualTo(new bool4x4(true, false, true, true)));
            Assert.That(nan_negativeOne_zero_rows.IsEqualOrNaN(nan_negativeOne_zero_rows), Is.EqualTo(new bool4x4(true)));
            Assert.That(
                nan_negativeOne_zero_rows.IsEqualOrNaN(-nan_negativeOne_zero_rows),
                Is.EqualTo(new bool4x4(
                    new bool4(true, false, true, true),
                    new bool4(true, false, true, true),
                    new bool4(true, false, true, true),
                    new bool4(true, false, true, true)))
                );
            Assert.That(nan_negativeOne_zero_columns.IsEqualOrNaN(zero), Is.EqualTo(new bool4x4(false, false, true, true)));
        }
    }
}
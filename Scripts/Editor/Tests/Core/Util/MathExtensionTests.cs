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

        [Test]
        public static void GetInverseTest()
        {
            Assert.That(new float3(1f).GetInverse(), Is.EqualTo(new float3(1f)));
            Assert.That(new float3(2f).GetInverse(), Is.EqualTo(new float3(0.5f)));

            Assert.That(new float3(-1f).GetInverse(), Is.EqualTo(new float3(-1f)));
            Assert.That(new float3(-2f).GetInverse(), Is.EqualTo(new float3(-0.5f)));

            Assert.That(new float3(7f, -2f, 0f).GetInverse(), Is.EqualTo(new float3(1f/7f, -0.5f, float.PositiveInfinity)));

            Assert.That(float3.zero.GetInverse(), Is.EqualTo(new float3(float.PositiveInfinity)));
            Assert.That(new float3(float.PositiveInfinity, float.NegativeInfinity, float.NaN), Is.EqualTo(new float3(float.PositiveInfinity, float.NegativeInfinity, float.NaN)).Using<float3>(EqualityWithNaN));
        }

        [Test]
        public static void GetInverseSafeTest()
        {
            Assert.That(new float3(1f).GetInverseSafe(), Is.EqualTo(new float3(1f)));
            Assert.That(new float3(2f).GetInverseSafe(), Is.EqualTo(new float3(0.5f)));

            Assert.That(new float3(-1f).GetInverseSafe(), Is.EqualTo(new float3(-1f)));
            Assert.That(new float3(-2f).GetInverseSafe(), Is.EqualTo(new float3(-0.5f)));

            Assert.That(new float3(7f, -2f, 0f).GetInverseSafe(), Is.EqualTo(new float3(1f/7f, -0.5f, 0f)));

            Assert.That(float3.zero.GetInverseSafe(), Is.EqualTo(float3.zero));
            Assert.That(new float3(float.PositiveInfinity, float.NegativeInfinity, float.NaN).GetInverseSafe(), Is.EqualTo(new float3(0, 0, 0f)));
        }

        [Test]
        public static void IsApproximatelyTest()
        {

        }

        [Test]
        public static void ToSignedInfiniteTest()
        {

        }

        [Test]
        public static void IsEqualOrNaNTest()
        {

        }

    }
}
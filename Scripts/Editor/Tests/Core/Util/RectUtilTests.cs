using NUnit.Framework;
using UnityEngine;
using Anvil.Unity.Core;

namespace Anvil.Unity.Tests
{
    public static class RectUtilTests
    {
        [Test]
        public static void CreateFromPointsTest_TwoPoint()
        {
            Assert.That(nameof(CreateFromPointsTest_TwoPoint), Does.StartWith(nameof(RectUtil.CreateFromPoints)));

            Rect rect = RectUtil.CreateFromPoints(new Vector2(1, 2), new Vector2(3, 4));

            Assert.That(rect.min.x, Is.EqualTo(1));
            Assert.That(rect.min.y, Is.EqualTo(2));
            Assert.That(rect.max.x, Is.EqualTo(3));
            Assert.That(rect.max.y, Is.EqualTo(4));

            rect = RectUtil.CreateFromPoints(new Vector2(3, 4), new Vector2(1, 2));

            Assert.That(rect.min.x, Is.EqualTo(1));
            Assert.That(rect.min.y, Is.EqualTo(2));
            Assert.That(rect.max.x, Is.EqualTo(3));
            Assert.That(rect.max.y, Is.EqualTo(4));
        }

        [Test]
        public static void CreateFromPointsTest_FourPoint()
        {
            Assert.That(nameof(CreateFromPointsTest_FourPoint), Does.StartWith(nameof(RectUtil.CreateFromPoints)));

            Rect rect = RectUtil.CreateFromPoints(new Vector2(1, 2), new Vector2(3, 4), new Vector2(1, 4), new Vector2(3, 2));

            Assert.That(rect.min.x, Is.EqualTo(1));
            Assert.That(rect.min.y, Is.EqualTo(2));
            Assert.That(rect.max.x, Is.EqualTo(3));
            Assert.That(rect.max.y, Is.EqualTo(4));

            rect = RectUtil.CreateFromPoints(new Vector2(3, 4), new Vector2(1, 2), new Vector2(3, 2), new Vector2(1, 4));

            Assert.That(rect.min.x, Is.EqualTo(1));
            Assert.That(rect.min.y, Is.EqualTo(2));
            Assert.That(rect.max.x, Is.EqualTo(3));
            Assert.That(rect.max.y, Is.EqualTo(4));
        }

        [Test]
        public static void AbsSizeTest()
        {
            Assert.That(nameof(AbsSizeTest), Does.StartWith(nameof(RectUtil.AbsSize)));

            Rect rect_five_five = new Rect(5, 5, 5, 5);
            Rect rect_negativeFive_negativeFive = new Rect(-5, -5, -5, -5);
            Rect rect_negativeTen_five = new Rect(-10, -10, 5, 5);

            Assert.That(RectUtil.AbsSize(Rect.zero), Is.EqualTo(Rect.zero));
            Assert.That(RectUtil.AbsSize(rect_five_five), Is.EqualTo(rect_five_five));
            Assert.That(RectUtil.AbsSize(rect_negativeFive_negativeFive), Is.EqualTo(rect_negativeTen_five));
        }

        [Test]
        public static void AreEquivalentTest()
        {
            Assert.That(nameof(AreEquivalentTest), Does.StartWith(nameof(RectUtil.AreEquivalent)));

            Rect rect_five_five = new Rect(5, 5, 5, 5);
            Rect rect_ten_negativeFive = new Rect(10,10,-5,-5);

            Rect rect_five_six = new Rect(5, 5, 6, 6);
            Rect rect_six_five = new Rect(6, 6, 5, 5);

            Assert.That(RectUtil.AreEquivalent(rect_five_five, rect_ten_negativeFive), Is.True);
            Assert.That(RectUtil.AreEquivalent(rect_ten_negativeFive, rect_five_five), Is.True);

            Assert.That(RectUtil.AreEquivalent(rect_five_five, Rect.zero), Is.False);
            Assert.That(RectUtil.AreEquivalent(rect_ten_negativeFive, Rect.zero), Is.False);

            Assert.That(RectUtil.AreEquivalent(rect_five_five, rect_five_six), Is.False);
            Assert.That(RectUtil.AreEquivalent(rect_ten_negativeFive, rect_six_five), Is.False);
        }
    }
}
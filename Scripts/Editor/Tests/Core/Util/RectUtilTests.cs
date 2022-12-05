using NUnit.Framework;
using UnityEngine;
using Anvil.Unity.Core;

namespace Anvil.Unity.Tests
{
    public static class RectUtilTests
    {
        [Test]
        public static void CreateFromPointsTest()
        {
            Assert.That(nameof(CreateFromPointsTest), Does.StartWith(nameof(RectUtil.CreateFromPoints)));

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
    }
}
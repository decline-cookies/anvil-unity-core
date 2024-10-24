using Anvil.Unity.Core;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;

namespace Anvil.Unity.Tests
{
    [TestFixture]
    public class TransformExtensionTests
    {
        [Test]
        public static void ResetTest()
        {
            Assert.That(nameof(ResetTest), Is.EqualTo(nameof(TransformExtension.Reset) + "Test"));

            Transform transform = new GameObject().transform;

            transform.localPosition = new Vector3(RandomFloat(), RandomFloat(), RandomFloat());
            transform.localRotation = Quaternion.Euler(new Vector3(RandomFloat(), RandomFloat(), RandomFloat()));
            transform.localScale = new Vector3(RandomFloat(), RandomFloat(), RandomFloat());
            transform.Reset();
            Assert.That(transform.localPosition, Is.EqualTo(Vector3.zero));
            Assert.That(transform.localRotation, Is.EqualTo(Quaternion.identity));
            Assert.That(transform.localScale, Is.EqualTo(Vector3.one));
            return;

            float RandomFloat() => Random.Range(-float.MaxValue, float.MaxValue);
        }

        [Test]
        public static void SetLossyScaleTest()
        {
            Assert.That(nameof(SetLossyScaleTest), Is.EqualTo(nameof(TransformExtension.SetLossyScale) + "Test"));

            Vector3 scale_two = new Vector3(2f, 2f, 2f);
            Vector3 scale_two_one_two = new Vector3(2f, 1f, 2f);

            // Setup
            GameObject parentGO = new GameObject();
            Transform parentTransform = parentGO.transform;
            parentTransform.localScale = scale_two;

            GameObject childGO = new GameObject();
            Transform childTransform = childGO.transform;
            childTransform.SetParent(parentTransform, false);


            // Tests
            childTransform.SetLossyScale(scale_two);
            Assert.That(childTransform.lossyScale, Is.EqualTo(scale_two));
            Assert.That(childTransform.localScale, Is.EqualTo(new Vector3(1f,1f,1f)));

            childTransform.SetLossyScale(scale_two_one_two);
            Assert.That(childTransform.lossyScale, Is.EqualTo(scale_two_one_two));
            Assert.That(childTransform.localScale, Is.EqualTo(new Vector3(1f, 0.5f, 1f)));

            // Teardown
            GameObject.DestroyImmediate(parentGO);
            GameObject.DestroyImmediate(childGO);
        }

        [Test]
        public static void ScaleAroundPointTest()
        {
            Assert.That(nameof(ScaleAroundPointTest), Is.EqualTo(nameof(TransformExtension.ScaleAroundPoint) + "Test"));

            Transform transform = new GameObject().transform;

            transform.localPosition = new Vector3(1f, 2f, 3f);
            transform.localScale = Vector3.one;
            transform.ScaleAroundPoint(2f, Vector3.zero);
            Assert.That(transform.localScale, Is.EqualTo(new Vector3(2f, 2f, 2f)));
            Assert.That(transform.localPosition, Is.EqualTo(new Vector3(2f, 4f, 6f)));

            transform.localPosition = new Vector3(1f, 2f, 3f);
            transform.localScale = Vector3.one;
            transform.ScaleAroundPoint(new Vector3(2f, 3f, 4f), Vector3.zero);
            Assert.That(transform.localScale, Is.EqualTo(new Vector3(2f, 3f, 4f)));
            Assert.That(transform.localPosition, Is.EqualTo(new Vector3(2f, 6f, 12f)));

            transform.localPosition = new Vector3(1f, 2f, 3f);
            transform.localScale = Vector3.one;
            transform.ScaleAroundPoint(2f, new Vector3(5f, 5f, 5f));
            Assert.That(transform.localScale, Is.EqualTo(new Vector3(2f, 2f, 2f)));
            Assert.That(transform.localPosition, Is.EqualTo(new Vector3(-3f, -1f, 1f)));

            transform.localPosition = new Vector3(1f, 2f, 3f);
            transform.localScale = Vector3.one;
            transform.ScaleAroundPoint(new Vector3(2f, 3f, 4f), new Vector3(5f, 5f, 5f));
            Assert.That(transform.localScale, Is.EqualTo(new Vector3(2f, 3f, 4f)));
            Assert.That(transform.localPosition, Is.EqualTo(new Vector3(-3f, -4f, -3f)));
        }
    }
}

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
        
        [Test]
        public static void RotateAroundPointTest()
        {
            Assert.That(nameof(RotateAroundPointTest), Is.EqualTo(nameof(TransformExtension.RotateAroundPoint) + "Test"));
            
            Vector3EqualityComparer vector3Comparer = new (10e-7f);
            Transform transform = new GameObject().transform;
            
            transform.localPosition = new Vector3(1f, 2f, 3f);
            transform.localRotation = Quaternion.identity;
            transform.RotateAroundPoint(Quaternion.AngleAxis(90f, Vector3.forward), Vector3.zero);
            Assert.That(transform.localRotation.eulerAngles, Is.EqualTo(new Vector3(0f, 0f, 90f)));
            Assert.That(transform.localPosition, Is.EqualTo(new Vector3(-2f, 1f, 3f)).Using(vector3Comparer));
            
            transform.localPosition = new Vector3(1f, 2f, 3f);
            transform.localRotation = Quaternion.identity;
            transform.RotateAroundPoint(Quaternion.AngleAxis(90f, Vector3.forward), new Vector3(5f, 5f, 5f));
            Assert.That(transform.localRotation.eulerAngles, Is.EqualTo(new Vector3(0f, 0f, 90f)));
            Assert.That(transform.localPosition, Is.EqualTo(new Vector3(8f, 1f, 3f)).Using(vector3Comparer));
        }
    }
}
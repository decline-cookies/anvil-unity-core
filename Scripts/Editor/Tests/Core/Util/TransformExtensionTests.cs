using Anvil.Unity.Core;
using NUnit.Framework;
using UnityEngine;

namespace Anvil.Unity.Tests
{
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
    }
}
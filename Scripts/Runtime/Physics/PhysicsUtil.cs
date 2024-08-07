using System;
using Unity.Mathematics;
using UnityEngine;

namespace Anvil.Unity.Physics
{
    /// <summary>
    /// A collection of utility methods for working with Unity's physics system.
    /// </summary>
    public static class PhysicsUtil
    {
        /// <summary>
        /// Extrapolate the provided state for a given <see cref="UnityEngine.Rigidbody"/>.
        ///
        /// Note: Does not account for collisions or trigger <see cref="FixedUpdate"/> on the body.
        ///       For a more accurate extrapolation <see cref="UnityEngine.Physics.Simulate"/> is more appropriate.
        /// </summary>
        /// <remarks>
        /// Only drag and gravity values are read off of the provided <see cref="Rigidbody"/>. All other actor state is
        /// provided through the method parameters.
        /// </remarks>
        public static void ExtrapolateState(
            float delta,
            Rigidbody rigidbody,
            ref Vector3 position,
            ref Quaternion rotation,
            ref Vector3 linearVelocity,
            ref Vector3 angularVelocity)
        {
            float drag = rigidbody.drag;
            float angularDrag = rigidbody.angularDrag;
            Vector3 gravity = rigidbody.useGravity ? UnityEngine.Physics.gravity : Vector3.zero;

            while (delta > 0f)
            {
                float stepDelta = math.min(delta, Time.fixedDeltaTime);
                StepState(stepDelta,
                    drag,
                    angularDrag,
                    gravity,
                    ref position,
                    ref rotation,
                    ref linearVelocity,
                    ref angularVelocity);
                delta -= stepDelta;
            }
        }

        /// <summary>
        /// Extrapolate the provided state for a given <see cref="Rigidbody"/> and execute a callback on each step.
        ///
        /// Note: Does not account for collisions or trigger <see cref="FixedUpdate"/> on the body.
        ///       For a more accurate extrapolation <see cref="Physics.Simulate"/> is more appropriate.
        /// </summary>
        /// <param name="onStep">
        /// Executed after each time the state is stepped. This can be used to apply additional effects each step to
        /// simulate the FixedUpdate of other <see cref="MonoBehaviour"/>s or evaluate collisions.
        ///
        /// Note: That values aren't re-read off of the RigidBody so all changes in <see cref="onStep"/> must make
        ///       changes to the state parameters passed into the ExtrapolateState method.
        /// </param>
        /// <remarks>
        /// Only drag and gravity values are read off of the provided <see cref="Rigidbody"/>. All other actor state is
        /// provided through the method parameters.
        /// </remarks>
        public static void ExtrapolateState(
            float delta,
            Rigidbody rigidbody,
            ref Vector3 position,
            ref Quaternion rotation,
            ref Vector3 linearVelocity,
            ref Vector3 angularVelocity,
            Action<float> onStep)
        {
            float drag = rigidbody.drag;
            float angularDrag = rigidbody.angularDrag;
            Vector3 gravity = rigidbody.useGravity ? UnityEngine.Physics.gravity : Vector3.zero;

            while (delta > 0f)
            {
                float stepDelta = math.min(delta, Time.fixedDeltaTime);

                StepState(stepDelta,
                    drag,
                    angularDrag,
                    gravity,
                    ref position,
                    ref rotation,
                    ref linearVelocity,
                    ref angularVelocity);
                onStep(stepDelta);

                delta -= stepDelta;
            }
        }

        private static void StepState(
            float delta,
            float drag,
            float angularDrag,
            Vector3 gravity,
            ref Vector3 position,
            ref Quaternion rotation,
            ref Vector3 linearVelocity,
            ref Vector3 angularVelocity)
        {
            linearVelocity += (gravity * delta);
            linearVelocity *= (1f - (delta * drag));
            angularVelocity *= (1f - (delta * angularDrag));

            position += linearVelocity * delta;
            rotation *= Quaternion.Euler(angularVelocity * delta);
        }

    }
}
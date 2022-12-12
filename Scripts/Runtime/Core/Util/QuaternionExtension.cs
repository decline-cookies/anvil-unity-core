using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace Anvil.Unity.Core
{
    /// <summary>
    /// A collection of utilities for working with <see cref="quaternion"/>
    /// </summary>
    public static class QuaternionExtension
    {
        /// <summary>
        /// Rotate the quaternion 180 degrees around the world X axis.
        /// </summary>
        /// <param name="q">The quaternion to rotate</param>
        /// <returns>The rotated quaternion</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static quaternion Rotate180AroundWorldX(this quaternion q) =>
            new quaternion(q.value.w, -q.value.z, q.value.y, -q.value.x);

        /// <summary>
        /// Rotate the quaternion 180 degrees around the world Y axis.
        /// </summary>
        /// <param name="q">The quaternion to rotate</param>
        /// <returns>The rotated quaternion</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static quaternion Rotate180AroundWorldY(this quaternion q) =>
            new quaternion(q.value.z, q.value.w, -q.value.x, -q.value.y);

        /// <summary>
        /// Rotate the quaternion 180 degrees around the world Z axis.
        /// </summary>
        /// <param name="q">The quaternion to rotate</param>
        /// <returns>The rotated quaternion</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static quaternion Rotate180AroundWorldZ(this quaternion q) =>
            new quaternion(-q.value.y, q.value.x, q.value.w, -q.value.z);

        /// <summary>
        /// Rotate the quaternion 180 degrees around the local X axis.
        /// </summary>
        /// <param name="q">The quaternion to rotate</param>
        /// <returns>The rotated quaternion</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static quaternion Rotate180AroundSelfX(this quaternion q) =>
            new quaternion(q.value.w, q.value.z, -q.value.y, -q.value.x);

        /// <summary>
        /// Rotate the quaternion 180 degrees around the local Y axis.
        /// </summary>
        /// <param name="q">The quaternion to rotate</param>
        /// <returns>The rotated quaternion</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static quaternion Rotate180AroundSelfY(this quaternion q) =>
            new quaternion(-q.value.z, q.value.w, q.value.x, -q.value.y);

        /// <summary>
        /// Rotate the quaternion 180 degrees around the local Z axis.
        /// </summary>
        /// <param name="q">The quaternion to rotate</param>
        /// <returns>The rotated quaternion</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static quaternion Rotate180AroundSelfZ(this quaternion q) =>
            new quaternion(q.value.y, -q.value.x, q.value.w, -q.value.z);

        /// <summary>Returns an 180° rotated version of this quaternion around the given axis</summary>
        /// <param name="q">The quaternion to rotate</param>
        /// <param name="axis">The axis to rotate around (0=x, 2=z)</param>
        /// <param name="space">The space of the axis</param>
        /// <remarks>
        /// This method and related methods adapted from:
        /// https://github.com/FreyaHolmer/Mathfs/blob/446593cea11f7a25b8e73b8398d77f665039bcee/Runtime/Extensions.cs#L159-L177
        /// </remarks>
        public static quaternion Rotate180Around( this quaternion q, int axis, Space space ) {
            return axis switch {
                0 => space == Space.Self ? Rotate180AroundSelfX( q ) : Rotate180AroundWorldX( q ),
                1 => space == Space.Self ? Rotate180AroundSelfY( q ) : Rotate180AroundWorldY( q ),
                2 => space == Space.Self ? Rotate180AroundSelfZ( q ) : Rotate180AroundWorldZ( q ),
                _ => throw new ArgumentOutOfRangeException( nameof(axis), $"Invalid axis: {axis}. Expected 0, 1 or 2" )
            };
        }

        /// <summary>
        /// Converts a <see cref="quaternion"/> into a Euler representation in radians.
        /// </summary>
        /// <param name="q">The <see cref="quaternion"/> to convert</param>
        /// <param name="order">The rotation order</param>
        /// <returns>The Euler values.</returns>
        /// <remarks>
        /// Taken from Unity.Physics.Math.cs which in turn was taken from another assembly with the comment below:
        /// Note: taken from Unity.Animation/Core/MathExtensions.cs, which will be moved to Unity.Mathematics at some point
        ///       after that, this should be removed and the Mathematics version should be used
        /// https://github.com/needle-mirror/com.unity.physics/blob/master/Unity.Physics/Base/Math/Math.cs
        /// </remarks>
        public static float3 ToEuler(this quaternion q, math.RotationOrder order = math.RotationOrder.Default)
        {
            const float epsilon = 1e-6f;
            //prepare the data
            var qv = q.value;
            var d1 = qv * qv.wwww * new float4(2.0f); //xw, yw, zw, ww
            var d2 = qv * qv.yzxw * new float4(2.0f); //xy, yz, zx, ww
            var d3 = qv * qv;
            var euler = new float3(0.0f);
            const float CUTOFF = (1.0f - 2.0f * epsilon) * (1.0f - 2.0f * epsilon);
            switch (order)
            {
                case math.RotationOrder.ZYX:
                {
                    var y1 = d2.z + d1.y;
                    if (y1 * y1 < CUTOFF)
                    {
                        var x1 = -d2.x + d1.z;
                        var x2 = d3.x + d3.w - d3.y - d3.z;
                        var z1 = -d2.y + d1.x;
                        var z2 = d3.z + d3.w - d3.y - d3.x;
                        euler = new float3(math.atan2(x1, x2), math.asin(y1), math.atan2(z1, z2));
                    }
                    else //zxz
                    {
                        y1 = math.clamp(y1, -1.0f, 1.0f);
                        var abcd = new float4(d2.z, d1.y, d2.y, d1.x);
                        var x1 = 2.0f * (abcd.x * abcd.w + abcd.y * abcd.z); //2(ad+bc)
                        var x2 = math.csum(abcd * abcd * new float4(-1.0f, 1.0f, -1.0f, 1.0f));
                        euler = new float3(math.atan2(x1, x2), math.asin(y1), 0.0f);
                    }
                    break;
                }
                case math.RotationOrder.ZXY:
                {
                    var y1 = d2.y - d1.x;
                    if (y1 * y1 < CUTOFF)
                    {
                        var x1 = d2.x + d1.z;
                        var x2 = d3.y + d3.w - d3.x - d3.z;
                        var z1 = d2.z + d1.y;
                        var z2 = d3.z + d3.w - d3.x - d3.y;
                        euler = new float3(math.atan2(x1, x2), -math.asin(y1), math.atan2(z1, z2));
                    }
                    else //zxz
                    {
                        y1 = math.clamp(y1, -1.0f, 1.0f);
                        var abcd = new float4(d2.z, d1.y, d2.y, d1.x);
                        var x1 = 2.0f * (abcd.x * abcd.w + abcd.y * abcd.z); //2(ad+bc)
                        var x2 = math.csum(abcd * abcd * new float4(-1.0f, 1.0f, -1.0f, 1.0f));
                        euler = new float3(math.atan2(x1, x2), -math.asin(y1), 0.0f);
                    }
                    break;
                }
                case math.RotationOrder.YXZ:
                {
                    var y1 = d2.y + d1.x;
                    if (y1 * y1 < CUTOFF)
                    {
                        var x1 = -d2.z + d1.y;
                        var x2 = d3.z + d3.w - d3.x - d3.y;
                        var z1 = -d2.x + d1.z;
                        var z2 = d3.y + d3.w - d3.z - d3.x;
                        euler = new float3(math.atan2(x1, x2), math.asin(y1), math.atan2(z1, z2));
                    }
                    else //yzy
                    {
                        y1 = math.clamp(y1, -1.0f, 1.0f);
                        var abcd = new float4(d2.x, d1.z, d2.y, d1.x);
                        var x1 = 2.0f * (abcd.x * abcd.w + abcd.y * abcd.z); //2(ad+bc)
                        var x2 = math.csum(abcd * abcd * new float4(-1.0f, 1.0f, -1.0f, 1.0f));
                        euler = new float3(math.atan2(x1, x2), math.asin(y1), 0.0f);
                    }
                    break;
                }
                case math.RotationOrder.YZX:
                {
                    var y1 = d2.x - d1.z;
                    if (y1 * y1 < CUTOFF)
                    {
                        var x1 = d2.z + d1.y;
                        var x2 = d3.x + d3.w - d3.z - d3.y;
                        var z1 = d2.y + d1.x;
                        var z2 = d3.y + d3.w - d3.x - d3.z;
                        euler = new float3(math.atan2(x1, x2), -math.asin(y1), math.atan2(z1, z2));
                    }
                    else //yxy
                    {
                        y1 = math.clamp(y1, -1.0f, 1.0f);
                        var abcd = new float4(d2.x, d1.z, d2.y, d1.x);
                        var x1 = 2.0f * (abcd.x * abcd.w + abcd.y * abcd.z); //2(ad+bc)
                        var x2 = math.csum(abcd * abcd * new float4(-1.0f, 1.0f, -1.0f, 1.0f));
                        euler = new float3(math.atan2(x1, x2), -math.asin(y1), 0.0f);
                    }
                    break;
                }
                case math.RotationOrder.XZY:
                {
                    var y1 = d2.x + d1.z;
                    if (y1 * y1 < CUTOFF)
                    {
                        var x1 = -d2.y + d1.x;
                        var x2 = d3.y + d3.w - d3.z - d3.x;
                        var z1 = -d2.z + d1.y;
                        var z2 = d3.x + d3.w - d3.y - d3.z;
                        euler = new float3(math.atan2(x1, x2), math.asin(y1), math.atan2(z1, z2));
                    }
                    else //xyx
                    {
                        y1 = math.clamp(y1, -1.0f, 1.0f);
                        var abcd = new float4(d2.x, d1.z, d2.z, d1.y);
                        var x1 = 2.0f * (abcd.x * abcd.w + abcd.y * abcd.z); //2(ad+bc)
                        var x2 = math.csum(abcd * abcd * new float4(-1.0f, 1.0f, -1.0f, 1.0f));
                        euler = new float3(math.atan2(x1, x2), math.asin(y1), 0.0f);
                    }
                    break;
                }
                case math.RotationOrder.XYZ:
                {
                    var y1 = d2.z - d1.y;
                    if (y1 * y1 < CUTOFF)
                    {
                        var x1 = d2.y + d1.x;
                        var x2 = d3.z + d3.w - d3.y - d3.x;
                        var z1 = d2.x + d1.z;
                        var z2 = d3.x + d3.w - d3.y - d3.z;
                        euler = new float3(math.atan2(x1, x2), -math.asin(y1), math.atan2(z1, z2));
                    } else //xzx
                    {
                        y1 = math.clamp(y1, -1.0f, 1.0f);
                        var abcd = new float4(d2.z, d1.y, d2.x, d1.z);
                        var x1 = 2.0f * (abcd.x * abcd.w + abcd.y * abcd.z); //2(ad+bc)
                        var x2 = math.csum(abcd * abcd * new float4(-1.0f, 1.0f, -1.0f, 1.0f));
                        euler = new float3(math.atan2(x1, x2), -math.asin(y1), 0.0f);
                    }
                    break;
                }
            }
            return EulerReorderBack(euler, order);
        }
        private static float3 EulerReorderBack(float3 euler, math.RotationOrder order)
        {
            switch (order)
            {
                case math.RotationOrder.XZY:
                    return euler.xzy;
                case math.RotationOrder.YZX:
                    return euler.zxy;
                case math.RotationOrder.YXZ:
                    return euler.yxz;
                case math.RotationOrder.ZXY:
                    return euler.yzx;
                case math.RotationOrder.ZYX:
                    return euler.zyx;
                case math.RotationOrder.XYZ:
                default:
                    return euler;
            }
        }
    }
}
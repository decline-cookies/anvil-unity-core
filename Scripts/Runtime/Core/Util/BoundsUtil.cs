using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace Anvil.Unity.Core
{
    /// <summary>
    /// A collection of utility methods to supplement the <see cref="Bounds"/> object.
    /// </summary>
    public static class BoundsUtil
    {
        /// <summary>
        /// A collection of all edges as indices of the results of <see cref="GetCorners"/>.
        /// </summary>
        public static readonly int2[] EdgeLookup = new[]
        {
            new int2(0, 1),
            new int2(0, 2),
            new int2(0, 4),
            new int2(1, 3),
            new int2(1, 5),
            new int2(2, 3),
            new int2(2, 6),
            new int2(3, 7),
            new int2(4, 5),
            new int2(4, 6),
            new int2(5, 7),
            new int2(6, 7)
        };

        /// <summary>
        /// Creates <see cref="Bounds"/> from a collection of arbitrary points
        /// </summary>
        /// <param name="points">The points to consider.</param>
        /// <returns><see cref="Bounds"/> containing the points provided.</returns>
        public static Bounds CreateBoundingVolume(params Vector3[] points)
        {
            return CreateBoundingVolume((IList<Vector3>)points);
        }

        /// <inheritdoc cref="CreateBoundingVolume(UnityEngine.Vector3[])"/>
        public static Bounds CreateBoundingVolume(IList<Vector3> points)
        {
            float3 min = float.MaxValue;
            float3 max = float.MinValue;

            float4 x = new float4();
            float4 y = new float4();
            float4 z = new float4();

            //TODO: #99 - Profile and evaluate whether this can be improved.
            // - Do math.min(i + 2, points.Length) instead of (i + 1) % points.Length to avoid potential cache invalidation
            // - Strides of 4 but do a separate math.min(batchMinX, minX) call
            // - Is it even worth batching?
            int pointCount = points.Count;
            for (int i = 0; i < pointCount; i += 3)
            {
                Vector3 point1 = points[i];
                // wrap around if the batch size is not a multiple of points.Length
                Vector3 point2 = points[(i + 1) % pointCount];
                Vector3 point3 = points[(i + 2) % pointCount];

                x.x = point1.x;
                x.y = point2.x;
                x.z = point3.x;

                y.x = point1.y;
                y.y = point2.y;
                y.z = point3.y;

                z.x = point1.z;
                z.y = point2.z;
                z.z = point3.z;

                x.w = min.x;
                y.w = min.y;
                z.w = min.z;

                min.x = math.cmin(x);
                min.y = math.cmin(y);
                min.z = math.cmin(z);

                x.w = max.x;
                y.w = max.y;
                z.w = max.z;
                max.x = math.cmax(x);
                max.y = math.cmax(y);
                max.z = math.cmax(z);
            }

            return new Bounds((min + max) / 2f, max - min);
        }

        /// <summary>
        /// Get the position of every corner for a given <see cref="Bounds"/>.
        /// </summary>
        /// <param name="bounds">The bounds to get the corners of.</param>
        /// <returns>A collection of the 8 corner points.</returns>
        public static float3[] GetCorners(this Bounds bounds)
        {
            return new[]
            {
                new float3(bounds.min.x, bounds.min.y, bounds.min.z),
                new float3(bounds.min.x, bounds.min.y, bounds.max.z),
                new float3(bounds.min.x, bounds.max.y, bounds.min.z),
                new float3(bounds.min.x, bounds.max.y, bounds.max.z),
                new float3(bounds.max.x, bounds.min.y, bounds.min.z),
                new float3(bounds.max.x, bounds.min.y, bounds.max.z),
                new float3(bounds.max.x, bounds.max.y, bounds.min.z),
                new float3(bounds.max.x, bounds.max.y, bounds.max.z)
            };
        }

        /// <summary>
        /// Does a plane intersect these bounds?
        /// </summary>
        /// <remarks>
        /// <see cref="bounds"/>, <see cref="planeOrigin"/>, and <see cref="planeNormal"/> must be in the same
        /// coordinate space.
        /// </remarks>
        /// <param name="bounds">The bounds to test against.</param>
        /// <param name="planeOrigin">A point on the plane.</param>
        /// <param name="planeNormal">The plane's normal.</param>
        /// <returns>True if at least one intersection is found.</returns>
        public static bool IntersectPlane(this Bounds bounds, float3 planeOrigin, float3 planeNormal)
        {
            //TODO: #135 - Optimize using Unity.Mathematics.
            Plane plane = new Plane(planeNormal, planeOrigin);
            float3[] corners = bounds.GetCorners();
            return EdgeLookup.Any(edge => !plane.SameSide(corners[edge.x], corners[edge.y]));
        }

        /// <summary>
        /// Does a plane intersect these bounds?
        /// </summary>
        /// <remarks>
        /// <see cref="bounds"/>, <see cref="planeOrigin"/>, and <see cref="planeNormal"/> must be in the same
        /// coordinate space.
        /// </remarks>
        /// <param name="bounds">The bounds to test against.</param>
        /// <param name="planeOrigin">A point on the plane.</param>
        /// <param name="planeNormal">The plane's normal.</param>
        /// <param name="intersections">The positions where the plane intersects the provided bounds.</param>
        /// <returns>True if at least one intersection is found.</returns>
        public static bool IntersectPlane(this Bounds bounds, float3 planeOrigin, float3 planeNormal, out float3[] intersections)
        {
            float3[] corners = bounds.GetCorners();

            //TODO: #134 - Avoid alloc. We know the collection can't be >12
            List<float3> hits = new List<float3>();

            foreach (int2 edge in EdgeLookup)
            {
                float3 p1 = corners[edge[0]];
                float3 p2 = corners[edge[1]];

                float3 direction = p2 - p1;
                float denominator = math.dot(planeNormal, direction);

                if (denominator.IsApproximately(0f))
                {
                    continue;
                }

                float t = math.dot(planeNormal, planeOrigin - p1) / denominator;

                if (t is < 0f or > 1f)
                {
                    continue;
                }

                float3 intersection = p1 + (t * direction);
                hits.Add(intersection);
            }

            intersections = hits.ToArray();

            return hits.Count != 0;
        }
    }
}


using Unity.Mathematics;
using UnityEngine;

namespace Anvil.Unity.Core
{
    /// <summary>
    /// A collection of utility methods to supplement the <see cref="Rect"/> object.
    /// </summary>
    public static class RectUtil
    {
        /// <summary>
        /// Creates a <see cref="Rect"/> from two arbitrary points
        /// </summary>
        /// <param name="point1">A point to consider.</param>
        /// <param name="point2">A point to consider.</param>
        /// <returns>A <see cref="Rect"/> with corners at the two points provided.</returns>
        public static Rect CreateFromPoints(Vector2 point1, Vector2 point2)
        {
            float minX;
            float minY;
            float maxX;
            float maxY;

            if (point1.x < point2.x)
            {
                minX = point1.x;
                maxX = point2.x;
            }
            else
            {
                minX = point2.x;
                maxX = point1.x;
            }

            if (point1.y < point2.y)
            {
                minY = point1.y;
                maxY = point2.y;
            }
            else
            {
                minY = point2.y;
                maxY = point1.y;
            }

            return Rect.MinMaxRect(minX, minY, maxX, maxY);
        }

        /// <summary>
        /// Creates a <see cref="Rect"/> from four arbitrary points
        /// </summary>
        /// <param name="point1">A point to consider.</param>
        /// <param name="point2">A point to consider.</param>
        /// <param name="point3">A point to consider.</param>
        /// <param name="point4">A point to consider.</param>
        /// <returns>A <see cref="Rect"/> containing the four points provided.</returns>
        public static Rect CreateBoundingRect(Vector2 point1, Vector2 point2, Vector2 point3, Vector2 point4)
        {
            float4 x = new float4(point1.x, point2.x, point3.x, point4.x);
            float4 y = new float4(point1.y, point2.y, point3.y, point4.y);

            float minX = math.cmin(x);
            float minY = math.cmin(y);

            float maxX = math.cmax(x);
            float maxY = math.cmax(y);

            return Rect.MinMaxRect(minX, minY, maxX, maxY);
        }

        /// <summary>
        /// Creates a <see cref="Rect"/> from a collection of arbitrary points
        /// </summary>
        /// <param name="points">The points to consider.</param>
        /// <returns>A <see cref="Rect"/> containing the points provided.</returns>
        public static Rect CreateBoundingRect(params Vector2[] points)
        {
            float minX = float.MaxValue;
            float minY = float.MaxValue;

            float maxX = float.MinValue;
            float maxY = float.MinValue;

            float4 x = new float4();
            float4 y = new float4();

            //TODO: #99 - Profile and evaluate whether this can be improved.
            // - Do math.min(i + 2, points.Length) instead of (i + 1) % points.Length to avoid potential cache invalidation
            // - Strides of 4 but do a separate math.min(batchMinX, minX) call
            // - Is it even worth batching?
            for (int i = 0; i < points.Length; i+=3)
            {
                Vector2 point1 = points[i];
                // wrap around if the batch size is not a multiple of points.Length
                Vector2 point2 = points[(i + 1) % points.Length];
                Vector2 point3 = points[(i + 2) % points.Length];

                x.x = point1.x;
                x.y = point2.x;
                x.z = point3.x;

                y.x = point1.y;
                y.y = point2.y;
                y.z = point3.y;

                x.w = minX;
                y.w = minY;
                minX = math.cmin(x);
                minY = math.cmin(y);

                x.w = maxX;
                y.w = maxY;
                maxX = math.cmax(x);
                maxY = math.cmax(y);
            }

            return Rect.MinMaxRect(minX, minY, maxX, maxY);
        }

        /// <summary>
        /// Ensures a <see cref="Rect"/> is expressed in positive width and height values.
        /// </summary>
        /// <param name="rect">The <see cref="Rect"/> to evaluate.</param>
        /// <returns>
        /// A <see cref="Rect"/> that is equivalent to the provided rectangle but guaranteed to have positive dimensions.
        /// </returns>
        /// <remarks>
        /// The returned <see cref="Rect"/> may or may not be equal but will be equivalent (<see cref="AreEquivalent"/>)
        /// to the rectangle passed in.
        /// </remarks>
        public static Rect AbsSize(Rect rect)
        {
            if (rect.width < 0)
            {
                (rect.xMin, rect.xMax) = (rect.xMax, rect.xMin);
            }

            if (rect.height < 0)
            {
                (rect.yMin, rect.yMax) = (rect.yMax, rect.yMin);
            }

            return rect;
        }

        /// <summary>
        /// Checks whether two <see cref="Rect"/>s represent the equivalent area.
        /// </summary>
        /// <param name="rect1">The first <see cref="Rect"/> the evaluate.</param>
        /// <param name="rect2">The first <see cref="Rect"/> the evaluate.</param>
        /// <returns>true if the rectangles are equivalent.</returns>
        /// <remarks>
        /// Since the <see cref="Rect"/> object supports negative width/height values two objets could represent the
        /// same area while not passing an equality check.
        /// </remarks>
        public static bool AreEquivalent(Rect rect1, Rect rect2)
        {
            rect1 = AbsSize(rect1);
            rect2 = AbsSize(rect2);

            float4 rect1_float = new float4(rect1.x, rect1.y, rect1.width, rect1.height);
            float4 rect2_float = new float4(rect2.x, rect2.y, rect2.width, rect2.height);

            return math.all(rect1_float.IsApproximately(rect2_float));
        }

    }
}
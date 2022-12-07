

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
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns>A <see cref="Rect"/> containing the two points provided.</returns>
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
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="point3"></param>
        /// <param name="point4"></param>
        /// <returns>A <see cref="Rect"/> containing the four points provided.</returns>
        public static Rect CreateFromPoints(Vector2 point1, Vector2 point2, Vector2 point3, Vector2 point4)
        {
            float4 x = new float4(point1.x, point2.x, point3.x, point4.x);
            float4 y = new float4(point1.y, point2.y, point3.y, point4.y);

            float minX = math.cmin(x);
            float minY = math.cmin(y);

            float maxX = math.cmax(x);
            float maxY = math.cmax(y);

            return Rect.MinMaxRect(minX, minY, maxX, maxY);
        }
    }
}
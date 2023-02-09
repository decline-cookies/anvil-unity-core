using Anvil.CSharp.Logging;
using Unity.Mathematics;
using UnityEngine;

namespace Anvil.Unity.Core
{
    /// <summary>
    /// A collection of utilities for dealing with display screen resolutions
    /// </summary>
    public static class ScreenUtil
    {
        /// <summary>
        /// The assumed base DPI for all edge border values provided to utility methods below.
        /// This is used to normalize values against the current screen's DPI.
        /// </summary>
        public const float BASE_DPI = 102f;

        /// <summary>
        /// Normalizes an input position to the edges of <see cref="Screen"/> within the borders provided where bottom
        /// left is (-1,-1) and top right is (1,1).
        /// </summary>
        /// <param name="position">The position to evaluate for normalization.</param>
        /// <param name="edgeBorderX">
        /// The pixel width on the horizontal edges of the screen that the <see cref="position"/> should be normalized
        /// between.
        /// </param>
        /// <param name="edgeBorderY">
        /// The pixel height on the vertical edges of the screen that the <see cref="position"/> should be normalized
        /// between.
        /// </param>
        /// <param name="normalizeEdgeBordersToScreenDPI">
        /// If true, the edge border values are normalized to the current screen DPI. Otherwise,
        /// <see cref="edgeBorderX"/> and <see cref="edgeBorderY"/> are evaluated as raw pixel values.
        ///
        /// This is used to maintain a consistent physical sizes across all screen densities.
        /// </param>
        /// <param name="ignoreOffscreenValues">
        /// When the position is beyond the limits of the screen (Ex: x < 0, y > height, etc...) return 0,0 to prevent
        /// movement.
        /// </param>
        /// <returns>A position normalized within a screen edge border.</returns>
        ///
        /// <example>
        /// Provided the <see cref="edgeBorderX"/> and <see cref="edgeBorderY"/> values of (10,10) and a screen size of
        /// (100,100).
        /// A <see cref="position"/> of (x,y) will be normalized to (x1,y1)
        /// (0,0) -> (-1,-1)
        /// (5,0) -> (-0.5,-1)
        /// (50,0) -> (0,-1)
        /// (95,0) -> (0.5,-1)
        /// </example>
        public static Vector2 NormalizeToScreenEdge(
            Vector2 position,
            float edgeBorderX,
            float edgeBorderY,
            bool normalizeEdgeBordersToScreenDPI = false,
            bool ignoreOffscreenValues = true)
        {
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            // If we're outside of the screen don't scroll
            if (ignoreOffscreenValues
                && (position.x < 0 || position.x > screenWidth || position.y < 0 || position.y > screenHeight))
            {
                return Vector2.zero;
            }

            if (normalizeEdgeBordersToScreenDPI)
            {
                float edgeBorderScaleFactor = GetDPIScaleFactor();
                edgeBorderX *= edgeBorderX * edgeBorderScaleFactor;
                edgeBorderY *= edgeBorderY * edgeBorderScaleFactor;
            }

            if (position.x <= edgeBorderX)
            {
                position.x = -1f + math.max(position.x / edgeBorderX, 0f);
            }
            else if (position.x >= screenWidth - edgeBorderX)
            {
                position.x = 1f - (math.max(screenWidth - position.x, 0f) / edgeBorderX);
            }
            else
            {
                position.x = 0f;
            }

            if (position.y <= edgeBorderY)
            {
                position.y = -1f + math.max(position.y / edgeBorderY, 0f);
            }
            else if (position.y >= screenHeight - edgeBorderY)
            {
                position.y = 1f - (math.max(screenHeight - position.y, 0f) / edgeBorderY);
            }
            else
            {
                position.y = 0;
            }

            return position;
        }

        /// <summary>
        /// Calculate the factor to scale sizes by to maintain a consistent physical size across all screen densities.
        /// It's assumed that the original size values were defined at <see cref="BASE_DPI"/>.
        /// </summary>
        /// <returns>The factor to scale sizes by based on the current display's DPI</returns>
        public static float GetDPIScaleFactor()
        {
#if UNITY_ANDROID
            // According to docs the DPI value returned isn't reliable on Android. The docs have a link to a forum post
            // with a native implementation that works.
            throw new NotSupportedException("NormalizeToScreenEdgeProcessor is not supported on Android. GetDPI needs to be implemented and tested. See: https://docs.unity3d.com/ScriptReference/Screen-dpi.html");
#endif

            float dpi = Screen.dpi;
            if (dpi < float.Epsilon)
            {
                Log.GetStaticLogger(typeof(ScreenUtil)).Warning($"Screen.dpi is invalid. EdgeBorder values won't be normalized and will be evaluated as pixel values. Screen.dpi:{dpi}");
                return 1;
            }

            return dpi / BASE_DPI;
        }
    }
}
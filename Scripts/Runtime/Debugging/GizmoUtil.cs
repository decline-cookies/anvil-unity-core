using System.Diagnostics;
using UnityEngine;

namespace Anvil.Unity.Debugging
{
    /// <summary>
    /// A collection of utility methods to supplement Unity's <see cref="Gizmos"/> tools
    /// </summary>
    public static class GizmoUtil
    {
        /// <summary>
        /// Draw a Wireframe quad gizmo. Optionally showing the normal.
        /// </summary>
        /// <param name="position">The position of the center of the quad</param>
        /// <param name="size">The total size of the quad</param>
        /// <param name="showNormal">If true, a line is drawn along the normal</param>
        [Conditional("UNITY_EDITOR")]
        public static void DrawWireQuad(Vector3 position, Vector2 size, bool showNormal = false)
        {
            Vector3 halfSize = size * 0.5f;
            Vector3 min = position - halfSize;
            Vector3 max = position + halfSize;

            Vector3 bottomLeft = min;
            Vector3 topRight = max;
            Vector3 bottomRight = new Vector3(max.x, min.y, min.z);
            Vector3 topLeft = new Vector3(min.x, max.y, min.z);

            Gizmos.DrawLine(bottomLeft, topLeft);
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
            Gizmos.DrawLine(topLeft, bottomRight);
            Gizmos.DrawLine(topRight, bottomLeft);

            if (showNormal)
            {
                Gizmos.DrawLine(position, position + (Vector3.forward * 0.25f));
            }
        }
    }
}
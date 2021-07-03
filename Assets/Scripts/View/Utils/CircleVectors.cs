using UnityEngine;

namespace View.Utils
{
    public static class CircleVectors
    {
        public static Vector3[] VectorPoints(int n, float radius, Vector3 center, Vector3 normal)
        {
            var points = new Vector3[n];
            var offset = Vector3.ProjectOnPlane(Vector3.up, normal).normalized * radius;
            float angle = 0;

            for (var i = 0; i < n; i++)
            {
                var rotatedOffset = Quaternion.AngleAxis(angle, normal) * offset;
                points[i] = center + rotatedOffset;
                angle += 360f / n;
            }

            return points;
        }
    }
}
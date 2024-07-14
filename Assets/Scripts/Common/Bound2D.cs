using System;
using UnityEngine;

namespace TTT.Common
{
    [Serializable]
    public class Bounds2D : ICloneable
    {
        public float MinX;
        public float MinY;
        public float MaxX;
        public float MaxY;

        // Constructor
        public Bounds2D(float minX, float minY, float maxX, float maxY)
        {
            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;
        }

        // Check if a point is within the bounds
        public bool Contains(Vector2 point)
        {
            return point.x >= MinX && point.x <= MaxX && point.y >= MinY && point.y <= MaxY;
        }

        // Get the center of the bounds
        public Vector2 Center
        {
            get { return new Vector2((MinX + MaxX) / 2, (MinY + MaxY) / 2); }
        }

        // Get the size of the bounds
        public Vector2 Size
        {
            get { return new Vector2(MaxX - MinX, MaxY - MinY); }
        }

        // Draw the bounds in the scene view
        public void DrawGizmos()
        {
            Vector2 topLeft = new Vector2(MinX, MaxY);
            Vector2 topRight = new Vector2(MaxX, MaxY);
            Vector2 bottomLeft = new Vector2(MinX, MinY);
            Vector2 bottomRight = new Vector2(MaxX, MinY);

            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
            Gizmos.DrawLine(bottomLeft, topLeft);
        }

        public object Clone()
        {
            return new Bounds2D(MinX, MinY, MaxX, MaxY);
        }
    }

}

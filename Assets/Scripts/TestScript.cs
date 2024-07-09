using System.Collections;
using System.Collections.Generic;
using TTT.Common;
using UnityEngine;

namespace TTT
{
    public class TestScript : MonoBehaviour
    {
        public RectTransform canvasRectTransform;

        private void Start()
        {
            DrawAxes();
        }

        private void DrawAxes()
        {
            // X-Axis
            DrawLine(new Vector2(-canvasRectTransform.rect.width / 2, 0),
                     new Vector2(canvasRectTransform.rect.width / 2, 0), Color.red);

            // Y-Axis
            DrawLine(new Vector2(0, -canvasRectTransform.rect.height / 2),
                     new Vector2(0, canvasRectTransform.rect.height / 2), Color.green);
        }

        private void DrawLine(Vector2 start, Vector2 end, Color color)
        {
            GameObject line = new GameObject("Line");
            line.transform.SetParent(canvasRectTransform, false);

            LineRenderer lr = line.AddComponent<LineRenderer>();
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.positionCount = 2;
            lr.startWidth = 2f;
            lr.endWidth = 2f;
            lr.startColor = color;
            lr.endColor = color;

            Vector3[] positions = new Vector3[2];
            positions[0] = canvasRectTransform.TransformPoint(start);
            positions[1] = canvasRectTransform.TransformPoint(end);
            lr.SetPositions(positions);

            // Set LineRenderer's sorting layer to UI
            lr.sortingLayerName = "UI";
        }
    }
}

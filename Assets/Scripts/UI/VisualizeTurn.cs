using UnityEngine;
using UnityEngine.Diagnostics;
using TTT.Common;

namespace TTT.UI
{
    public class VisualizeTurn : MonoBehaviour
    {
        public void Start()
        {
            UtilClass.CreateWorldText("0123", null, Vector3.zero, 30, Color.white, TextAnchor.MiddleCenter);
        }
    }
}

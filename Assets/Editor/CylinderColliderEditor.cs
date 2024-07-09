
using UnityEngine;
using UnityEditor;
using TTT.Physics;

namespace TTT
{
    [CustomEditor(typeof(CylinderCollider))]
    public class CylinderColliderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            CylinderCollider script = (CylinderCollider)target;
            if (GUILayout.Button("Build Object"))
            {
                script.BuildCollider();
            }
            if (GUILayout.Button("Clear Collider"))
            {
                script.ClearCollider();
            }
        }
    }
}

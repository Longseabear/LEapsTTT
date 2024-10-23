using TTT.System;
using Unity.VisualScripting;
using UnityEngine;

namespace TTT.Animations
{
    public class FollowMouse : MonoBehaviour
    {
        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (UnityEngine.Physics.Raycast(ray, out hit))
            {
                Vector3 mousePosition = hit.point;
                mousePosition.y = transform.position.y;
                transform.position = mousePosition;
            }

        }

    }
}

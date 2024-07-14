using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTT
{
    public class CameraTest : MonoBehaviour
    {
        public CinemachineSmoothPath SmoothPath;
        public CinemachineVirtualCamera virtualCamera;
        private CinemachineTrackedDolly dolly;

        private float TargetPivot;

        // Start is called before the first frame update
        void Start()
        {
            dolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
            }
        }
    }
}

using Sirenix.OdinInspector;
using System;
using TTT.Rhythms;
using TTT.System;
using UnityEngine;

namespace TTT.Animations
{
    public class SimpleRotation : MonoBehaviour, IRhythmHandler
    {
        public float RotationSpeed = 100.0f;

        public float OriginalScale = 1.0f;
        public float GrowScale = 1.5f;

        [Range(0.84f, 0.99f)]
        public float ShrinkScale = 0.9f;

        public void Start()
        {
            OriginalScale = (transform.localScale.x + transform.localScale.y + transform.localScale.z) / 3.0f;

            foreach(var item in UltimateRhythmManager.Instance.BeatUnits)
            {
                item.Register(this);
            }
        }

        public void Update()
        {
            transform.Rotate(Vector3.forward, RotationSpeed * Time.deltaTime);

            if (OriginalScale < transform.localScale.x)
            {
                transform.localScale = transform.localScale * ShrinkScale;
            }
            else
            {
                transform.localScale = Vector3.one * OriginalScale;
            }
        }

        [Button("Simulation")]
        public void Receive()
        {
            transform.localScale = Vector3.one * GrowScale;
        }
    }
}

using DG.Tweening;
using Sirenix.OdinInspector;
using TTT.Rhythms;
using UnityEngine;
using UnityEngine.Timeline;

namespace TTT.GmaeObject
{
    public class BeatDoll : MonoBehaviour, IRhythmHandler
    {
        public float OriginalScale = 1.0f;
        public float GrowScale = 1.5f;

        [Range(0.84f, 0.99f)]
        public float ShrinkScale = 0.9f;

        public void Start()
        {
            OriginalScale = (transform.localScale.x + transform.localScale.y + transform.localScale.z) / 3.0f;
        }

        public void Update()
        {
            if(OriginalScale < transform.localScale.x)
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

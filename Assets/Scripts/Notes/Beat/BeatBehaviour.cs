using System;
using UnityEngine;
using UnityEngine.Playables;

namespace TTT.Beat
{
    [Serializable]
    public class BeatBehaviour : PlayableBehaviour
    {
        [SerializeField] public float BeatCurve = 0f;

        public float Interval = 0.5f;
        public int SampleBeatResolution = 200;


        public override void OnPlayableCreate(Playable playable)
        {

        }
    }
}
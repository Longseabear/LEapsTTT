using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TTT.Beat
{
    [Serializable]
    public class BeatBehaviour : PlayableBehaviour
    {
        [SerializeField] public float BeatCurve = 0f;

        public int SampleBeatResolution = 200;
        public float Pivot = 0.5f;

        public TimelineClip Clip { get; private set; }
        public int TargetMeasureIndex = 0;

        public void Initialize(TimelineClip clip)
        {
            Clip = clip;
        }
        public override void OnPlayableCreate(Playable playable)
        {

        }
        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {

        }

    }
}
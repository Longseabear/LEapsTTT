using FMOD;
using System;
using TTT.Measures;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TTT.Notes.FlowNode
{
    [Serializable]
    public class FlowNodeClip : PlayableAsset, ITimelineClipAsset
    {
        public FlowNodeBehaviour template = new FlowNodeBehaviour();

        [Range(0,1f)]
        public double pivot;

        public ClipCaps clipCaps
        { 
            get { return ClipCaps.ClipIn | ClipCaps.SpeedMultiplier; }
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<FlowNodeBehaviour>.Create(graph, template);
            FlowNodeBehaviour clone = playable.GetBehaviour();
            PlayableDirector director = owner.GetComponent<PlayableDirector>();
            MeasureDirector measureDirector = owner.GetComponent<MeasureDirector>();

            clone.NodeInitialize(new FlowNodeClipInfo(measureDirector?.ParentMeasure ?? null, playable, GetTimelineClip(director)));

            return playable;
        }

        private TimelineClip GetTimelineClip(PlayableDirector director)
        {
            if (director != null && director.playableAsset is TimelineAsset timelineAsset)
            {
                foreach (var track in timelineAsset.GetOutputTracks())
                {
                    foreach (var clip in track.GetClips())
                    {
                        if (clip.asset == this)
                        {
                            return clip;
                        }
                    }
                }
            }
            return null;
        }


        public override double duration => 0.5f;
    }
}
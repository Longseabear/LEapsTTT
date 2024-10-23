using System;
using TTT.Measures;
using TTT.Notes.FlowNode;
using TTT.Notes;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TTT.Beat
{
    [Serializable]
    public class BeatClip : PlayableAsset, ITimelineClipAsset
    {
        public BeatBehaviour template = new BeatBehaviour();

        public ClipCaps clipCaps
        {
            get { return ClipCaps.None; }
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<BeatBehaviour>.Create(graph, template);
            BeatBehaviour clone = playable.GetBehaviour();
            PlayableDirector director = owner.GetComponent<PlayableDirector>();
            clone.Initialize(GetTimelineClip(director));
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
    }
}
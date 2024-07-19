using System;
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
            var clone = playable.GetBehaviour();
            return playable;
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Timeline;

namespace TTT.Beat
{
    [TrackColor(0.9245283f, 0.616559f, 0.2982912f)]
    [TrackClipType(typeof(BeatClip))]
    public class BeatTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<BeatMixerBehaviour>.Create(graph, inputCount);
        }

        public double BeatInterval = 0.25f; // Base Beat Interval
        public List<BeatMarker> markers = new List<BeatMarker>();
        protected override void OnCreateClip(TimelineClip clip)
        {
            clip.duration = 2.0;
        }
        public void GenerateMarkers()
        {
            markers.Clear();
            double timelineDuration = timelineAsset.duration;
            for (double t = 0; t <= timelineDuration; t += BeatInterval)
            {
                markers.Add(new BeatMarker { time = t });
            }
        }
    }
}

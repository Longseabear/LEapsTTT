using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TTT.Notes.FlowNode
{
    [TrackColor(0.3787753f, 0.6742933f, 0.9698113f)]
    [TrackClipType(typeof(FlowNodeClip))]
    public class FlowNodeTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var playable = ScriptPlayable<FlowNodeMixerBehaviour>.Create(graph, inputCount);
            return playable;
        }

        [Button("4-4 Aligment")]
        private void ApplyPivotToClips()
        {   
            var clips = GetClips().ToList();
            if (clips.Count == 0)
                return;

            foreach (var clip in clips)
            {
                var flowNodeClip = clip.asset as FlowNodeClip;
                if (flowNodeClip != null)
                {
                    double offset = flowNodeClip.pivot * clip.duration;
                    clip.start -= offset;
                }
            }
        }
    }

}
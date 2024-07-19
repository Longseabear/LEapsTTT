using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTT.Measures;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TTT.Notes
{
    public struct FlowNodeClipInfo
    {
        public FlowNodeClipInfo(Measure parentMeasure, Playable playable, TimelineClip timelineClip)
        {
            ParentMeasure = parentMeasure;
            Playable = playable;
            TimelineClip = timelineClip;
        }
        public Measure ParentMeasure { get; }
        public Playable Playable { get; }
        public TimelineClip TimelineClip { get; }
    }
}

using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TTT.Measures
{
    [Serializable, CreateAssetMenu(fileName = "Measure", menuName = "Measure/SimpleMeasure")]
    public class SimpleMeasureMeta : MeasureMeta
    {
        public TimelineAsset Assets;
        public DirectorWrapMode Mode;

        public SimpleMeasureMeta() { }
        public SimpleMeasureMeta(SimpleMeasureMeta node)
        {
            Assets = node.Assets;
            Mode = node.Mode;
        }
        public override Measure Build()
        {
            return new SimpleMeasure(this);
        }

        public override MeasureMeta DeepCopy()
        {
            return new SimpleMeasureMeta(this);
        }
    }
}

using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TTT.Node;
using UnityEngine;

namespace TTT.Rhythms.Measures
{
    [Serializable]
    public class CommonTimeMeasure : MeasureNode
    {
        [ShowInInspector] public List<FlowNode> Nodes { get; private set; }

        [Serializable]
        public class CommonTimeMeasureMeta : MeasureNodeMeta
        {
            [SerializeReference] public List<FlowNodeMeta> Nodes;
            public CommonTimeMeasureMeta() 
            {
                Nodes = new List<FlowNodeMeta>();
            }
            public CommonTimeMeasureMeta(CommonTimeMeasureMeta rhs) : base(rhs)
            {
                Nodes = new List<FlowNodeMeta>();
                foreach(var node in rhs.Nodes) Nodes.Add(node.DeepCopy());
            }

            public override FlowNode Build()
            {
                return new CommonTimeMeasure(this);
            }

            public override FlowNodeMeta DeepCopy()
            {
                return new CommonTimeMeasureMeta(this);
            }
        }

        public CommonTimeMeasure(CommonTimeMeasureMeta meta) : base(meta)
        {
            Nodes = new List<FlowNode>();
            foreach (var node in meta.Nodes) Nodes.Add(node.Build());
        }

        public override FlowNode DeepCopy()
        {
            return new CommonTimeMeasure(MetaData as CommonTimeMeasureMeta);
        }

        protected override void InitializeInternal()
        {
            int eventCount = Nodes.Count;
            float divLength = 1.0f / (float)eventCount;
            float leftPadding = divLength / 2.0f;

            for (int i = 0; i < eventCount; i++)
            {
                Nodes[i].Initialize(Timer.MakeSubTimer(((float)i * divLength + leftPadding) * Length));
            }
        }

        public override IEnumerable<FlowNode> GetChilds()
        {
            return Nodes;
        }
    }
}

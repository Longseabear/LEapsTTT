using System;
using TTT.Node;
using UnityEngine;

namespace TTT.Rhythms.Measures
{
    [Serializable]
    public class MeasureProxy
    {
        [SerializeReference] public FlowNode.FlowNodeMeta MetaData;

        public MeasureProxy()
        {

        }
        public MeasureProxy(FlowNode.FlowNodeMeta metaData)
        {
            MetaData = metaData;
        }

        private FlowNode _flowNode { get; set; }

        public FlowNode Get()
        {
            if (_flowNode == null && MetaData != null) _flowNode = MetaData.Build();
            return _flowNode;
        }
    }
}

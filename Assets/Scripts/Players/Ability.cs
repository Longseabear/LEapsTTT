using Sirenix.OdinInspector;
using System;
using TTT.Node;

namespace TTT.Players
{
    [Serializable]
    public class Ability<T> where T : FlowNode
    {
        [ShowInInspector] public FlowNode.FlowNodeMeta MetaData { get; private set; }
        public T FlowNode { get; private set; }

        public Ability(FlowNode.FlowNodeMeta metaData)
        {
            MetaData = metaData;
            FlowNode = null;
        }

        public T Get()
        {
            if(FlowNode == null) FlowNode = MetaData.Build() as T;
            return FlowNode;
        }
    }
}

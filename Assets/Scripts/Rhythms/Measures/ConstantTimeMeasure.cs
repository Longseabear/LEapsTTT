using System;
using System.Collections.Generic;
using TTT.Node;

namespace TTT.Rhythms.Measures
{
    [Serializable]
    public class ConstantTImeMeasure : MeasureNode
    {
        public ConstantTImeMeasure(MeasureNodeMeta meta) : base(meta)
        {
        }

        [Serializable]
        public class ConstantTImeMeasureMeta : MeasureNodeMeta
        {
            public ConstantTImeMeasureMeta() { }
            public ConstantTImeMeasureMeta(ConstantTImeMeasureMeta rhs) : base(rhs)
            {
            }

            public override FlowNode Build()
            {
                return new ConstantTImeMeasure(this);
            }

            public override FlowNodeMeta DeepCopy()
            {
                return new ConstantTImeMeasureMeta(this);
            }
        }

        protected override void InitializeInternal()
        {
        }

        public override FlowNode DeepCopy()
        {
            return new ConstantTImeMeasure(MetaData as ConstantTImeMeasureMeta);
        }

        public override IEnumerable<FlowNode> GetChilds()
        {
            throw new NotImplementedException();
        }
    }
}

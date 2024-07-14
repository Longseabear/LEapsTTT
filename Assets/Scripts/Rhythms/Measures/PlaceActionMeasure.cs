using System;
using System.Collections.Generic;
using System.Linq;
using TTT.Node;
using TTT.Players;
using UnityEngine;

namespace TTT.Rhythms.Measures
{
    [Serializable]
    public class PlaceActionMeasure : MeasureNode
    {
        public PlaceAction PlaceAction { get;private set; }
        public float NodePosition { get; private set; }

        [Serializable]
        public class PlaceActionMeasureMeta : MeasureNodeMeta
        {
            [SerializeReference] public PlaceAction.PlaceActionMeta PlaceActionMeta;
            public float NodePosition = 1.75f;

            public PlaceActionMeasureMeta() { }
            public PlaceActionMeasureMeta(PlaceActionMeasureMeta rhs) : base(rhs)
            {
                PlaceActionMeta = rhs.PlaceActionMeta.DeepCopy() as PlaceAction.PlaceActionMeta;
                NodePosition = rhs.NodePosition;

                Length = 2.0f;
            }

            public override FlowNode Build()
            {
                return new PlaceActionMeasure(this);
            }

            public override FlowNodeMeta DeepCopy()
            {
                return new PlaceActionMeasureMeta(this);
            }

            public FlowNode Build(Player player, ITimerable timer)
            {
                FlowNode simpleAttackNode = Build();

                foreach (var flowNode in simpleAttackNode.GetAllNestedFlowNode().OfType<IPlayerBindable>())
                {
                    flowNode.Bind(player);
                }

                simpleAttackNode.Register(null);
                simpleAttackNode.Initialize(timer);

                return simpleAttackNode;
            }
        }

        public PlaceActionMeasure(PlaceActionMeasureMeta meta) : base(meta)
        {
            PlaceAction = meta.PlaceActionMeta.Build() as PlaceAction;
            NodePosition = meta.NodePosition;
        }

        public override FlowNode DeepCopy()
        {
            return new PlaceActionMeasure(MetaData as PlaceActionMeasureMeta);
        }

        protected override void InitializeInternal()
        {
            PlaceAction.Initialize(Timer.MakeSubTimer(NodePosition));
        }

        public override IEnumerable<FlowNode> GetChilds()
        {
            yield return PlaceAction;
        }
    }
}

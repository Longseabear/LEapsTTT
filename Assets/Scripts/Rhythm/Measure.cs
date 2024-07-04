using System;
using System.Collections.Generic;
using System.Linq;
using TTT.Node;
using UnityEngine;
namespace TTT.Rhythm
{

    [Serializable]
    public abstract class MeasureNode : FlowNode
    {
        [SerializeReference] public List<FlowNode> Nodes = new List<FlowNode>();
        public override void Register(FlowNode parent)
        {
            base.Register(parent);
            foreach (var node in Nodes)
            {
                node.Register(this);
            }
        }
        public override void Reset()
        {
            base.Reset();
            foreach (var node in Nodes)
            {
                node.Reset();
            }
        }

        protected override void OnPlay()
        {
            bool allFinish = true;
            foreach (var node in Nodes)
            {
                if (!node.IsFinish)
                {
                    allFinish = false;
                    node.Update();
                }
            }
            if (allFinish && Length <= Timer.ElapsedTime) ChangeState(NodeState.FINISH);
        }

        public override void Update()
        {
            if (State == NodeState.FINISH) return;
            
            // Idle => Play
            if (State == NodeState.IDLE && (Nodes.Any(node => node.Timer.StartTime < Timer.ElapsedTime) || Timer.ElapsedTime >= 0)) ChangeState(NodeState.PLAYING);

            if (State == NodeState.PLAYING)
            {
                OnPlay();
            } 
        }
    }

    [Serializable]
    public class CommonTimeMeasure : MeasureNode
    {
        public override void Initialize(ITimerable timer)
        {
            Timer = timer;

            int eventCount = Nodes.Count;
            float divLength = 1.0f / (float)eventCount;
            float leftPadding = divLength / 2.0f;

            for (int i = 0; i < eventCount; i++)
            {
                Nodes[i].SetTimer(Timer.MakeSubTimer(((float)i * divLength + leftPadding) * Length));
            }
            ChangeState(NodeState.IDLE);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using TTT.Node;
using UnityEngine;
namespace TTT.Rhythm.Measures
{

    [Serializable]
    public abstract class MeasureNode : FlowNode
    {
        [SerializeReference] public List<FlowNode> Nodes = new List<FlowNode>();
        public sealed override void Initialize(ITimerable timer)
        {
            SetTimer(timer);
            InitializeInternal();
            ChangeState(NodeState.IDLE);
        }

        protected abstract void InitializeInternal();

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
            foreach (var node in Nodes)
            {
                node.Update();
            }
        }

        public override void Update()
        {
            if (State == NodeState.FINISH)
            {
                OnFinish();
                return;
            }

            // Idle => Play
            if (State == NodeState.IDLE && (Nodes.Any(node => node.StartTime < Timer.ElapsedTime) || Timer.ElapsedTime >= 0)) ChangeState(NodeState.PLAYING);

            if (IsPlaying)
            {
                OnPlay();
            }
            else if (State == NodeState.IDLE) OnIdle();

            if (Nodes.All(item => item.IsFinish) && Length <= Timer.ElapsedTime) ChangeState(NodeState.FINISH);
        }
    }

}

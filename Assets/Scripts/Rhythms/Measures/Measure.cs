using System;
using System.Collections.Generic;
using System.Linq;
using TTT.Node;
using UnityEngine;
namespace TTT.Rhythms.Measures
{
    [Serializable]
    public abstract class MeasureNode : FlowNode, IFlowNodeParentable, IRhythmProvider
    {
        public List<IRhythmHandler> RhythmHandlers => _rhythmHandlers;

        public Rhythm Rhythm { get; }

        private Rhythm GenerateRhythm()
        {
            AnimationCurve curve = new AnimationCurve();
            foreach (var node in GetChilds())
            {

            }
            return new Rhythm(Timer.MakeSubTimer(0.0f), Length, curve);
        }

        [Serializable]
        public abstract class MeasureNodeMeta : FlowNodeMeta
        {
            public MeasureNodeMeta()
            {
            }
            public MeasureNodeMeta(MeasureNodeMeta rhs) : base(rhs) 
            {
            }
        }
        protected MeasureNode(MeasureNodeMeta meta) : base(meta)
        {
        }

        public sealed override void Initialize(ITimerable timer)
        {
            SetTimer(timer);
            InitializeInternal();
            ChangeState(NodeState.IDLE);
        }

        protected abstract void InitializeInternal();

        protected override void OnPlay()
        {
            foreach (var node in GetChilds())
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
            if (State == NodeState.IDLE && (GetChilds().Any(node => node.StartTime < Timer.ElapsedTime) || Timer.ElapsedTime >= 0)) ChangeState(NodeState.PLAYING);

            if (IsPlaying)
            {
                OnPlay();
            }
            else if (State == NodeState.IDLE) OnIdle();

            if (GetChilds().All(item => item.IsFinish) && Length <= Timer.ElapsedTime) ChangeState(NodeState.FINISH);
        }

        public abstract IEnumerable<FlowNode> GetChilds();


        private List<IRhythmHandler> _rhythmHandlers;

        public void Subscribe(IRhythmHandler register)
        {
            _rhythmHandlers.Add(register);
        }

        public void Unsubscribe(IRhythmHandler register)
        {
            _rhythmHandlers.Remove(register);
        }

        public void NotifyAll(Rhythm rhythm)
        {
            foreach(var handler in _rhythmHandlers)
            {
                handler.Receive(rhythm);
            }
        }
    }

}

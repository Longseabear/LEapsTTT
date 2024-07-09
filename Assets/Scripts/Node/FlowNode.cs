using Sirenix.OdinInspector;
using System;
using System.ComponentModel;
using TTT.Common;
using TTT.Rhythm;
using UnityEngine;

namespace TTT.Node
{
    [Serializable]
    public abstract class FlowNode : FlowNodeEntity, INormalizedValue
    {
        [Flags]
        public enum FlowNodeOption
        {
            NonPlay = 1
        }
        public static FlowNodeOption FLOW_NODE_OPTION { get; set; } = 0;

        public enum NodeState
        {
            IDLE, PLAYING, FINISH
        }

        [SerializeField] public float Length;

        [Range(-1.0f, 1.0f), DefaultValue(0.0f)]
        [SerializeField] public float Pivot;
        [ShowInInspector] public float StartTime => Timer?.StartTime ?? 0;

        [SerializeField] public float PreDelay = 0.0f;

        // FlowNode Idle
        // FlowNode Playing
        // FlowNode Finish

        [ShowInInspector, Sirenix.OdinInspector.ReadOnly] public NodeState State { get; private set; }

        public bool IsTimeOver => Timer.ElapsedTime >= Length;
        public bool IsFinish => State == NodeState.FINISH;
        public bool IsPlaying => State == NodeState.PLAYING && ((FLOW_NODE_OPTION & FlowNodeOption.NonPlay)==0);

        public ITimerable Timer { get; private set; }

        public float NormalizedValue => Mathf.Clamp01(Timer.ElapsedTime / Length);

        public void ChangeState(NodeState state)
        {
            if(State == NodeState.PLAYING)
            {
                OnPlayEnd();
            }

            State = state;
            switch (State)
            {
                case NodeState.IDLE:
                OnEnterIdle();
                break;
                case NodeState.PLAYING:
                OnEnterPlay();
                break;
                case NodeState.FINISH:
                OnEnterFinish();
                break;
            }
        }

        public abstract void Update();
        protected virtual void OnEnterIdle() { }
        protected virtual void OnIdle() { }
        protected virtual void OnEnterPlay() { }
        protected abstract void OnPlay();
        protected virtual void OnPlayEnd() { }
        protected virtual void OnEnterFinish() { }
        protected virtual void OnFinish() { }

        public abstract void Initialize(ITimerable timer);

        public void SetStartTime(float startTime)
        {
            Timer.StartTime = startTime - Pivot * Length;
            Timer.StartTime += PreDelay;
        }
        public void SetTimer(ITimerable timer)
        {
            Timer = timer;
            Timer.StartTime -= (Pivot * Length);
            Timer.StartTime += PreDelay;
        }

        public virtual void Reset()
        {
            ChangeState(NodeState.IDLE);
        }
    }
}
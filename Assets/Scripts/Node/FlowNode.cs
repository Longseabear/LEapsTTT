using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TTT.Common;
using TTT.Measures;
using TTT.Notes;
using TTT.Notes.FlowNode;
using TTT.Rhythms;
using TTT.System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

namespace TTT.Node
{
    [Serializable]
    public abstract class FlowNode : FlowNodeEntity, INormalizedValue
    {
        [Serializable]
        public abstract class FlowNodeMeta
        {
            [Range(0f, 1.0f)]
            public float Pivot = 0.5f;

            public FlowNodeMeta() { }
            public FlowNodeMeta(FlowNodeMeta node)
            {
                Pivot = node.Pivot;
            }
            public abstract FlowNodeMeta DeepCopy();
            public abstract FlowNode Build();
        }

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

        public FlowNodeMeta MetaData { get; private set; }

        public float Pivot { get; private set;  }

        public FlowNode(FlowNodeMeta meta)
        {
            MetaData = meta.DeepCopy();
            State = NodeState.IDLE;

            Pivot = meta.Pivot;
        }
        
        // Runtime Data Group
        [ShowInInspector, Sirenix.OdinInspector.ReadOnly] public NodeState State { get; private set; }

        public FlowNodeClipInfo FlowNodeClipInfo { get; private set; }
        public Playable Playable => FlowNodeClipInfo.Playable;
        [ShowInInspector] public double StartTime => FlowNodeClipInfo.TimelineClip.start; // global start time
        [ShowInInspector] public double EndTime => FlowNodeClipInfo.TimelineClip.end; // global end time

        [ShowInInspector] public double CurrentTime => Playable.GetTime(); // 0 start time
        [ShowInInspector] public double Length => EndTime - StartTime;

        [ShowInInspector] public Measure ParentMeasure => FlowNodeClipInfo.ParentMeasure;

        public bool IsTimeOver => CurrentTime >= Length;
        public bool IsFinish => State == NodeState.FINISH;
        public bool IsPlaying => State == NodeState.PLAYING && ((FLOW_NODE_OPTION & FlowNodeOption.NonPlay) == 0);

        public float NormalizedValue => Mathf.Clamp01((float)CurrentTime / (float)Length);

        public void ChangeState(NodeState state)
        {
            if (State == NodeState.PLAYING)
            {
                OnEndPlay();
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

        protected virtual void OnEnterIdle() { }
        public virtual void OnIdle() { }
        protected virtual void OnEnterPlay() { }
        public abstract void OnPlay();
        protected virtual void OnEndPlay() { }
        protected virtual void OnEnterFinish() { }
        public virtual void OnFinish() { }

        public void Initialize(FlowNodeClipInfo _flowNodeClipInfo)
        {
            FlowNodeClipInfo = _flowNodeClipInfo;
            InitializeInternal();
        }

        protected virtual void InitializeInternal() { }

        public void Reset()
        {
            ChangeState(NodeState.IDLE);
        }

        public sealed override void Register(FlowNode parent)
        {
            base.Register(parent);
        }

        public abstract FlowNode DeepCopy();
    }
}
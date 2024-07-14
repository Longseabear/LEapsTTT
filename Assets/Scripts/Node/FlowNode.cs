using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TTT.Common;
using TTT.Rhythms;
using TTT.System;
using UnityEngine;

namespace TTT.Node
{
    [Serializable]
    public abstract class FlowNode : FlowNodeEntity, INormalizedValue
    {
        [Serializable]
        public abstract class FlowNodeMeta
        {
            // DataGroup
            public float Length;

            [Range(-1.0f, 1.0f)]
            public float Pivot = 0.0f;

            public float PreDelay = 0.0f;

            public FlowNodeMeta() { }
            public FlowNodeMeta(FlowNodeMeta node)
            {
                Length = node.Length;
                Pivot = node.Pivot;
                PreDelay = node.PreDelay;
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

        public FlowNodeMeta MetaData { get; set; }

        public FlowNode(FlowNodeMeta meta)
        {
            MetaData = meta.DeepCopy();

            Length = meta.Length;
            Pivot = meta.Pivot;
            PreDelay = meta.PreDelay;

            State = NodeState.IDLE;
        }

        [SerializeField] public float Length { get; protected set; }

        [SerializeField] public float Pivot { get; protected set; }

        [SerializeField] public float PreDelay { get; protected set; }
        
        // Runtime Data Group
        [ShowInInspector, Sirenix.OdinInspector.ReadOnly] public NodeState State { get; private set; }
        [ShowInInspector] public float StartTime => Timer?.StartTime ?? 0;
        public bool IsTimeOver => Timer.ElapsedTime >= Length;
        public bool IsFinish => State == NodeState.FINISH;
        public bool IsPlaying => State == NodeState.PLAYING && ((FLOW_NODE_OPTION & FlowNodeOption.NonPlay) == 0);

        public ITimerable Timer { get; private set; }

        public float NormalizedValue => Mathf.Clamp01(Timer.ElapsedTime / Length);

        public void ChangeState(NodeState state)
        {
            if (State == NodeState.PLAYING)
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

        public void Reset()
        {
            ChangeState(NodeState.IDLE);
            if (this is IFlowNodeParentable parent)
            {
                foreach (var node in parent.GetChilds()) node.Reset();
            }
        }

        public sealed override void Register(FlowNode parent)
        {
            base.Register(parent);

            if(this is IRhythmProviderBindable rhythmable)
            {
                IRhythmProvider provider = null;

                for(FlowNode cur = Parent; cur != null; cur = cur.Parent)
                {
                    if (cur is IRhythmProvider)
                    {
                        provider = cur as IRhythmProvider;
                        break;
                    }
                }

                rhythmable.Bind(provider ?? UltimateRhythmManager.Instance);
            }

            if(this is IFlowNodeParentable parentable)
            {
                foreach (var node in parentable.GetChilds()) node.Register(this);
            }
        }

        public abstract FlowNode DeepCopy();
    }

    public interface IFlowNodeParentable
    {
        public IEnumerable<FlowNode> GetChilds();
    }

    public static class FlowNodeExtension
    {
        public static IEnumerable<FlowNode> GetAllNestedFlowNode(this FlowNode currentNode)
        {
            yield return currentNode;
            if (currentNode is IFlowNodeParentable parent)
            {
                foreach (var child in parent.GetChilds())
                {
                    foreach (var nestedChild in GetAllNestedFlowNode(child))
                    {
                        yield return nestedChild;
                    }
                }
            }
        }
    }
}
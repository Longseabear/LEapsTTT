//#define PRINT_SEQUENCE

using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

#if PRINT_SEQUENCE
using System.Diagnostics;
#endif

namespace TTT.Notes.FlowNode
{
    [Serializable]
    public class FlowNodeBehaviour : PlayableBehaviour
    {
        [SerializeReference] public TTT.Node.FlowNode.FlowNodeMeta MetaData;
        [ShowInInspector] public TTT.Node.FlowNode node { get; private set; }
        public double StartTime { get; set; }

        public override void OnPlayableCreate(Playable playable)
        {
#if PRINT_SEQUENCE
            string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
            UnityEngine.Debug.Log($"{methodName} called when {Time.time}");
#endif


            if (MetaData != null)
            {
                node = MetaData.Build();
            }
        }

        // After Clone in clip
        public void NodeInitialize(FlowNodeClipInfo flowNodeClipInfo)
        {

#if PRINT_SEQUENCE
            string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
            UnityEngine.Debug.Log($"{methodName} called when {Time.time}");
#endif

            node.Initialize(flowNodeClipInfo);
        }

        public override void OnGraphStart(Playable playable)
        {
#if PRINT_SEQUENCE
            string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
            UnityEngine.Debug.Log($"{methodName} called when {Time.time}");
#endif

            if (node != null)
            {
                node.Reset();
            }
        }
        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
#if PRINT_SEQUENCE
            string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
            UnityEngine.Debug.Log($"{methodName} called when {Time.time}");
#endif

            if (node != null)
            {
                node.ChangeState(Node.FlowNode.NodeState.PLAYING);
            }
        }
        public override void OnGraphStop(Playable playable)
        {
#if PRINT_SEQUENCE
            string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
            UnityEngine.Debug.Log($"{methodName} called when {Time.time}");
#endif

            if (node != null)
            {
                node.Reset();
            }
        }
        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
#if PRINT_SEQUENCE
            string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
            UnityEngine.Debug.Log($"{methodName} called when {Time.time}");
#endif

            if (node != null)
            {
                node.ChangeState(Node.FlowNode.NodeState.FINISH);
            }
        }
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
//#if PRINT_SEQUENCE
//            string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
//            UnityEngine.Debug.Log($"{methodName} called when {Time.time}");
//#endif

            if (node == null) return;

            if (node.State == Node.FlowNode.NodeState.IDLE) node.OnIdle();
            if (node.State == Node.FlowNode.NodeState.PLAYING) node.OnPlay();
            if (node.State == Node.FlowNode.NodeState.FINISH) node.OnFinish();
        }
    }
}

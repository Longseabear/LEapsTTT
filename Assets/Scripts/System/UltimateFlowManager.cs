using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using TTT.Common;
using TTT.GmaeObject;
using TTT.Node;
using TTT.Rhythms;
using UnityEngine;

namespace TTT.System
{
    public class UltimateFlowManager : MonoBehaviour
    {
        [Header("Instance")]
        public static UltimateFlowManager Instance;


        [Header("Debug Module")]

        [OnValueChanged("SetFlowNode")]
        public int TargetID;
        public BeatDoll VisualizeObject;
        [SerializeReference] public FlowNode FlowNode;
        private void SetFlowNode()
        {
            if (FlowNode is IRhythmProvider provider && VisualizeObject != null)
            {
                provider.Unsubscribe(VisualizeObject);
            }

            if (TargetID > FlowNodes.Length) return;
            FlowNode = FlowNodes[TargetID] as FlowNode;

            if (FlowNode is IRhythmProvider provider2 && VisualizeObject != null)
            {
                provider2.Subscribe(VisualizeObject);
            }
        }


        [Header("FlowNodes")]
        public FlowNodeEntity[] FlowNodes = new FlowNodeEntity[MAX_FLOW_NODE_NUM];
        Dictionary<string, FlowNodeEntity> NameToFlowNode = new Dictionary<string, FlowNodeEntity>();
        private const int MAX_FLOW_NODE_NUM = 1028;

        public struct FlowInformation
        {
            public int ID;
            public string ClassName;
            public string NodeName;
            public FlowInformation(int id, string className, string nodeName)
            {
                ID = id;
                ClassName = className;
                NodeName = nodeName;
            }
        }
        [ShowInInspector] List<FlowInformation> flowInfo => FlowNodes.Where(item => item != null).Select(item => new FlowInformation(item.ID, item.GetType().Name, item.Name)).ToList();


        private int NextID = 0;

        public void Reset()
        {
            NextID = 0;
            NameToFlowNode.Clear();
        }
        public int Register(FlowNodeEntity node)
        {
            FlowNodes[NextID] = node;
            return NextID++;
        }
        public void RegisterUsingName(FlowNodeEntity node)
        {
            NameToFlowNode[node.Name] = node;
        }

        public FlowNodeEntity this[int index] => FlowNodes[index];

        public void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}

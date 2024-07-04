using Sirenix.OdinInspector;
using TMPro;
using TTT.Assets.Scripts.Rhythm;
using TTT.Node;
using TTT.Rhythm;
using Unity.VisualScripting;
using UnityEngine;
using ProgressBar = TTT.UI.ProgressBar;

namespace TTT
{
    class SimpleTest : MonoBehaviour
    {
        public TextMeshProUGUI Text;
        public ProgressBar ProgressBar;
        
        [PropertyRange(1f, 1000f)]
        [OnValueChanged("OnBPMChanged")]
        public float BPM = 60;


        [OnValueChanged("TextVisualize")]
        public int ROI_index;
        public TextMeshProUGUI Text2;

        private FlowNode _ROI_node = null;
        void TextVisualize()
        {
            _ROI_node = System.UltimateFlowManager.Instance[ROI_index] as FlowNode;
        }

        private void OnBPMChanged()
        {
            _count = (int)Mathf.Ceil((NodeProcessor.Timer.ElapsedTime * BPM) / 60f - 1);
            SetNextTriggerTime();
        }

        private void SetNextTriggerTime()
        {
            nextTriggerTime = (float)(_count + 1) * (60f / BPM);
        }

        [ShowInInspector] private float nextTriggerTime;
        private int _count = 0;
        
        [SerializeReference] public NodeProcessor NodeProcessor;


        void OnMeasureChanged(FlowNode node)
        {
            if(node is INormalizedValue inode)
            {
                ProgressBar.Bind(inode);
            }
        }

        void Start()
        {
            NodeProcessor.Initialize();
            NodeProcessor.OnNodeChanged += OnMeasureChanged;
            ProgressBar.Bind(NodeProcessor.CurrentProcessedNode);
            _ROI_node = NodeProcessor.Turns[1];
        }

        public void Update()
        {
            Text.text = NodeProcessor.Timer.ToString() + $" Bit: {_count}";

            if(_ROI_node is not null)
            {
                Text2.text = _ROI_node.Timer.ToString() + $" Bit: {_count}";
            }
            NodeProcessor.Update();
        }
    }
}


using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using TTT.Node;
using TTT.Players;
using TTT.Rhythms;
using TTT.System;
using UnityEngine;
using ProgressBar = TTT.UI.ProgressBar;

namespace TTT
{
    class SimpleTest : MonoBehaviour
    {
        public TextMeshProUGUI Text;
        public ProgressBar ProgressBar;

        public PlayerMeta Player1Meta;
        public PlayerMeta Player2Meta;

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

        //public async void Update()
        //{
        //    //Text.text = NodeProcessor.Timer.ToString() + $" Bit: {_count}";
        //    Text.text = $"{UltimateSimulationManager.Instance.SimulationParam.ProcessedFrame}/{UltimateSimulationManager.Instance.SimulationParam.ProcessedMeasure} : {UltimateSimulationManager.Instance.SimulationParam.MainTimer.ToString()}";
        //}

        private async void Start()
        {
            UltimateSimulationManager.Instance.Initialize(new Hero(Player1Meta), new Enemy(Player2Meta));
            //await MyUniTask();
        }
        private async UniTask MyUniTask()
        {
            var result = UltimateGamePlay.Instance.UIBoard.Board.CheckFinish();
            if (result.WinPlayer > 0)
            {
                Debug.Log($"Winner {result.WinPlayer}");
                UltimateGamePlay.Instance.ClearBoard();
                await UniTask.Delay(2000);
            }

            NodeProcessor.Update();
        }

        bool _isProcessing = false;
        public async void Update()
        {
            Text.text = UltimateSimulationManager.Instance.SimulationParam.MainTimer.ToString();
            Text2.text = UltimateSimulationManager.Instance.SimulationParam.LocalTimer.ToString();

            if (_isProcessing) return;

            _isProcessing = true;

            try
            {
                await MyUniTask();
            }
            finally
            {
                _isProcessing = false;
            }
        }
    }
}

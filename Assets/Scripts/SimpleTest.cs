using Sirenix.OdinInspector;
using TMPro;
using TTT.Audio;
using TTT.Rhythm;
using UnityEngine;
using UnityEngine.UIElements;
using TTT.UI;
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

        [SerializeReference] public Measure measure;

        private void OnBPMChanged()
        {
            _count = (int)Mathf.Ceil((_timer.ElapsedTime * BPM) / 60f - 1);
            SetNextTriggerTime();
        }

        private void SetNextTriggerTime()
        {
            nextTriggerTime = (float)(_count + 1) * (60f / BPM);
        }

        private Timer _timer = new Timer();

        [ShowInInspector] private float nextTriggerTime;
        private int _count = 0;

        void Start()
        {
            _timer = new Timer();
            _timer.Initialize();

            measure.Initialize(_timer);
            ProgressBar.Bind(measure);
        }

        public void Update()
        {
            if (_timer.ElapsedTime >= nextTriggerTime)
            {
                //UltimateAudioManager.Instance.Play(SFXBundle.SFX.drumsticks);
                _count++;
                SetNextTriggerTime();
            }

            Text.text = _timer.ToString() + $" Bit: {_count}";

            measure.Update();
        }
    }
}

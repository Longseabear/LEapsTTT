using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using TTT.Event;
using UnityEditor.Experimental.GraphView;
using UnityEngine; 
namespace TTT.Rhythm
{
    public abstract class Measure : INormalizedValue
    {
        private Timer Timer { get; set; }

        [Header("DebugInfomation")]
        [SerializeField, Min(0.01f), OnValueChanged("UpdateMeasureTimingVariable")] public float Period = 0.01f;

        [ShowInInspector] private int _iterationCount;
        [ShowInInspector] private bool _isRunning;
        private float lastFinishElapsedTime;

        public float CurrentPos { get; private set; }
        public float CurrentRelativePos { get; private set; }

        public float NormalizedValue { get => CurrentRelativePos; }
        public void Initialize(Timer timer)
        {
            lastFinishElapsedTime = 0f;
            Timer = timer;

            UpdateMeasureTimingVariable();

            InitializeInternal();

            _isRunning = true;
        }

        private void UpdateMeasureTimingVariable()
        {
            if(Timer.ElapsedTime - lastFinishElapsedTime >= Period)
            {
                Reset();
                _iterationCount = (int)Mathf.Floor(Timer.ElapsedTime / Period);
                lastFinishElapsedTime = (float)_iterationCount * Period;
            }

            CurrentPos = Timer.ElapsedTime - lastFinishElapsedTime;
            CurrentRelativePos = CurrentPos / Period;
        }

        protected abstract void InitializeInternal();
        protected abstract void Process();
        protected abstract void Reset();

        public void Start()
        {
            _isRunning = true;
        }
        public void Stop()
        {
            _isRunning = false;
        }

        public void Update()
        {
            if (_isRunning)
            {
                UpdateMeasureTimingVariable();
                Process();
            }
        }
    }

    [Serializable]
    public class CommonTimeMeasure : Measure
    {
        // 절대값과 상대값 중 뭘로하냐..
        [SerializeReference, InlineProperty, OnValueChanged("InitializeInternal")] public List<Note> NoteEvents = new List<Note>();

        private int NoteIndex = 0;
        protected override void InitializeInternal()
        { 
            int eventCount = NoteEvents.Count;
            float divLength = 1.0f / (float)eventCount;
            float leftPadding = divLength / 2.0f;
            
            for(int i = 0; i < eventCount; i++)
            {
                NoteEvents[i].Initialize();
                NoteEvents[i].Timing = (float)i * divLength + leftPadding;
            }

            for(NoteIndex = 0; NoteIndex < eventCount; NoteIndex++)
            {
                if (!NoteEvents[NoteIndex].Valid(CurrentRelativePos)) break;
            }
            if (NoteIndex == NoteEvents.Count) NoteIndex = 0;
        }
        
        protected override void Process()
        {
            if (NoteIndex < NoteEvents.Count && NoteEvents[NoteIndex].Valid(CurrentRelativePos))
            {
                NoteEvents[NoteIndex].EventOccur();
                // Debug.Log($"Run: {NoteIndex}, Current {CurrentPos}, Relative {CurrentRelativePos}");
                NoteIndex++;
            }
        }

        protected override void Reset()
        {
            NoteIndex = 0;
        }
    }
}

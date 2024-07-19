using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace TTT.Rhythms
{
    public interface ITimerable
    {
        public float StartTime { get; set; }
        public float ElapsedTime { get; }
        public float TimeScale { get; }

        public string StringFormat()
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(ElapsedTime);

            string timeString = string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}",
                timeSpan.Hours,
                timeSpan.Minutes,
                timeSpan.Seconds,
                timeSpan.Milliseconds);

            return timeString;
        }

        public ITimerable MakeSubTimer(float _startTime)
        {
            return new SubTimer(this, _startTime);
        }
    }

    [DefaultExecutionOrder(-100)]
    public class Timer : MonoBehaviour, ITimerable
    {
        public float StartTime { get; set; }
        public float ElapsedTime { get; private set; }
        [ShowInInspector] public float TimeScale { get; set; } = 1.0f;
        
        private bool isPaused = false;

        public void Start()
        {
            StartTime = 0;
            ElapsedTime = 0;
            isPaused = false;
        }

        public void Update()
        {
            if (!isPaused)
            {
                ElapsedTime += Time.deltaTime * TimeScale;
            }
        }

        public void Pause()
        {
            isPaused = true;
        }

        public void Resume()
        {
            isPaused = false;
        }

        public void Reset()
        {
            StartTime = 0;
            ElapsedTime = 0;
            isPaused = false;
        }

        public override string ToString()
        {
            return (this as ITimerable).StringFormat();
        }
    }

    public class SubTimer : ITimerable
    {
        public float StartTime { get; set; }
        public float ElapsedTime => Timer.ElapsedTime - StartTime;
        public float TimeScale => Timer.TimeScale;
        public ITimerable Timer { get; set; }

        public SubTimer(ITimerable timer, float startTime)
        {
            Timer = timer;
            StartTime = startTime;
        }
        public override string ToString()
        {
            return (this as ITimerable).StringFormat();
        }
    }

    public class ManualTimer : ITimerable
    {
        public float StartTime { get; set; }
        public float ElapsedTime { get; private set; }
        [ShowInInspector] public float TimeScale { get; set; } = 1.0f;

        private bool isPaused = false;

        public void Start()
        {
            StartTime = 0;
            ElapsedTime = 0;
            isPaused = false;
        }

        public void Set(float deltaTime)
        {
            ElapsedTime = deltaTime;
        }

        public void Pause()
        {
            isPaused = true;
        }

        public void Resume()
        {
            isPaused = false;
        }

        public void Reset()
        {
            StartTime = 0;
            ElapsedTime = 0;
            isPaused = false;
        }

        public override string ToString()
        {
            return (this as ITimerable).StringFormat();
        }
    }
}
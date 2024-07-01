using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace TTT.Rhythm
{
    [Serializable]
    public class Timer
    {
        [ShowInInspector] public float StartTime { get; private set; }
        [ShowInInspector] public float ElapsedTime => Time.time - StartTime;

        public void Initialize()
        {
            StartTime = Time.time; 
        }

        public override string ToString()
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(ElapsedTime);

            string timeString = string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}",
                timeSpan.Hours,
                timeSpan.Minutes,
                timeSpan.Seconds,
                timeSpan.Milliseconds);

            return timeString;
        }
    }
}
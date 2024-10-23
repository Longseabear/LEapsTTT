using System;
using UnityEngine;

namespace TTT.Audio
{
    [Serializable]
    public struct AudioParameter
    {
        [SerializeField] public float Volume;
        [SerializeField] public float Pitch;
        [SerializeField] public float StartTime;

        public AudioParameter(float volume = 1.0f, float pitch = 1.0f, float startTime = 0) : this()
        {
            Volume = volume;
            Pitch = pitch;
            StartTime = startTime;
        }
    }
}

using Sirenix.OdinInspector;
using TTT.Audio;
using UnityEngine;

namespace TTT.System
{
    public class UltimateAudioManager : MonoBehaviour, IAudioVolume
    {
        [Header("Instance")]
        public static UltimateAudioManager Instance;

        [Range(0f, 1f)]
        public float MasterVolume = 1.0f;
        public float Volume { get => MasterVolume; set => MasterVolume = value; }

        // Primitive Audio Bundle
        public SFXBundle SFXBundle = new SFXBundle();

        public void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            SFXBundle.Load();
        }

        [Button("Play SFX")]
        public void Play(SFXBundle.SFX target, float _volume=1.0f)
        {
            AudioParameter param = new AudioParameter(_volume);
            SFXBundle.Play(target, param);
        }

        [Button("Play SFX 2")]
        public void Play_latest(SFXBundle.SFX target, AudioParameter param)
        {
            SFXBundle.Play(target, param);
        }

    }
}

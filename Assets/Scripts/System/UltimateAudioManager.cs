using Sirenix.OdinInspector;
using TTT.Audio;
using UnityEngine;

namespace TTT
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
            SFXBundle.Play(target, _volume);
        }
    }
}

using Sirenix.OdinInspector;
using System.Collections.Generic;
using TTT.Audio;
using UnityEngine;

namespace TTT.System
{
    public class UltimateAudioManager : MonoBehaviour
    {
        [Header("Instance")]
        [ShowInInspector] public static UltimateAudioManager Instance;

        [Range(0f, 1f)]
        public float MasterVolume = 1.0f;
        public float Volume { get => MasterVolume; set => MasterVolume = value; }

        public int BankSize = 10;
        private List<AudioSource> _sourceClip;

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

        [Button("Initialize")]
        public void Start()
        {
            _sourceClip = new List<AudioSource>();
            for(int i=0; i < BankSize; i++)
            {
                GameObject sourceInstance = new GameObject($"Sound {i}");
                sourceInstance.AddComponent<AudioSource>();
                sourceInstance.transform.parent = transform;
                _sourceClip.Add(sourceInstance.GetComponent<AudioSource>());
            }
        }

        private void SetParameter(AudioSource source, AudioClip clip, AudioParameter parameter)
        {
            source.clip = clip;
            source.volume = parameter.Volume;
            source.time = parameter.StartTime;
        }

        //private AudioItem AppendAudioSource()
        //{
        //    GameObject sourceInstance = new GameObject($"Sound {_sourceClip.Count}");
        //    sourceInstance.AddComponent<AudioSource>();
        //    sourceInstance.transform.parent = transform;
        //    _sourceClip.Add(new AudioItem(sourceInstance.GetComponent<AudioSource>()));
        //    return _sourceClip[_sourceClip.Count - 1];
        //}

        [Button("Play SFX")]
        public void Play(AudioClip target, AudioParameter param)
        {
            if (target.length < param.StartTime) return;

            for (int i = 0; i < _sourceClip.Count; i++)
            {
                if (!_sourceClip[i].isPlaying)
                {
                    SetParameter(_sourceClip[i], target, param);
                    _sourceClip[i].Play();
                    return;
                }
            }

            GameObject sourceInstance = new GameObject("Sound");
            sourceInstance.AddComponent<AudioSource>();
            sourceInstance.transform.parent = transform;
            SetParameter(sourceInstance.GetComponent<AudioSource>(), target, param);
            sourceInstance.GetComponent<AudioSource>().Play();
            _sourceClip.Add(sourceInstance.GetComponent<AudioSource>());
        }

        [Button("Play with delay")]
        public void PlayWithDelay(AudioClip target, AudioParameter param, double delay)
        {
            if (target.length < param.StartTime || delay < 0) return;

            double timeScale = UltimateSimulationManager.Instance.Timer.TimeScale;

            for (int i = 0; i < _sourceClip.Count; i++)
            {
                if (!_sourceClip[i].isPlaying)
                {
                    SetParameter(_sourceClip[i], target, param);             
                    _sourceClip[i].PlayScheduled(AudioSettings.dspTime + delay / timeScale);
                    return;
                }
            }

            GameObject sourceInstance = new GameObject("Sound");
            sourceInstance.AddComponent<AudioSource>();
            sourceInstance.transform.parent = transform;
            SetParameter(sourceInstance.GetComponent<AudioSource>(), target, param);
            if (timeScale != 0) sourceInstance.GetComponent<AudioSource>().PlayScheduled(AudioSettings.dspTime + delay / timeScale);

            _sourceClip.Add(sourceInstance.GetComponent<AudioSource>());
        }

        //public SoundInfo GetInfo(SFXBundle.SFX target)
        //{
        //    return SFXBundle.Info(target);
        //}
    }

}

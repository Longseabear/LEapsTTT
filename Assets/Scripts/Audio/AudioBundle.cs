using System;
using UnityEngine;
using FMODUnity;
using FMOD;
using System.IO;

namespace TTT.Audio
{
    public interface IAudioVolume
    {
        float Volume { get; set; }
    }

    [Serializable]
    public struct AudioParameter
    {
        [SerializeField] public float Volume;
        [SerializeField] public float Pitch;

        public AudioParameter(float volume = 1.0f, float pitch = 1.0f) : this()
        {
            Volume = volume;
            Pitch = pitch;
        }
    }

    [Serializable]
    public abstract class AudioBundle : IAudioVolume
    {
        public ChannelGroup ChannelGroup { get; protected set; }
        public float Volume { get; set; } = 1.0f;
        public abstract void Load();

        public IAudioVolume Parent { get; protected set; } = null;
        public float ParentVolume => Parent?.Volume ?? 1.0f;
    }

    [Serializable]
    public abstract class AudioBundle<T> : AudioBundle where T : Enum
    {
        public abstract void Play(T target, AudioParameter _volume);
    }

    public class SFXBundle : AudioBundle<SFXBundle.SFX>
    {
        private static readonly string BundleFolderName = "SFX";

        public Sound[] Sounds { get; private set; }
        public Channel[] Channels { get; private set; }
        public DSP[] pitchDSPs { get; private set; }

        public enum SFX
        {
            drumsticks
        }

        public override void Load()
        {
            int count = Enum.GetValues(typeof(SFX)).Length;

            ChannelGroup = new ChannelGroup();
            Sounds = new Sound[count];
            Channels = new Channel[count];
            pitchDSPs = new DSP[count];

            for(int i = 0; i < count; i++)
            {
                string fileName = $"{Enum.GetName(typeof(SFX), i)}.ogg";
                RuntimeManager.CoreSystem.createSound(Path.Combine(Application.streamingAssetsPath, BundleFolderName, fileName), MODE.CREATESAMPLE, out Sounds[i]);
                RuntimeManager.CoreSystem.createDSPByType(DSP_TYPE.PITCHSHIFT, out pitchDSPs[i]);
            }

            foreach (var channel in Channels)
            {
                channel.setChannelGroup(ChannelGroup);
            }
        }

        public override void Play(SFX target, AudioParameter param)
        {
            int targetIndex = (int)target;
            Channel channel = Channels[targetIndex];

            channel.stop();
            RuntimeManager.CoreSystem.playSound(Sounds[targetIndex], ChannelGroup, false, out channel);

            channel.addDSP(CHANNELCONTROL_DSP_INDEX.HEAD, pitchDSPs[targetIndex]);
            pitchDSPs[targetIndex].setParameterFloat((int)DSP_PITCHSHIFT.PITCH, param.Pitch);

            channel.setPaused(true);
            channel.setVolume((param.Volume * Volume * ParentVolume));
            channel.setPaused(false);
        }
    }
}

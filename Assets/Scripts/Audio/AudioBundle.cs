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
        public abstract void Play(T target, float _volume);
    }

    public class SFXBundle : AudioBundle<SFXBundle.SFX>
    {
        private static readonly string BundleFolderName = "SFX";

        public Sound[] Sounds { get; private set; }
        public Channel[] Channels { get; private set; }

        public enum SFX
        {
            drumsticks
        }

        public override void Load()
        {
            int count = System.Enum.GetValues(typeof(SFX)).Length;

            ChannelGroup = new ChannelGroup();
            Sounds = new Sound[count];
            Channels = new Channel[count];

            for(int i = 0; i < count; i++)
            {
                string fileName = $"{System.Enum.GetName(typeof(SFX), i)}.ogg";
                RuntimeManager.CoreSystem.createSound(Path.Combine(Application.streamingAssetsPath, BundleFolderName, fileName), MODE.CREATESAMPLE, out Sounds[i]);
            }

            foreach (var channel in Channels)
            {
                channel.setChannelGroup(ChannelGroup);
            }
        }

        public override void Play(SFX target, float _volume)
        {
            int targetIndex = (int)target;
            Channel channel = Channels[targetIndex];

            channel.stop();
            RuntimeManager.CoreSystem.playSound(Sounds[targetIndex], ChannelGroup, false, out channel);

            channel.setPaused(true);
            channel.setVolume((_volume * Volume * ParentVolume));
            channel.setPaused(false);
        }
    }
}

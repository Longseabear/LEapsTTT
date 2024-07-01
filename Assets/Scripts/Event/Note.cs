using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TTT.Audio;
using UnityEngine;

namespace TTT.Event
{
    [Serializable]
    public abstract class Note
    {
        [SerializeField] public float Timing;

        public virtual void Initialize()
        {

        }
        public abstract void EventOccur();
        public virtual bool Valid(float CurrentTime)
        {
            return Timing <= CurrentTime;
        }
    }

    [Serializable]
    public class NoteBundle : Note
    {
        [SerializeReference] public List<Note> Notes;

        public override void EventOccur()
        {
            foreach (var note in Notes) { note.EventOccur(); }
        }
        public override void Initialize()
        {
            foreach (var note in Notes) { note.Initialize(); }
        }
    }

    [Serializable]
    public class SoundEffectNote : Note
    {
        [EnumPaging] public SFXBundle.SFX TargetSound;
        public override void EventOccur()
        {
            UltimateAudioManager.Instance.Play(SFXBundle.SFX.drumsticks);
        }
    }
}
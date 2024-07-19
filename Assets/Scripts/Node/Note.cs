using Sirenix.OdinInspector;
using System;
using TTT.Audio;
using TTT.Common;
using TTT.Rhythms;
using TTT.System;
using UnityEngine;
using UnityEngine.Playables;

namespace TTT.Node
{
    // Length = 0. Note.
    [Serializable]
    public abstract class Note : FlowNode
    {
        [Serializable]
        public abstract class NoteMeta : FlowNodeMeta
        {
            public NoteMeta()
            {

            }
            public NoteMeta(NoteMeta rhs) : base(rhs)
            {
            }
        }

        protected Note(NoteMeta meta) : base(meta)
        {
        }
    }
    public class Comma : FlowNode
    {
        [Serializable]
        public class CommaMeta : FlowNodeMeta
        {
            public override FlowNode Build()
            {
                return new Comma(this);
            }
            public CommaMeta()
            {
            }
            public CommaMeta(CommaMeta rhs) : base(rhs)
            {
            }
            public override FlowNodeMeta DeepCopy()
            {
                return new CommaMeta(this);
            }
        }

        public Comma(CommaMeta meta) : base(meta)
        {
        }

        public override FlowNode DeepCopy()
        {
            return new Comma(MetaData as CommaMeta);
        }

        protected override void InitializeInternal()
        {
            ChangeState(NodeState.FINISH);
        }

        public override void OnPlay()
        {
        }
    }

    [Serializable]
    public abstract class Segment : FlowNode
    {
        public Segment(SegmentMeta meta) : base(meta)
        {
        }

        [Serializable]
        public abstract class SegmentMeta : FlowNodeMeta
        {
            public SegmentMeta()
            {
            }
            public SegmentMeta(SegmentMeta rhs) : base(rhs)
            {
            }
        }
    }

    [Serializable]
    public class SoundEffectNote : Note
    {
        public SoundEffectNote(SoundEffectNoteMeta meta) : base(meta)
        {
            TargetSound = meta.TargetSound;
            AudioParameter = meta.AudioParameter;
        }

        [Serializable]
        public class SoundEffectNoteMeta : NoteMeta
        {
            [ShowInInspector, EnumPaging] public SFXBundle.SFX TargetSound;
            public AudioParameter AudioParameter;

            public override FlowNode Build()
            {
                return new SoundEffectNote(this);
            }
            public SoundEffectNoteMeta() : base() { }
            public SoundEffectNoteMeta(SoundEffectNoteMeta rhs) : base(rhs)
            {
                TargetSound = rhs.TargetSound;
                AudioParameter = rhs.AudioParameter;
            }
            public override FlowNodeMeta DeepCopy()
            {
                return new SoundEffectNoteMeta(this);
            }
        }

        [EnumPaging] public SFXBundle.SFX TargetSound { get; private set; }
        public AudioParameter AudioParameter { get; private set; }

        public override FlowNode DeepCopy()
        {
            return new SoundEffectNote(MetaData as SoundEffectNoteMeta);
        }

        protected override void OnEnterPlay()
        {

            Debug.Log($"Play Sound");
            if (Application.isPlaying)
            {
                UltimateAudioManager.Instance.Play(TargetSound, new AudioParameter(AudioParameter.Volume, AudioParameter.Pitch, (float)CurrentTime));
            }
        }

        public override void OnPlay()
        {
        }
    }
}
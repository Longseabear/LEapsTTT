using Sirenix.OdinInspector;
using System;
using TTT.Audio;
using TTT.Rhythms;
using TTT.System;
using UnityEngine;
using TTT.Common;

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
                Length = 0;
            }
            public NoteMeta(NoteMeta rhs) : base(rhs)
            {
            }
        }

        protected Note(NoteMeta meta) : base(meta)
        {
        }

        public override void Initialize(ITimerable timer)
        {
            SetTimer(timer);
            Length = 0;
        }
        public override void Update()
        {
            if (State == NodeState.FINISH)
            {
                OnFinish();
                return;
            }

            if (Timer.ElapsedTime >= 0 && State == NodeState.IDLE)
            {
                ChangeState(NodeState.PLAYING);
                if(IsPlaying) OnPlay();
                ChangeState(NodeState.FINISH);
            }
            else
            {
                OnIdle();
            }
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
                Length = 0;
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

        public override void Initialize(ITimerable timer)
        {
            SetTimer(timer);
            Length = 0;
            ChangeState(NodeState.FINISH);
        }
        public override void Update()
        {
        }
        protected override void OnPlay()
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

        public override void Update()
        {
            if (State == NodeState.FINISH)
            {
                OnFinish();
                return;
            }

            // Idle => Play
            if (State == NodeState.IDLE && Timer.ElapsedTime > 0) ChangeState(NodeState.PLAYING);

            if (IsPlaying)
            {
                OnPlay();
            }
            else
            {
                OnIdle();
            }

            if (Length <= Timer.ElapsedTime) ChangeState(NodeState.FINISH);
        }
    }

    [Serializable]
    public class SoundEffectNote : Note, IRhythmProviderBindable
    {
        public SoundEffectNote(SoundEffectNoteMeta meta) : base(meta)
        {
            TargetSound = meta.TargetSound;
        }

        [Serializable]
        public class SoundEffectNoteMeta : NoteMeta
        {
            [EnumPaging] public SFXBundle.SFX TargetSound { get; private set; }

            public override FlowNode Build()
            {
                return new SoundEffectNote(this);
            }
            public SoundEffectNoteMeta() : base() { }
            public SoundEffectNoteMeta(SoundEffectNoteMeta rhs) : base(rhs)
            {
                TargetSound = rhs.TargetSound;
            }
            public override FlowNodeMeta DeepCopy()
            {
                return new SoundEffectNoteMeta(this);
            }
        }

        public override void Initialize(ITimerable timer) 
        {
            base.Initialize(timer);

            Rhythm = GenerateRhythm();
        }

        [EnumPaging] public SFXBundle.SFX TargetSound { get; private set; }

        public override FlowNode DeepCopy()
        {
            return new SoundEffectNote(MetaData as SoundEffectNoteMeta);
        }

        protected override void OnPlay()
        {
            UltimateAudioManager.Instance.Play(TargetSound);
            _provider.NotifyAll(Rhythm);
        }

        private IRhythmProvider _provider { get; set; }

        public void Bind(IRhythmProvider provider)
        {
            _provider = provider;
        }

        [ShowInInspector] public Rhythm Rhythm { get; private set; }

        [Button]
        private void BakeRhythm()
        {
            Rhythm = GenerateRhythm();
        }

        private Rhythm GenerateRhythm()
        {
            return new Rhythm(Timer.MakeSubTimer(0.0f), RhythmProperty.OneBeatLength, RhythmProperty.SimpleBeatCurve);
        }
    }
}
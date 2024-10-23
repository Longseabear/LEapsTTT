using Sirenix.OdinInspector;
using System;
using TTT.Audio;
using TTT.Common;
using TTT.Rhythms;
using TTT.System;
using UnityEngine;
using UnityEngine.Playables;
using static TTT.Node.SoundEffectNote;
using static TTT.Notes.FLowNodeVisualization;

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
        public class SoundEffectNoteMeta : NoteMeta, IAudioVisualizer
        {
            [ShowInInspector, EnumPaging] public AudioClip TargetSound;
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

            public AudioClip GetAudioClipForVisualize()
            {
                return TargetSound;
            }
        }

        [EnumPaging] public AudioClip TargetSound { get; private set; }
        public AudioParameter AudioParameter { get; private set; }

        public override FlowNode DeepCopy()
        {
            return new SoundEffectNote(MetaData as SoundEffectNoteMeta);
        }

        protected override void OnEnterPlay()
        {
            double targetTime = Length * Pivot - CurrentTime;
            double startTime = 0;
            if(targetTime < 0)
            {
                startTime = -targetTime;
                targetTime = 0;
            }
            double delay = targetTime;

            UltimateAudioManager.Instance.PlayWithDelay(TargetSound, new AudioParameter(AudioParameter.Volume, AudioParameter.Pitch, (float)startTime), delay);
        }

        public override void OnPlay()
        {
        }
    }

    [Serializable]
    public class BeatSignalNode : Note
    {
        public BeatSignalNode(BeatSignalNodeMeta meta) : base(meta)
        {
        }

        [Serializable]
        public class BeatSignalNodeMeta : NoteMeta
        {
            public override FlowNode Build()
            {
                return new BeatSignalNode(this);
            }
            public BeatSignalNodeMeta() : base() { }
            public BeatSignalNodeMeta(BeatSignalNodeMeta rhs) : base(rhs)
            {
            }
            public override FlowNodeMeta DeepCopy()
            {
                return new BeatSignalNodeMeta(this);
            }
        }

        public int TargetBeatIndex = 0;
        public double TargetTime = 0;
        protected override void InitializeInternal()
        {
            TargetTime = Length * Pivot;
            double targetGlobalTime = StartTime + TargetTime;
            TargetBeatIndex = (int)(Mathf.Floor((float)(targetGlobalTime / RhythmProperty.BaseMeasureLength) * RhythmProperty.NumBeatInMeasure));
        }

        public override void OnPlay()
        {
            if(CurrentTime > TargetTime)
            {
                UltimateRhythmManager.Instance.BeatUnits[TargetBeatIndex].Receive();
                ChangeState(NodeState.FINISH);
            }
        }

        public override FlowNode DeepCopy()
        {
            return new BeatSignalNode(MetaData as BeatSignalNodeMeta);

        }
    }
}
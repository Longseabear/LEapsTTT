using Sirenix.OdinInspector;
using System;
using TTT.Audio;
using TTT.Rhythm;
using TTT.System;

namespace TTT.Node
{

    // Length = 0. Note.
    [Serializable]
    public abstract class Note : FlowNode
    {
        public override void Initialize(ITimerable timer)
        {
            Timer = timer;
            Length = 0;
        }
        public override void Update()
        {
            if (State == NodeState.FINISH) return;

            if (Timer.ElapsedTime >= 0 && State == NodeState.IDLE)
            {
                ChangeState(NodeState.PLAYING);
                OnPlay();
                ChangeState(NodeState.FINISH);
            }
        }
    }

    [Serializable]
    public class SoundEffectNote : Note
    {
        [EnumPaging] public SFXBundle.SFX TargetSound;

        protected override void OnPlay()
        {
            UltimateAudioManager.Instance.Play(SFXBundle.SFX.drumsticks);
        }
    }
}
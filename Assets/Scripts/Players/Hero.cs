using System;
using TTT.Measures;

namespace TTT.Players
{
    [Serializable]
    public class Hero : Player
    {
        public Hero(PlayerMeta playerMeta) : base(playerMeta)
        {
        }

        public override Measure EvaluateAttackMeasure()
        {
            return SimpleAttackMeasure;
        }
    }
}

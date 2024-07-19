using System;
using TTT.Measures;

namespace TTT.Players
{
    [Serializable]
    public class Enemy : Player
    {
        public Enemy(PlayerMeta playerMeta) : base(playerMeta)
        {
        }
        public override Measure EvaluateAttackMeasure()
        {
            return SimpleAttackMeasure;
        }
    }
}

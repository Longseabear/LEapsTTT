using System;
using TTT.Rhythms.Measures;

namespace TTT.Players
{
    [Serializable]
    public class Enemy : Player
    {
        public Enemy(PlayerMeta playerMeta) : base(playerMeta)
        {
        }
        public override MeasureNode GetNextMeasure()
        {
            return SimplePlaceAction;
        }
    }
}

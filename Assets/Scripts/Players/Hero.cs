using System;
using TTT.Rhythms.Measures;

namespace TTT.Players
{
    [Serializable]
    public class Hero : Player
    {
        public Hero(PlayerMeta playerMeta) : base(playerMeta)
        {
        }

        public override MeasureNode GetNextMeasure()
        {
            return SimplePlaceAction;
        }
    }
}

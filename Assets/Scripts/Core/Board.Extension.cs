using System.Collections.Generic;
using System.Linq;

namespace TTT.Core
{
    public static class BoardExtensions
    {
        public static int GetWinner(this List<Cell> cells)
        {
            if (cells.Count < 3) return 0;
            int BasePlayer = cells[0].Player;
            if (BasePlayer == 0) return 0;

            if (cells.All(item => item.Player == BasePlayer)) return BasePlayer;
            return 0;
        }
    }
}

using System.Collections.Generic;
using TTT.Core;

namespace TTT.Core.Events
{
    public class GamePlayEvents
    {
        public class WinData : Event
        {
            public int WinPlayer = 0;
            public List<List<Cell>> WinningCells = new List<List<Cell>>();

            public void Append(List<Cell> cells)
            {
                WinningCells.Add(cells);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using TTT.Core.Events;
using UnityEngine;

namespace TTT.Core
{
    [Serializable]
    public class Board : ISubscriber<CellEvents.OnMouseDown>, ISubscriber<CellEvents.OnMouseEnter>, ISubscriber<CellEvents.OnMouseExit>
    {
        public static Vector2Int BoardMaxSize = new Vector2Int(5, 5);
        public static Vector2Int CenterPivot = new Vector2Int(2, 2);
        public static Vector2Int InitialOpened = new Vector2Int(3, 3);
        private Cell[,] _cells { get; set; }
        public Cell[,] Cells => _cells;

        public Publisher<CellEvents.OnMouseDown> CellClickEvent;

        public Board()
        {
            _cells = new Cell[BoardMaxSize.y, BoardMaxSize.x];

            foreach(var y in GetY())
            {
                foreach(var x in GetX())
                {
                    this[y, x] = new Cell(new Vector2Int(y, x));
                    this[y, x].IsEnabled = false;
                }
            }

            for(int i = -1; i <= 1; i++)
            {
                for(int j = -1; j <= 1; j++)
                {
                    this[i, j].IsEnabled = true;
                }
            }

            CellClickEvent = new Publisher<CellEvents.OnMouseDown>();

            // Event Subscribe
            foreach (var cell in _cells) cell.MouseEvents.SubscribeAll(this); 
        }

        public IEnumerable<int> GetX()
        {
            for (int i = 0; i < BoardMaxSize.x; i++) yield return i - CenterPivot.x;
        }
        public IEnumerable<int> GetY()
        {
            for (int i = 0; i < BoardMaxSize.y; i++) yield return i - CenterPivot.y;
        }

        public IEnumerable<Cell> GetAllCell()
        {
            foreach(int i in GetY())
            {
                foreach (int j in GetX())
                {
                    yield return this[i, j];
                }
            }
        }

        public void Recieve(CellEvents.OnMouseDown data)
        {
            CellClickEvent.Notify(data);
        }

        public void Recieve(CellEvents.OnMouseEnter data)
        {
        }

        public void Recieve(CellEvents.OnMouseExit data)
        {
        }

        public Cell this[int y, int x]
        {
            get
            {
                return _cells[y + CenterPivot.y, x + CenterPivot.x];
            }
            set
            {
                _cells[y + CenterPivot.y, x + CenterPivot.x] = value;
            }
        }
        public Cell this[Vector2Int index]
        {
            get
            {
                return _cells[index.y + CenterPivot.y, index.x + CenterPivot.x];
            }
            set
            {
                _cells[index.y + CenterPivot.y, index.x + CenterPivot.x] = value;
            }
        }

        public bool IsValidIndex(int y, int x) => y >= -CenterPivot.y && y <= CenterPivot.y && x >= -CenterPivot.x && x <= CenterPivot.x;

        public IEnumerable<Cell> GetActivatedVerticalCells(int x)
        {
            foreach(int y in GetY())
            {
                if (!this[y, x].IsEnabled) continue;
                yield return this[y, x];
            }
        }
        public IEnumerable<List<Cell>> GetAllActivatedVerticalCells()
        {
            foreach (int x in GetX()) yield return GetActivatedVerticalCells(x).ToList();
        }
        public IEnumerable<Cell> GetActivatedHorizontalCells(int y)
        {
            foreach (int x in GetX())
            {
                if (!this[y, x].IsEnabled) continue;
                yield return this[y, x];
            }
        }
        public IEnumerable<List<Cell>> GetAllActivatedHorizontalCells()
        {
            foreach (int y in GetY()) yield return GetActivatedHorizontalCells(y).ToList();
        }

        public IEnumerable<Cell> GetActivatedSlashCells(int b)
        {
            if (-BoardMaxSize.x < b && b <= 0)
            {
                for(int x = b, y = CenterPivot.y; x < b + BoardMaxSize.x; x++, y--)
                {
                    if (!IsValidIndex(y, x) || !this[y, x].IsEnabled) continue;
                    yield return this[y, x];
                }
            }
        }
        public IEnumerable<List<Cell>> GetAllActivatedSlashCells()
        {
            for (int x = -BoardMaxSize.x + 1; x <= 0; x++) yield return GetActivatedSlashCells(x).ToList();
        }
        public IEnumerable<Cell> GetActivatedBackSlashCells(int b)
        {
            if (0 <= b && b < BoardMaxSize.x)
            {
                for (int x = b, y = CenterPivot.y; x > b - BoardMaxSize.x; x--, y--)
                {
                    if (!IsValidIndex(y, x) || !this[y, x].IsEnabled) continue;
                    yield return this[y, x];
                }
            }
        }
        public IEnumerable<List<Cell>> GetAllActivatedBackSlashCells()
        {
            for (int x = 0; x < BoardMaxSize.x; x++) yield return GetActivatedBackSlashCells(x).ToList();
        }
        public GamePlayEvents.WinData CheckFinish()
        {
            var winData = new GamePlayEvents.WinData();
            winData.WinPlayer = 0;
            // Horizontal
            foreach(var cells in GetAllActivatedHorizontalCells())
            {
                int winner = cells.GetWinner();
                if(winner > 0)
                {
                    winData.WinPlayer = winner;
                    winData.Append(cells);
                }
            }
            foreach (var cells in GetAllActivatedVerticalCells())
            {
                int winner = cells.GetWinner();
                if (winner > 0)
                {
                    winData.WinPlayer = winner;
                    winData.Append(cells);
                }
            }
            foreach (var cells in GetAllActivatedSlashCells())
            {
                int winner = cells.GetWinner();
                if (winner > 0)
                {
                    winData.WinPlayer = winner;
                    winData.Append(cells);
                }
            }
            foreach (var cells in GetAllActivatedBackSlashCells())
            {
                int winner = cells.GetWinner();
                if (winner > 0)
                {
                    winData.WinPlayer = winner;
                    winData.Append(cells);
                }
            }
            return winData;
        }
    }
}

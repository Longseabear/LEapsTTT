using UnityEngine;

namespace TTT.Core.Events
{
    public static class CellEvents
    {
        public class OnMouseEnter : Event
        {
            public readonly Cell Cell;
            public readonly Vector2 MousePosition;
            public readonly float Time;
            public OnMouseEnter(Cell cell, Vector2 position, float time)
            {
                Cell = cell;
                MousePosition = position;
                Time = time;
            }
        }
        public class OnMouseExit : Event
        {
            public readonly Cell Cell;
            public readonly Vector2 MousePosition;
            public readonly float Time;
            public OnMouseExit(Cell cell, Vector2 position, float time)
            {
                Cell = cell;
                MousePosition = position;
                Time = time;
            }
        }
        public class OnMouseDown : Event
        {
            public readonly Cell Cell;
            public readonly Vector2 MousePosition;
            public readonly float Time;
            public OnMouseDown(Cell cell, Vector2 position, float time)
            {
                Cell = cell;
                MousePosition = position;
                Time = time;
            }
        }
    }
}

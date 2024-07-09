using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTT.Core.Events;
using UnityEngine;

namespace TTT.Core.Events
{
    public static class BoardEvents
    {   
        public class CellClickEvent : Event
        {
            public readonly Cell SelectedCell;
            public readonly float GlobalTime;

            public CellClickEvent(Cell cell, float globalTime)
            {
                SelectedCell = cell;
                GlobalTime = globalTime;
            }
        }
    }
}

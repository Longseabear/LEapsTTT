using System;
using System.Collections.Generic;
using System.Linq;
using TTT.Core;
using TTT.System;
using UnityEngine;

namespace TTT.Node
{
    [Serializable]
    public class RandomPlaceAction : PlaceAction
    {
        [Serializable]
        public class RandomPlaceActionMeta : PlaceActionMeta
        {
            public RandomPlaceActionMeta() : base() 
            {
                Length = 2.0f;
                Pivot = 0.5f;
            }
            public RandomPlaceActionMeta(PlaceActionMeta rhs) : base(rhs)
            {
            }

            public override FlowNode Build()
            {
                return new RandomPlaceAction(this);
            }

            public override FlowNodeMeta DeepCopy()
            {
                return new RandomPlaceActionMeta(this);
            }
        }
        public RandomPlaceAction(PlaceActionMeta meta) : base(meta)
        {
        }

        public override FlowNode DeepCopy()
        {
            throw new NotImplementedException();
        }
        private float _timingOffset { get; set; }

        protected override void OnEnterPlay()
        {
            float minDelta = Mathf.Min(1.0f - Pivot, Pivot);
            _timingOffset = UnityEngine.Random.Range(-Length * minDelta, Length * minDelta);
        }
        protected override void OnPlay()
        {
            if(State == NodeState.PLAYING && _pivotPosition + _timingOffset < Timer.ElapsedTime)
            {
                float score = GetScore();

                Board board = UltimateGamePlay.Instance.UIBoard.Board;
                List<Cell> freeCells = board.GetAllCell().Where(cell => cell.IsEnabled && !cell.IsAssigned).ToList();
                if (freeCells.Count > 0)
                {
                    var cell = freeCells[UnityEngine.Random.Range(0, freeCells.Count)];
                    var UIcell = UltimateGamePlay.Instance.UIBoard.CellToUICell[cell];

                    Vector3 spawnPosition = UIcell.transform.position;
                    PlaceSymbol(UIcell, spawnPosition, Color.blue);
                }
                ChangeState(NodeState.FINISH);
            }
        }
    }
}

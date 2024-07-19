using Sirenix.OdinInspector;
using System;
using TTT.Core;
using TTT.Core.Events;
using TTT.GmaeObject;
using TTT.Rhythms;
using TTT.System;
using UnityEngine;

using Debug = UnityEngine.Debug;

namespace TTT.Node
{
    [Serializable]
    public class SimplePlaceAction : PlaceAction, ISubscriber<CellEvents.OnMouseDown>
    {
        [Serializable]
        public class SimplePlaceActionMeta : PlaceActionMeta
        {
            public SimplePlaceActionMeta() : base()
            {
            }
            public SimplePlaceActionMeta(PlaceActionMeta rhs) : base(rhs)
            {
            }

            public override FlowNode Build()
            {
                return new SimplePlaceAction(this);
            }

            public override FlowNodeMeta DeepCopy()
            {
                return new SimplePlaceActionMeta(this);
            }
        }

        public SimplePlaceAction(PlaceActionMeta meta) : base(meta)
        {
        }

        protected override void OnEnterPlay()
        {
            UltimateGamePlay.Instance.UIBoard.Board.CellClickEvent.Subscribe(this);
        }
        protected override void OnEndPlay()
        {
            UltimateGamePlay.Instance.UIBoard.Board.CellClickEvent.Unsubscribe(this);
        }
        public override void OnPlay()
        {
        }

        public void Recieve(CellEvents.OnMouseDown data)
        {
            float SelectedTime = (float)CurrentTime;
            float score = GetScore();
            Debug.Log($"Global Selec Time {data.Time}, Local Select Time: {SelectedTime}, Clip duration: {Length}, Timing Score: {score}");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
            RaycastHit hit;

            if (UnityEngine.Physics.Raycast(ray, out hit))
            {
                var UIcell = UltimateGamePlay.Instance.UIBoard.CellToUICell[data.Cell];
                Vector3 spawnPosition = hit.point;
                PlaceSymbol(UIcell, spawnPosition);
            }

            ChangeState(NodeState.FINISH);
        }

        public override FlowNode DeepCopy()
        {
            return new SimplePlaceAction(MetaData as SimplePlaceActionMeta);
        }
    }
}

using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using TTT.Assets.Scripts.Map;
using TTT.Assets.Scripts.System;
using TTT.Core;
using TTT.Core.Events;
using TTT.GmaeObject;
using TTT.Rhythm;
using TTT.System;
using Unity.Burst.CompilerServices;
using UnityEngine;

namespace TTT.Node
{
    [Serializable]
    public class RandomPlaceAction : Segment
    {
        [ShowInInspector, ReadOnly] private float _pivotPosition;

        public Transform SymbolPrefab;

        public float Height = 1.5f;
        public float Speed = 5.0f;

        private float _TimingOffset = 0.0f;

        public override void Initialize(ITimerable timer)
        {
            SetTimer(timer);
            _pivotPosition = Pivot * Length;
        }

        protected override void OnEnterPlay()
        {
            float minDelta = Mathf.Min(1.0f - Pivot, Pivot);
            _TimingOffset = UnityEngine.Random.Range(-Length * minDelta, Length * minDelta);
        }
        protected override void OnPlayEnd()
        {
        }
        protected override void OnPlay()
        {
            if(State == NodeState.PLAYING && _pivotPosition + _TimingOffset >= Timer.ElapsedTime)
            {
                Debug.Log(State);
                float SelectedTime = Timer.ElapsedTime;
                float score = MathF.Abs(SelectedTime - _pivotPosition);

                Board board = UltimateGamePlay.Instance.UIBoard.Board;
                List<Cell> freeCells = board.GetAllCell().Where(cell => cell.IsEnabled && !cell.IsAssigned).ToList();
                if (freeCells.Count > 0)
                {
                    var cell = freeCells[UnityEngine.Random.Range(0, freeCells.Count)];
                    var UIcell = UltimateGamePlay.Instance.UIBoard.CellToUICell[cell];

                    Vector3 spawnPosition = UIcell.transform.position;
                    spawnPosition = new Vector3(spawnPosition.x + UnityEngine.Random.Range(-0.2f, 0.2f), Height, spawnPosition.z + UnityEngine.Random.Range(-0.2f, 0.2f));

                    var instance = UltimatePrefabManager.Instance.Instantiate<Symbol>(spawnPosition, Quaternion.identity);
                    
                    instance.GetComponent<Rigidbody>().velocity = Vector3.down * Speed;
                    instance.GetComponent<Rigidbody>().MovePosition(spawnPosition);
                    instance.GetComponent<Rigidbody>().MoveRotation(Quaternion.identity);

                    cell.Player = 2;

                    instance.GetComponent<MeshRenderer>().material.color = Color.blue;
                    Debug.Log("Finish");
                }
                ChangeState(NodeState.FINISH);
            }
        }
    }
}

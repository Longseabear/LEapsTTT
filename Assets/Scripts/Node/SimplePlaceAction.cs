using Sirenix.OdinInspector;
using System;
using TTT.Assets.Scripts.System;
using TTT.Core.Events;
using TTT.GmaeObject;
using TTT.Rhythm;
using TTT.System;
using Unity.VisualScripting;
using UnityEngine;

using Debug = UnityEngine.Debug;

namespace TTT.Node
{
    [Serializable]
    public class SimplePlaceAction : Segment, ISubscriber<CellEvents.OnMouseDown>
    {
        [ShowInInspector, ReadOnly] private bool _selected = false;
        [ShowInInspector, ReadOnly] private float _pivotPosition;

        public Transform SymbolPrefab;

        public float Height = 1.5f;
        public float Speed = 5.0f;


        public override void Initialize(ITimerable timer)
        {
            SetTimer(timer);
            _selected = false;
            _pivotPosition = Pivot * Length;
        }

        protected override void OnEnterPlay()
        {
            UltimateGamePlay.Instance.UIBoard.Board.CellClickEvent.Subscribe(this);
            _selected = false;
        }
        protected override void OnPlayEnd()
        {
            UltimateGamePlay.Instance.UIBoard.Board.CellClickEvent.Unsubscribe(this);
            _selected = true;
        }
        protected override void OnPlay()
        {
        }

        public void Recieve(CellEvents.OnMouseDown data)
        {
            _selected = true;
            float SelectedTime = Timer.ElapsedTime;
            float score = MathF.Abs(SelectedTime - _pivotPosition);
            Debug.Log($"Global Selec Time {data.Time}, Local Select Time: {SelectedTime}, Timing Score: {score}");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
            RaycastHit hit;

            if (UnityEngine.Physics.Raycast(ray, out hit))
            {
                Vector3 spawnPosition = hit.point + Vector3.up * Height;
                var instance = UltimatePrefabManager.Instance.Instantiate<Symbol>(spawnPosition, Quaternion.identity);

                instance.GetComponent<Rigidbody>().velocity = Vector3.down * Speed;

                //                instance.GetComponent<Rigidbody>().AddForce(Vector3.down * Speed);
                instance.GetComponent<Rigidbody>().MovePosition(spawnPosition);
                instance.GetComponent<Rigidbody>().MoveRotation(Quaternion.identity);
                instance.GetComponent<Renderer>().material.color = Color.red;

                data.Cell.Player = 1;
            }

            ChangeState(NodeState.FINISH);
        }
    }
}

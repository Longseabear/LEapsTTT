using Sirenix.OdinInspector;
using System;
using TTT.GmaeObject;
using TTT.Map;
using TTT.Players;
using TTT.Rhythms;
using TTT.System;

using UnityEngine;

namespace TTT.Node
{
    [Serializable]
    public abstract class PlaceAction : Segment, IPlayerBindable
    {
        public abstract class PlaceActionMeta : SegmentMeta
        {
            public PlaceActionMeta() : base() { }
            public PlaceActionMeta(PlaceActionMeta rhs) : base(rhs) { }
        }

        public PlaceAction(PlaceActionMeta meta) : base(meta)
        {
        }

        [ShowInInspector, ReadOnly] protected float _pivotPosition { get; set; }

        public Player Player { get; private set; }
        public void Bind(Player player)
        {
            Player = player;
        }

        public override void Initialize(ITimerable timer)
        {
            SetTimer(timer);
            _pivotPosition = Pivot * Length;
        }

        protected void PlaceSymbol(UICell targetUICell, Vector3 position, Color color)
        {
            position = GetPlacePosition(position);

            var instance = UltimatePrefabManager.Instance.Instantiate<Symbol>(position, Quaternion.identity);

            instance.GetComponent<Rigidbody>().velocity = Vector3.down * GetPlacePower();
            instance.GetComponent<Rigidbody>().MovePosition(position);
            instance.GetComponent<Rigidbody>().MoveRotation(Quaternion.identity);

            targetUICell.Cell.Player = Player.PlayerID;

            instance.GetComponent<MeshRenderer>().material.color = color;
        }

        protected float GetScore()
        {
            float SelectedTime = Timer.ElapsedTime;
            float score = MathF.Abs(SelectedTime - _pivotPosition);
            return score;
        }
        public Vector3 GetPlacePosition(Vector3 BasePosition)
        {
            return new Vector3(BasePosition.x + UnityEngine.Random.Range(Player.PlaceAttribute.PlaceNoiseIntensity.MinX, Player.PlaceAttribute.PlaceNoiseIntensity.MaxX),
                Player.PlaceAttribute.Height,
                BasePosition.z + UnityEngine.Random.Range(Player.PlaceAttribute.PlaceNoiseIntensity.MinY, Player.PlaceAttribute.PlaceNoiseIntensity.MaxY));
        }
        public float GetPlacePower()
        {
            return Player.PlaceAttribute.BaseSpeed;
        }
    }
}

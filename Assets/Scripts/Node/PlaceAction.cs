using Sirenix.OdinInspector;
using System;
using TTT.Actions;
using TTT.GmaeObject;
using TTT.Map;
using TTT.Physics;
using TTT.Players;
using TTT.Rhythms;
using TTT.System;

using UnityEngine;

namespace TTT.Node
{
    [Serializable]
    public abstract class PlaceAction : Segment
    {
        public abstract class PlaceActionMeta : SegmentMeta
        {
            public PlaceActionMeta() : base() { }
            public PlaceActionMeta(PlaceActionMeta rhs) : base(rhs) 
            {
            }
        }
        public Player Player => UltimateGamePlay.Instance.Attacker;

        protected float _pivotPosition { get; set; }

        public PlaceAction(PlaceActionMeta meta) : base(meta)
        {
        }

        protected override void InitializeInternal()
        {
            // Note: Playable Time not correct in initialization time. Just use timeclip information
            _pivotPosition = Pivot * (float)Length;
        }

        protected void PlaceSymbol(UICell targetUICell, Vector3 position)
        {
            position = GetPlacePosition(position);
           
            if(ParentMeasure != null)
            {
                ParentMeasure.OccurEvent(new SimplePlaceActionEvent(ParentMeasure, ParentMeasure.Runtime.Attacker, position, GetPlacePower()));
            }

            targetUICell.Cell.Player = ParentMeasure.Runtime.Attacker.PlayerID;
        }

        protected float GetScore()
        {
            float SelectedTime = (float)CurrentTime;
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

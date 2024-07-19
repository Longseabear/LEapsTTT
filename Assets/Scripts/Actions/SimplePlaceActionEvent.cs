using System;
using TTT.Measures;
using TTT.Physics;
using TTT.Players;
using UnityEngine;

namespace TTT.Actions
{
    [Serializable]
    public class SimplePlaceActionEvent : ActionEvent
    {
        public SimplePlaceActionEvent(Measure measure, Player player, Vector3 position, float power)
        {
            Measure = measure;
            Player = player;
            Position = position;
            Power = power;
        }

        public Measure Measure { get; }
        public Player Player { get; }
        public Vector3 Position { get; }
        public float Power { get; }

        public override void Execute()
        {
            var instance = Player.MakeSymbol();
            var targetRigidbody = new RigidbodyState();
            targetRigidbody.position = Position;
            targetRigidbody.rotation = Quaternion.identity;
            targetRigidbody.velocity = Vector3.down * Power;

            instance.Initialize(Measure, targetRigidbody);
        }

        public override void Undo()
        {
            throw new global::System.NotImplementedException();
        }
    }
}

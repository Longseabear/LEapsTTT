using OpenCover.Framework.Model;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TTT.Node;
using TTT.Rhythms;
using TTT.Rhythms.Measures;
using Unity.VisualScripting;

namespace TTT.Players
{
    [Serializable]
    public abstract partial class Player
    {
        [Title("Meta Information"), ShowInInspector]
        private PlayerMeta Meta { get; set; }

        [Title("Instance Property"), ShowInInspector]
        public PlaceActionMeasure SimplePlaceAction { get; private set; }

        [VerticalGroup("Property"), ShowInInspector]
        public float HP { get; private set; }
        [VerticalGroup("Property"), ShowInInspector]
        public float Power { get; private set; }
        [VerticalGroup("Property"), ShowInInspector]
        public int PlayerID { get; private set; }

        public PlaceAttribute PlaceAttribute { get; private set; }

        public Player(PlayerMeta playerMeta)
        {
            Meta = playerMeta;
        }

        public void Initialize()
        {
            Power = Meta.Power;
            HP = Meta.HP;
            
            // Action

            // Attribute
            PlaceAttribute = Meta.PlaceAttribute.Clone() as PlaceAttribute;
        }
        public void SetPlayerID(int id)
        {
            PlayerID = id;
        }

        public abstract MeasureNode GetNextMeasure();
    }

    // Turn
    public abstract partial class Player
    {
        [ShowInInspector, ReadOnly] private List<MeasureNode> _turnNodes;
        public ITimerable Timer { get; private set; }

        public float TotalLength => _turnNodes.Count == 0 ? 0 : _turnNodes.Max((node) => node.Length + node.StartTime);
        public void GamePlayStart(ITimerable timer, int playerID)
        {
            // Bake All
            SetPlayerID(playerID);
            Timer = timer;

            SimplePlaceAction = Meta.SimplePlaceActionMeta.Build(this, Timer.MakeSubTimer(0f)) as PlaceActionMeasure;
        }
        
        public void AddMeasure(MeasureNode Node)
        {
            // Must baked
            Node.Reset();
            _turnNodes.Add(Node);
        }
        public void TurnStart(float StartTime)
        {
            Timer.StartTime = StartTime;

            _turnNodes = new List<MeasureNode>();
            AddMeasure(GetNextMeasure());
        }

        public void Turn()
        {
            foreach (var node in _turnNodes)
            {
                node.Update();
            }
        }
        public bool TurnFinish() => Timer.ElapsedTime >= TotalLength;
    }

    public interface IPlayerBindable
    {
        void Bind(Player player);
        Player Player { get; }
    }
}

using Sirenix.OdinInspector;
using System;
using TTT.GmaeObject;
using TTT.Measures;
using TTT.Node;
using TTT.System;
using UnityEngine;
using UnityEngine.UIElements;

namespace TTT.Players
{
    [Serializable]
    public abstract partial class Player
    {
        [Title("Meta Information"), ShowInInspector]
        private PlayerMeta Meta { get; set; }

        [Title("Instance Property"), ShowInInspector]
        public PlaceAction SimplePlaceAction { get; private set; }

        [VerticalGroup("Property"), ShowInInspector]
        public float HP { get; private set; }
        [VerticalGroup("Property"), ShowInInspector]
        public float Power { get; private set; }
        [VerticalGroup("Property"), ShowInInspector]
        public int PlayerID { get; private set; }
        [VerticalGroup("Property"), ShowInInspector]
        public Color SymbolColor { get; private set; }

        public PlaceAttribute PlaceAttribute { get; private set; }
        
        // Measures
        public SimpleMeasure SimpleAttackMeasure;

        public Player(PlayerMeta playerMeta)
        {
            Meta = playerMeta;
        }

        public void Initialize()
        {
            Power = Meta.Power;
            HP = Meta.HP;
            
            // Attribute
            PlaceAttribute = Meta.PlaceAttribute.Clone() as PlaceAttribute;
            SimpleAttackMeasure = Meta.SimplePlaceActionMeta.Build() as SimpleMeasure;
            SymbolColor = Meta.SymbolColor;
        }
        public void SetPlayerID(int id)
        {
            PlayerID = id;
        }

        public abstract Measure EvaluateAttackMeasure();

        public Symbol MakeSymbol()
        {
            var instance = UltimatePrefabManager.Instance.Instantiate<Symbol>();
            instance.GetComponent<Renderer>().material.color = SymbolColor;
            return instance;
        }
    }

    // Turn

    public interface IPlayerBindable
    {
        void Bind(Player player);
        Player Player { get; }
    }
}

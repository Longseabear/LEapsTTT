using System;
using TTT.Measures;
using TTT.Node;
using UnityEngine;

namespace TTT.Players
{
    [CreateAssetMenu(fileName = "NewPlayer", menuName = "Player"), Serializable]
    public class PlayerMeta : ScriptableObject
    {
        public int HP;
        public float Power;
        public Color SymbolColor;

        public PlaceAttribute PlaceAttribute;
        [SerializeReference] public SimpleMeasureMeta SimplePlaceActionMeta;
    }
}
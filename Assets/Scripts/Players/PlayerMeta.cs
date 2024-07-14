using System;
using TTT.Rhythms.Measures;
using UnityEngine;

namespace TTT.Players
{
    [CreateAssetMenu(fileName = "NewPlayer", menuName = "Player"), Serializable]
    public class PlayerMeta : ScriptableObject
    {
        public int HP;
        public float Power;

        public PlaceAttribute PlaceAttribute;

        [SerializeReference] public PlaceActionMeasure.PlaceActionMeasureMeta SimplePlaceActionMeta;
    }
}
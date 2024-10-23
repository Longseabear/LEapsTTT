using System.Collections.Generic;
using UnityEngine;

namespace TTT.Rhythms
{
    public class RhythmProducer : MonoBehaviour
    {
        public List<IRhythmHandler> handler = new List<IRhythmHandler>();
        public int Count { get; private set; }

        public void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            Count = 0;
        }

        public void Update()
        {

        }
    }
}

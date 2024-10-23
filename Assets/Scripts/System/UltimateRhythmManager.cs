using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TTT.Common;
using TTT.Rhythms;
using UnityEngine;

namespace TTT.System
{
    public class UltimateRhythmManager : MonoBehaviour
    {
        [Header("Instance")]
        public static UltimateRhythmManager Instance;

        [Serializable]
        public class BeatUnit
        {
            [ShowInInspector] public List<IRhythmHandler> Handlers = new List<IRhythmHandler>();
            public BeatUnit()
            {
                Initialize();
            }

            public void Initialize()
            {
                Handlers = new List<IRhythmHandler>();
            }
            public void Register(IRhythmHandler handler)
            {
                Handlers.Add(handler);
            }
            public void Unregister(IRhythmHandler handler)
            {
                Handlers.Remove(handler);
            }
            public void Receive()
            {
                foreach (var handler in Handlers)
                {
                    handler.Receive();
                }
            }
        }

        public BeatUnit[] BeatUnits = new BeatUnit[Constant.NUM_BEAT_ON_MEASURE];


        [Serializable]
        public class BeatObjectInfo
        {
            public GameObject GameObject;
            public int MeasureIndex;
        }

        public List<BeatObjectInfo> InitialBeatObjectInfo;

        public void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            foreach (var beatUnit in BeatUnits)
            {
                beatUnit.Initialize();
            }
            foreach (var item in InitialBeatObjectInfo)
            {
                BeatUnits[item.MeasureIndex].Register(item.GameObject.GetComponent<IRhythmHandler>());
            }
        }
    }
}

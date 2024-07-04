using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TTT.Node;
using TTT.Rhythm;
using Unity.VisualScripting;
using UnityEngine;

namespace TTT.Assets.Scripts.Rhythm
{
    [Serializable]
    public class NodeProcessor
    {
        [InlineProperty, SerializeReference] public List<MeasureNode> Turns = new List<MeasureNode>();

        // Actions
        public Action<FlowNode> OnNodeChanged { get; set; } = delegate { };
        public FlowNode CurrentProcessedNode { get; private set; }

        [SerializeField] public TTT.Rhythm.Timer Timer;

        private float TotalLength = 0;

        public void Initialize()
        {
            float BaseStartTime = 0;
            TotalLength = 0;
            foreach (var turn in Turns)
            {
                turn.Register(null);

                turn.Initialize((Timer as ITimerable).MakeSubTimer(BaseStartTime));
                BaseStartTime += turn.Length;

                TotalLength += turn.Length;
            }
        }
        public void Update()
        {
            for(int i = 0; i < Turns.Count; i++)
            {
                Turns[i].Update();
                if (Turns[i].IsFinish)
                {
                    Turns[i].Timer.StartTime += TotalLength;
                    Turns[i].Reset();
                }

                if(Turns[i].Timer.StartTime <= Timer.ElapsedTime && Timer.ElapsedTime < Turns[i].Timer.StartTime + Turns[i].Length)
                {
                    if(CurrentProcessedNode != Turns[i])
                    {
                        CurrentProcessedNode = Turns[i];
                        OnNodeChanged?.Invoke(CurrentProcessedNode);
                    }
                }
            }
        }
    }
}

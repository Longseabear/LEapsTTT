using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TTT.Node;
using TTT.Players;
using TTT.Rhythms.Measures;
using UnityEngine;

namespace TTT.Rhythms
{
    [Serializable]
    public class NodeProcessor
    {
        [SerializeReference] public Player Player1;
        [SerializeReference] public Player Player2;

        [ShowInInspector, ReadOnly] public Player CurrentPlayer { get; private set; }
        public Player GetNextPlayer => CurrentPlayer == Player1 ? Player2 : Player1;

        [SerializeReference] public List<MeasureProxy> Turns = new List<MeasureProxy>();

        // Actions
        public Action<FlowNode> OnNodeChanged { get; set; } = delegate { };
        public FlowNode CurrentProcessedNode { get; private set; }

        [SerializeField] public TTT.Rhythms.Timer Timer;

        private float TotalLength = 0;

        public void Initialize(Player player1, Player player2)
        {
            Player1 = player1;
            Player2 = player2;

            Player1.Initialize();
            Player2.Initialize();
            Player1.GamePlayStart((Timer as ITimerable).MakeSubTimer(0), 1);
            Player2.GamePlayStart((Timer as ITimerable).MakeSubTimer(0), 2);

            float BaseStartTime = 0;
            TotalLength = 0;

            foreach (var turn in Turns)
            {
                turn.Get().Register(null);
                turn.Get().Initialize((Timer as ITimerable).MakeSubTimer(BaseStartTime));
                BaseStartTime += turn.Get().Length;
                TotalLength += turn.Get().Length;
            }

            CurrentPlayer = Player1;
            CurrentPlayer.TurnStart(0f);
        }
        public void Update()
        {
            if (CurrentPlayer.TurnFinish())
            {
                var nextPlayer = GetNextPlayer;
                nextPlayer.TurnStart(CurrentPlayer.Timer.StartTime + CurrentPlayer.TotalLength);
                CurrentPlayer = nextPlayer;
            }
            CurrentPlayer.Turn();

            // Background action
            for (int i = 0; i < Turns.Count; i++)
            {
                Turns[i].Get().Update();
                if (Turns[i].Get().IsFinish)
                {
                    Turns[i].Get().SetStartTime(Turns[i].Get().StartTime + TotalLength);
                    Turns[i].Get().Reset();
                }

                if(Turns[i].Get().StartTime <= Timer.ElapsedTime && Timer.ElapsedTime < Turns[i].Get().StartTime + Turns[i].Get().Length)
                {
                    if(CurrentProcessedNode != Turns[i].Get())
                    {
                        CurrentProcessedNode = Turns[i].Get();
                        OnNodeChanged?.Invoke(CurrentProcessedNode);
                    }
                }
            }
        }
    }
}

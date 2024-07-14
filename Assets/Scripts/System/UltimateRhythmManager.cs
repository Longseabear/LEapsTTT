using System.Collections.Generic;
using TTT.Rhythms;
using UnityEngine;

namespace TTT.System
{
    public class UltimateRhythmManager : MonoBehaviour, IRhythmProvider, IRhythmHandler, IRhythmProviderBindable
    {
        [Header("Instance")]
        public static UltimateRhythmManager Instance;
        public IRhythmProvider MainRhythm { get; private set; }

        public List<IRhythmHandler> RhythmHandlers { get; private set; }

        public Rhythm Rhythm => MakeRhythm();

        private Rhythm MakeRhythm()
        {
            return MainRhythm.Rhythm;
        }

        public void Bind(IRhythmProvider mainRhythm)
        {
            MainRhythm.Unsubscribe(this);
            MainRhythm = mainRhythm;
            MainRhythm.Subscribe(this);
        }

        public void NotifyAll(Rhythm rhythm)
        {
            foreach (var handler in RhythmHandlers)
            {
                handler.Receive(rhythm);
            }
        }

        public void Subscribe(IRhythmHandler handler)
        {
            RhythmHandlers.Add(handler);
        }

        public void Unsubscribe(IRhythmHandler handler)
        {
            RhythmHandlers.Remove(handler);
        }

        public void Receive(Rhythm rhythm)
        {
            foreach (var handler in RhythmHandlers)
            {
                handler.Receive(rhythm);
            }
        }
    }
}

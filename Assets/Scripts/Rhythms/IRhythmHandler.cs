using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using TTT.Node;
using UnityEngine;

namespace TTT.Rhythms
{
    [Serializable]
    public struct Rhythm
    {
        public readonly ITimerable Timer;
        public float Duration;
        public AnimationCurve Curve;
        public Rhythm(ITimerable timer, float duration, AnimationCurve curve) : this()
        {
            Timer = timer;
            Duration = duration;
            Curve = curve;
        }

        public FlowNode.NodeState EvaluateState()
        {
            if (Timer.ElapsedTime > Duration) return FlowNode.NodeState.FINISH;
            else if (Timer.ElapsedTime >= 0) return FlowNode.NodeState.PLAYING;
            return FlowNode.NodeState.IDLE;
        }
        public float Evaluate()
        {
            return Curve.Evaluate(Timer.ElapsedTime / Duration);
        }
    }
    public interface IRhythmHandler
    {
        public void Receive(Rhythm ryhthm);
    }
    public interface IRhythmable
    {
        public Rhythm Rhythm { get; }
    }
    public interface IRhythmProvider : IRhythmable
    {
        public List<IRhythmHandler> RhythmHandlers { get;}

        public void Subscribe(IRhythmHandler register);
        public void Unsubscribe(IRhythmHandler register);
        public void NotifyAll(Rhythm rhythm);
    }
    public interface IRhythmProviderBindable : IRhythmable
    {
        public void Bind(IRhythmProvider provider);
    }
}

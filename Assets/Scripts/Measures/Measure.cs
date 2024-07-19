using System;
using System.Collections.Generic;
using TTT.Actions;
using TTT.Players;
using TTT.Rhythms;
using TTT.System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TTT.Measures
{
    [Serializable]
    public class MeasureRuntimeData
    {
        public ITimerable Timer { get; private set; }
        public Player Attacker { get; private set; }
        public Player Defender { get; private set; }

        public List<ActionEvent> Events { get; private set; }

        public MeasureRuntimeData(ITimerable timer, Player attacker, Player defender)
        {
            Timer = timer;
            Attacker = attacker;
            Defender = defender;
            Events = new List<ActionEvent>();
        }
    }

    [Serializable]
    public abstract class Measure
    {
        public MeasureRuntimeData Runtime { get; set; }

        public Measure(MeasureMeta meta)
        {
        }
        public abstract void RegisterSimulation(MeasureRuntimeData timer);
        public abstract void UnregisterSimulation();

        public Measure NextMeasure { get; set; }


        public void OccurEvent(ActionEvent @event)
        {
            Runtime.Events.Add(@event);
            @event.Execute();
        }
    }

    public class SimpleMeasure : Measure
    {
        public TimelineAsset Assets;
        private MeasureDirector _measureDirector { get; set; }
        private DirectorWrapMode Mode {get; set;}

        public SimpleMeasure(SimpleMeasureMeta meta) : base(meta)
        {
            Assets = meta.Assets;
            Mode = meta.Mode;
        }

        public override void RegisterSimulation(MeasureRuntimeData measureRuntimeData)
        {
            Runtime = measureRuntimeData;

            _measureDirector = UltimatePrefabManager.Instance.Instantiate<MeasureDirector>();
            _measureDirector.Initialize(this, new MeasureDirector.MeasureDirectorMeta(true, Assets));
            _measureDirector.PlayableDirector.extrapolationMode = Mode;
            UltimateSimulationManager.Instance.Register(_measureDirector);
        }

        public override void UnregisterSimulation()
        {
            UltimatePrefabManager.Instance.Release(_measureDirector);
            UltimateSimulationManager.Instance.Unregister(_measureDirector);
            _measureDirector = null;
        }

    }
}

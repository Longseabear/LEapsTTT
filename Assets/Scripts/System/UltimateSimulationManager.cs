using JetBrains.Annotations;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Threading;
using TTT.Assets.Scripts.Common;
using TTT.Common;
using TTT.Measures;
using TTT.Players;
using TTT.Rhythms;
using TTT.Simulation;
using UnityEngine;
using Timer = TTT.Rhythms.Timer;

namespace TTT.System
{

    [Serializable]
    public class SimulationParam
    {
        public ITimerable MainTimer;
        public ManualTimer LocalTimer;
        [SerializeReference] public List<ISimulationable> SimulatableInstance;

        public long ProcessedFrame;
        public long ProcessedMeasure;

        public float NextFrameTime => (ProcessedFrame + 1) * UltimateSimulationManager.OneStepDeltaTime;

        public List<Measure> Measure;

        public SimulationParam(ITimerable mainTimer)
        {
            SimulatableInstance = new List<ISimulationable>();
            LocalTimer = new ManualTimer();
            ProcessedFrame = 0;
            ProcessedMeasure = 0;
            MainTimer = mainTimer;
            Measure = new List<Measure>();
        }

        public Measure CurrentMeasure => Measure[(int)ProcessedMeasure];
        public float MeasureStartTIme => ProcessedMeasure * UltimateSimulationManager.OneMeasureBaseTime;
    }

    [RequireComponent(typeof(Timer)), DefaultExecutionOrder(UpdateOrderConstant.SCRIPT_SIMULATOR_ORDER)]
    public class UltimateSimulationManager : MonoBehaviour
    {
        public static UltimateSimulationManager Instance { get; private set; }

        public static float OneStepDeltaTime = 0.02f;
        public static float OneMeasureBaseTime = 2f;
        [ShowInInspector, ShowIf("@this.SimulationParam != null")] public SimulationParam SimulationParam { get; private set; }

        public List<MeasureMeta> DefaultMeasures = new List<MeasureMeta>();
        public Timer Timer;

        public void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            UnityEngine.Physics.simulationMode = SimulationMode.Script;

            Timer = GetComponent<Timer>();
        }
        [Button("Set Default Simulation Param")]
        public void InitializeSimulationParam()
        {
            SimulationParam = new SimulationParam(GetComponent<Timer>());
        }
        public void Initialize(Player player, Player player2)
        {
            if (SimulationParam == null) InitializeSimulationParam();

            UltimateGamePlay.Instance.Attacker = player;
            UltimateGamePlay.Instance.Defender = player2;
            UltimateGamePlay.Instance.Attacker.Initialize();
            UltimateGamePlay.Instance.Defender.Initialize();
            player.SetPlayerID(1);
            player2.SetPlayerID(2);

            SimulationParam.Measure.Add(UltimateGamePlay.Instance.Attacker.EvaluateAttackMeasure());
            SimulationParam.CurrentMeasure.RegisterSimulation(new MeasureRuntimeData(((ITimerable)SimulationParam.LocalTimer).MakeSubTimer(0.0f), player, player2));
            UltimateGamePlay.Instance.CurrentMeasure = SimulationParam.CurrentMeasure;

            foreach(var measureMeta in DefaultMeasures)
            {
                var measure = measureMeta.Build();
                measure.RegisterSimulation(new MeasureRuntimeData(((ITimerable)SimulationParam.MainTimer).MakeSubTimer(0.0f), null, null));
            }
        }

        public void Register(ISimulationable simulationable)
        {
            SimulationParam.SimulatableInstance.Add(simulationable);
        }
        public void Unregister(ISimulationable simulationable)
        {
            SimulationParam.SimulatableInstance.Remove(simulationable);
        }

        // Measure 단위로 계속 진행되는 구조
        // Measure => Measure 생성 => ...
        // Measure Unit은 2s.. 

        public void OneFrameSimulation()
        {
            // currentMeeasureIndex
            long currentMeasureIndex = (long)Mathf.Floor(SimulationParam.NextFrameTime / OneMeasureBaseTime);
            if (SimulationParam.ProcessedMeasure < currentMeasureIndex)
            {
                // 평가 & 새로운 Measure 추가
                // 1) 이벤트가 없으면 플레이어 교대 후 선택
                if (SimulationParam.CurrentMeasure.NextMeasure != null)
                {
                    SimulationParam.Measure.Add(SimulationParam.CurrentMeasure.NextMeasure);
                }
                else
                {
                    UltimateGamePlay.Instance.PlayerSwap();
                    SimulationParam.Measure.Add(UltimateGamePlay.Instance.Attacker.EvaluateAttackMeasure());
                }
                SimulationParam.CurrentMeasure.UnregisterSimulation();
                SimulationParam.ProcessedMeasure++;
                SimulationParam.CurrentMeasure.RegisterSimulation(new MeasureRuntimeData(((ITimerable)SimulationParam.LocalTimer).MakeSubTimer(SimulationParam.MeasureStartTIme),
                    UltimateGamePlay.Instance.Attacker, UltimateGamePlay.Instance.Defender));
            }

            SimulationParam.LocalTimer.Set(SimulationParam.NextFrameTime);
            foreach (var instance in SimulationParam.SimulatableInstance) instance.Simulate();
            UnityEngine.Physics.Simulate(OneStepDeltaTime);
        }
        public void FixedUpdate()
        {
            if (SimulationParam == null) return;

            // frame index
            long elapsedFrame = (long)Mathf.Floor(SimulationParam.MainTimer.ElapsedTime / (float)OneStepDeltaTime);

            // Update version
            //for (; SimulationParam.ProcessedFrame < elapsedFrame; SimulationParam.ProcessedFrame++)
            //{
            //    OneFrameSimulation();
            //}
           
            // Fixed Update version
            OneFrameSimulation();
            SimulationParam.ProcessedFrame++;
        }
    }
}

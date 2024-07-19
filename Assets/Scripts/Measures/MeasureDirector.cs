using System;
using TTT.Rhythms;
using TTT.Simulation;
using TTT.System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TTT.Measures
{
    [RequireComponent(typeof(PlayableDirector))]
    public class MeasureDirector : PrefabPoolable, ISimulationable
    {
        [Serializable]
        public struct MeasureDirectorMeta
        {
            public bool IsPlaying;
            public TimelineAsset Measure;

            public MeasureDirectorMeta(bool isPlaying, TimelineAsset measure)
            {
                IsPlaying = isPlaying;
                Measure = measure;
            }
            public MeasureDirectorMeta(MeasureDirector director)
            {
                IsPlaying = director.IsPlaying;
                Measure = director.PlayableDirector.playableAsset as TimelineAsset;
            }

            public void Restore(MeasureDirector director)
            {
                director.IsPlaying = IsPlaying;
                director.PlayableDirector.playableAsset = Measure;
            }
        }

        public PlayableDirector PlayableDirector { get; private set; }

        public Measure ParentMeasure { get; private set; }

        public bool IsPlaying { get; private set; }

        private void Awake()
        {
            PlayableDirector = GetComponent<PlayableDirector>();
            PlayableDirector.timeUpdateMode = DirectorUpdateMode.Manual;
            IsPlaying = false;
        }

        public void Initialize(Measure measure, MeasureDirectorMeta MeasureDirectorMeta)
        {
            ParentMeasure = measure;

            if(PlayableDirector.playableAsset != null)
            {
                PlayableDirector.Stop();
            }

            IsPlaying = MeasureDirectorMeta.IsPlaying;

            PlayableDirector.playableAsset = MeasureDirectorMeta.Measure;
            PlayableDirector.time = ParentMeasure.Runtime.Timer.ElapsedTime;

            // In Playtime, Called OnPlayableCreate() and set node director.
            PlayableDirector.Play();
        }

        public void Simulate()
        {
            if (PlayableDirector.extrapolationMode == DirectorWrapMode.Loop)
            {
                PlayableDirector.time = ParentMeasure.Runtime.Timer.ElapsedTime % PlayableDirector.duration;
            }
            else
            {
                PlayableDirector.time = ParentMeasure.Runtime.Timer.ElapsedTime;
            }
            PlayableDirector.Evaluate();
        }

        public override void OnReleased()
        {
            PlayableDirector.Stop();
        }

        public override object Save()
        {
            return new MeasureDirectorMeta(this);
        }

        public override void Restore(object parameter)
        {
            throw new NotImplementedException("MeasureDirector must be initialized with Measure. Use initialize()");
        }
    }
}

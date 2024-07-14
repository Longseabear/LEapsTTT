using DG.Tweening;
using Sirenix.OdinInspector;
using TTT.Rhythms;
using UnityEngine;
using UnityEngine.Timeline;

namespace TTT.GmaeObject
{
    public class BeatDoll : MonoBehaviour, IRhythmHandler
    {
        private Tweener _scaleTweener;
        private Rhythm _currentRhythm;
        public Vector3 TargetScale;
        public Vector3 OriginalScale;


        public TimelineAsset testAsset;

        public void Awake()
        {
            OriginalScale = transform.localScale;
        }

        [Button("Simulation")]
        public void Receive(Rhythm rhythm)
        {
            _currentRhythm = rhythm;

            // Stop any existing tweener
            if (_scaleTweener != null && _scaleTweener.IsPlaying())
            {
                _scaleTweener.Kill();
            }

            Vector3 initialScale = transform.localScale;
            Vector3 targetScale = TargetScale; // 원하는 목표 크기로 설정

            // Stop any existing tweener
            if (_scaleTweener != null && _scaleTweener.IsPlaying())
            {
                _scaleTweener.Kill();
            }

            // Start a new tweener
            _scaleTweener = DOTween.To(() => 0f, value =>
            {
                float curveValue = _currentRhythm.Evaluate();
                transform.localScale = Vector3.Lerp(initialScale, targetScale, curveValue);

                if (_currentRhythm.EvaluateState() == Node.FlowNode.NodeState.FINISH)
                {
                    _scaleTweener.Kill();
                }
            }, 1f, _currentRhythm.Duration)
            .SetEase(Ease.Linear);
        }

    }
}

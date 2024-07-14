using Sirenix.OdinInspector;
using System.Collections;
using Unity.EditorCoroutines.Editor;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UIElements;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TTT.CameraUtils
{
    public class CameraPath : MonoBehaviour
    {
        public Transform Target;
        public Transform OriginalPosition;

        [Serializable]
        public class TargetPoint{
            public Transform Target;

            [ShowIf("IsTargetPoint"), VerticalGroup("Property")]
            public float Duration = 0.5f;

            [ShowIf("IsTargetPoint"), VerticalGroup("Property")]
            public float BazierControlWeight = 0.5f;

            [ShowIf("IsTargetPoint"), VerticalGroup("Property")]
            public float AfterDelay;

            [ShowIf("IsTargetPoint"), VerticalGroup("TimeScale")]
            public Ease TimeEasy = Ease.Linear;
            [ShowIf("IsTargetPoint"), VerticalGroup("TimeScale")]
            [ReadOnly] public AnimationCurve easeCurve;

            [HideInInspector]
            public bool IsTargetPoint = true;

            public void GenerateEaseCurve()
            {
                easeCurve = new AnimationCurve();
                for (int i = 0; i <= 100; i++)
                {
                    float t = i / 100f;
                    float value = DOVirtual.EasedValue(0f, 1f, t, TimeEasy);
                    easeCurve.AddKey(t, value);
                }
            }

            public object Clone()
            {
                var point = new TargetPoint();
                point.AfterDelay = AfterDelay;
                point.easeCurve = easeCurve; 
                point.BazierControlWeight = BazierControlWeight;
                point.Duration = Duration;
                point.Target = Target;
                point.TimeEasy = TimeEasy;

                return point;
            }
        }

        [TableList, OnValueChanged("OnTargetPositionChanged")]
        public List<TargetPoint> TargetPositions = new List<TargetPoint>();

        private void OnTargetPositionChanged()
        {
            if (TargetPositions.Count > 0) TargetPositions[0].IsTargetPoint = false;
            for(int i=1;i<TargetPositions.Count;i++) { TargetPositions[i].IsTargetPoint = true; };

            if (TargetPositions.Count > lastCount)
            {
                for (int i = lastCount; i < TargetPositions.Count; i++)
                {

                    if(i-1 >= 0 && TargetPositions[i-1] != null)
                    {
                        TargetPositions[i] = TargetPositions[i - 1].Clone() as TargetPoint;
                        GameObject newPoint = new GameObject("PathPoint_" + i);
                        newPoint.transform.parent = this.transform;
                        newPoint.transform.localPosition = TargetPositions[i].Target.transform.localPosition;
                        newPoint.transform.rotation = TargetPositions[i].Target.transform.rotation;
                        TargetPositions[i].Target = newPoint.transform;
                    }else if (TargetPositions[i] == null)
                    {
                        GameObject newPoint = new GameObject("PathPoint_" + i);
                        newPoint.transform.parent = this.transform;
                        newPoint.transform.localPosition = Vector3.zero;
                        TargetPositions[i].Target = newPoint.transform;
                    }
                }
            }
            else if (TargetPositions.Count < lastCount)
            {
                for (int i = TargetPositions.Count; i < lastCount; i++)
                {
                    Transform pointToRemove = transform.Find("PathPoint_" + i);
                    if (pointToRemove != null)
                    {
                        DestroyImmediate(pointToRemove.gameObject);
                    }
                }
            }

            lastCount = TargetPositions.Count;
        }

        private int lastCount = 0;
        private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            Vector3 p = uuu * p0; // (1-t)^3 * p0
            p += 3 * uu * t * p1; // 3 * (1-t)^2 * t * p1
            p += 3 * u * tt * p2; // 3 * (1-t) * t^2 * p2
            p += ttt * p3;        // t^3 * p3

            return p;
        }

        private (Vector3, Vector3) CalculateControlPoint(Transform from, Transform to, float ControlPointWeight)
        {
            Vector3 controlPoint1 = from.position - from.forward * ControlPointWeight;
            Vector3 controlPoint2 = to.position - to.forward * ControlPointWeight;

            return (controlPoint1, controlPoint2);
        }

        void OnDrawGizmos()
        {
           
            for (int i = 0; i < TargetPositions.Count; i++)
            {
                if (TargetPositions[i] != null && TargetPositions[i].Target != null)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(TargetPositions[i].Target.position, 0.1f);
                    Gizmos.DrawWireSphere(TargetPositions[i].Target.position, 0.2f);

                    // Index
                    UnityEditor.Handles.color = Color.white;
                    UnityEditor.Handles.Label(TargetPositions[i].Target.position + Vector3.up * 0.2f, i.ToString());

                    // Direction
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(TargetPositions[i].Target.position, TargetPositions[i].Target.position + TargetPositions[i].Target.forward * 0.5f);
                    Gizmos.DrawRay(TargetPositions[i].Target.position + TargetPositions[i].Target.forward * 0.5f, Quaternion.Euler(0, 30, 0) * -TargetPositions[i].Target.forward * 0.2f);
                    Gizmos.DrawRay(TargetPositions[i].Target.position + TargetPositions[i].Target.forward * 0.5f, Quaternion.Euler(0, -30, 0) * -TargetPositions[i].Target.forward * 0.2f);
                }
            }

            for(int i = 0; i < TargetPositions.Count - 1; i++)
            {
                if (TargetPositions[i] == null || TargetPositions[i].Target == null ||
                    TargetPositions[i+1] == null || TargetPositions[i+1].Target == null) continue;

                Vector3 startPosition = TargetPositions[i].Target.position;
                Vector3 endPosition = TargetPositions[i+1].Target.position;
                Vector3 startDirection = TargetPositions[i].Target.forward;
                Vector3 endDirection = TargetPositions[i + 1].Target.forward;

                (Vector3 controlPoint1, Vector3 controlPoint2) = CalculateControlPoint(TargetPositions[i].Target, TargetPositions[i + 1].Target, TargetPositions[i+1].BazierControlWeight);

                Vector3 previousPoint = startPosition;
                Gizmos.color = Color.green;
                for (int j = 1; j <= 20; j++) // Resolution
                {
                    float t = j / 20f;
                    Vector3 currentPoint = CalculateBezierPoint(t, startPosition, controlPoint1, controlPoint2, endPosition);
                    Gizmos.DrawLine(previousPoint, currentPoint);
                    previousPoint = currentPoint;
                }
            }
        }

        private Vector3 initialPosition;
        private Quaternion initialRotation;
        public float Delay = 0.5f;

        private bool isRunning = false;
        private void OnValidate()
        {
            foreach(var position in TargetPositions)
            {
                position.GenerateEaseCurve();
            }
        }


#if UNITY_EDITOR
        private EditorCoroutine currentCoroutine;

        [Button]
        public void StartChangingValue()
        {
            if (!isRunning && Target != null)
            {
                initialPosition = Target.transform.position;
                initialRotation = Target.transform.rotation;
                currentCoroutine = EditorCoroutineUtility.StartCoroutine(ChangeValueOverTime(), this);
                isRunning = true;
            }
        }
        private IEnumerator ChangeValueOverTime()
        {

            for(int i = 1; i < TargetPositions.Count; i++)
            {
                double startTime = EditorApplication.timeSinceStartup;
                double elapsedTime = 0f;

                Vector3 startPosition = TargetPositions[i-1].Target.position;
                Vector3 endPosition = TargetPositions[i].Target.position;
                Quaternion startRotation = TargetPositions[i - 1].Target.rotation;
                Quaternion endRotation = TargetPositions[i].Target.rotation;
                Vector3 startDirection = TargetPositions[i-1].Target.forward;
                Vector3 endDirection = TargetPositions[i].Target.forward;

                (Vector3 controlPoint1, Vector3 controlPoint2) = CalculateControlPoint(TargetPositions[i-1].Target, TargetPositions[i].Target, TargetPositions[i].BazierControlWeight);

                while (elapsedTime < TargetPositions[i].Duration)
                {
                    elapsedTime = EditorApplication.timeSinceStartup - startTime;
                    float t = DOVirtual.EasedValue(0f, 1f, Mathf.Clamp01((float)(elapsedTime / TargetPositions[i].Duration)), TargetPositions[i].easeCurve);

                    Target.transform.position = CalculateBezierPoint(t, startPosition, controlPoint1, controlPoint2, endPosition);
                    Target.transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);

                    yield return null;
                }
                yield return new EditorWaitForSeconds(TargetPositions[i].AfterDelay);
            }
            StopChangingValue();
        }

        [Button]
        [ShowIf("isRunning")]
        public void StopChangingValue()
        {
            if (isRunning)
            {
                if (currentCoroutine != null)
                {
                    EditorCoroutineUtility.StopCoroutine(currentCoroutine);
                }
                Target.transform.position = initialPosition;
                Target.transform.rotation = initialRotation;
                isRunning = false;
            }
        }
#endif  
    }
}

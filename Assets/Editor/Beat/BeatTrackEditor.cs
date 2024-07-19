using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System;
using TTT.Beat;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

[CustomTimelineEditor(typeof(BeatClip))]
public class BeatClipDrawer : ClipEditor
{
    private const float LineLength = 10f;

    public override void DrawBackground(TimelineClip clip, ClipBackgroundRegion region)
    {
        base.DrawBackground(clip, region);

        BeatClip beatClips = clip.asset as BeatClip;
        if (beatClips == null) return;

        double interval = clip.duration;
        double startTime = clip.start + region.startTime;
        double endTime = clip.start + region.endTime;

        double duration = endTime - startTime;

        int sampleCount = Mathf.Max((int)region.position.width, 200);
        Vector3[] points = new Vector3[sampleCount];

        var binds = UnityEditor.AnimationUtility.GetCurveBindings(clip.curves);
        foreach(var bind in binds)
        {
            if(bind.propertyName == "BeatCurve")
            {
                var curve = AnimationUtility.GetEditorCurve(clip.curves, bind);

                for (int i = 0; i < sampleCount; i++)
                {
                    float t = Mathf.Lerp((float)0, (float)(endTime - startTime), (float)i / (sampleCount - 1));
                    float value = curve.Evaluate(t);
                    points[i] = new Vector3(Mathf.Lerp(0, region.position.width, (float)i / (sampleCount - 1)), Mathf.Lerp(region.position.height, 0, value), 0);
                }

                Handles.color = Color.green;
                Handles.DrawAAPolyLine(2, points);
                break;
            }
        }

        for (double t = startTime; t <= endTime; t += interval)
        {

            float normalizedTime = (float)((t - startTime) / duration);
            float width = 2f;
            float targetX = (float) normalizedTime * region.position.width - width/2f;

            targetX = Mathf.Min(Mathf.Max(targetX, 1f), region.position.width - width - 1f);

            Rect rect = new Rect(targetX, 0, width, LineLength);
            EditorGUI.DrawRect(rect, Color.red);
        }
    }


}

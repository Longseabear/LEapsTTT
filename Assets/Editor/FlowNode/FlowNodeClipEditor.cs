
using TTT.Node;
using TTT.Notes.FlowNode;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;



[CustomTimelineEditor(typeof(FlowNodeClip))]
public class FlowNodeClipEditor : ClipEditor
{
    public override void DrawBackground(TimelineClip clip, ClipBackgroundRegion region)
    {
        FlowNodeClip beatClips = clip.asset as FlowNodeClip;

        if (beatClips.template.MetaData == null) return;

        FlowNode.FlowNodeMeta Meta = beatClips.template.MetaData;

        double startTime = clip.start + region.startTime;
        double endTime = clip.start + region.endTime;

        double duration = clip.end - clip.start;
        double pivotTime = duration * Meta.Pivot;

        double regionTimeLength = region.endTime - region.startTime;

        float normalizedTime = (float)(pivotTime - region.startTime) / (float)regionTimeLength;

        float width = 7f;
        float targetX = region.position.x + normalizedTime * region.position.width - width/2.0f;

            // (float)normalizedTime * region.position.width - width / 2f;
        
        Rect rect = new Rect(targetX, 2, width, region.position.height-4);
        EditorGUI.DrawRect(rect, Color.red);
    }
}

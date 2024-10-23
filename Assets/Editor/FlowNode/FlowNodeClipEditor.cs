using TTT.Node;
using TTT.Notes.FlowNode;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;
using static TTT.Notes.FLowNodeVisualization;



[CustomTimelineEditor(typeof(FlowNodeClip))]
public class FlowNodeClipEditor : ClipEditor
{
    private void DrawWaveform(ClipBackgroundRegion region, TimelineClip clip, AudioClip audioClip)
    {
        FlowNodeClip beatClips = clip.asset as FlowNodeClip;

        double Pivot = beatClips.pivot;

        float[] samples = new float[audioClip.samples * audioClip.channels];
        audioClip.GetData(samples, 0);

        int width = (int)region.position.width;
        int height = (int)region.position.height;

        if (width < 0) return;

        double visualDuration = region.endTime - region.startTime;
        double pixelPerTime = (double)width / visualDuration;


        float st = (float)(Pivot * clip.duration);
        float startPixelX = (float)((st - region.startTime) * pixelPerTime);
        float targetWidth = (float)(audioClip.length * pixelPerTime);


        Vector3[] points = new Vector3[width];

        for (int x = 0; x < width; x++)
        {
            float sampleX = x - startPixelX;
            float sampleValue;
            if (sampleX < 0 || sampleX > targetWidth)
            {
                sampleValue = 0;
            }
            else
            {
                sampleValue = samples[(int)Mathf.Floor((sampleX * samples.Length) / targetWidth)];
            }
            float y = region.position.y + (region.position.height / 2) - (sampleValue * (region.position.height / 2));
            points[x] = new Vector3(region.position.x + x, y, 0);
        }

        Handles.color = Color.yellow;
        Handles.DrawAAPolyLine(3f, points);
    }

    public override void DrawBackground(TimelineClip clip, ClipBackgroundRegion region)
    {
        FlowNodeClip beatClips = clip.asset as FlowNodeClip;

        if (beatClips.template.MetaData == null) return;

        FlowNode.FlowNodeMeta Meta = beatClips.template.MetaData;

        if (Meta is IAudioVisualizer audioInterface)
        {
            AudioClip audioClip = audioInterface.GetAudioClipForVisualize() as AudioClip;
            clip.displayName = audioClip.name;

            DrawWaveform(region, clip, audioClip);
        }
        else
        {
            double startTime = clip.start + region.startTime;
            double endTime = clip.start + region.endTime;

            double duration = clip.end - clip.start;
            double pivotTime = duration * Meta.Pivot;

            double regionTimeLength = region.endTime - region.startTime;

            float normalizedTime = (float)(pivotTime - region.startTime) / (float)regionTimeLength;

            float width = 7f;
            float targetX = region.position.x + normalizedTime * region.position.width - width / 2.0f;

            // (float)normalizedTime * region.position.width - width / 2f;

            Rect rect = new Rect(targetX, 2, width, region.position.height - 4);
            EditorGUI.DrawRect(rect, Color.red);
        }
    }
}

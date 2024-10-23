using UnityEngine;

namespace TTT.Notes
{
    public static class FLowNodeVisualization
    {
        public interface IVisualizer
        {
        }

        public interface IFlowNodePivot : IVisualizer
        {
        }

        public interface IAudioVisualizer : IVisualizer
        {
            public AudioClip GetAudioClipForVisualize();
        }
    }
}

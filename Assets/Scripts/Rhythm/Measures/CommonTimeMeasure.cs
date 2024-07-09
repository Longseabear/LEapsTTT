using System;

namespace TTT.Rhythm.Measures
{
    [Serializable]
    public class CommonTimeMeasure : MeasureNode
    {
        protected override void InitializeInternal()
        {
            int eventCount = Nodes.Count;
            float divLength = 1.0f / (float)eventCount;
            float leftPadding = divLength / 2.0f;

            for (int i = 0; i < eventCount; i++)
            {
                Nodes[i].Initialize(Timer.MakeSubTimer(((float)i * divLength + leftPadding) * Length));
            }
        }
    }
}

using Unity.VisualScripting;
using UnityEngine;

namespace TTT.Common
{
    public static class RhythmProperty
    {
        public static float BaseMeasureLength { get; } = 2.0f;
        public static float BaseBeatDuration { get; } = 0.5f;
        public static float OneBeatLength { get; } = 0.2f;

        public static AnimationCurve SimpleBeatCurve { get; private set; }

        private static void MakeSimpleBeat()
        {
            Keyframe[] keys = new Keyframe[3];
            keys[0] = new Keyframe(0f, 0f);   // x = 0, y = 0
            keys[1] = new Keyframe(0.5f, 1f); // x = 0.5, y = 1
            keys[2] = new Keyframe(1.0f, 0f);   // x = 1, y = 1

            // Create the curve/
            SimpleBeatCurve = new AnimationCurve(keys);

            // Ensure the curve is linear between keyframes
            for (int i = 0; i < keys.Length; i++)
            {
                SimpleBeatCurve.keys[i].inTangent = 0f;
                SimpleBeatCurve.keys[i].outTangent = 0f;
            }
        }
        static RhythmProperty()
        {
            MakeSimpleBeat();
        }
    }
}

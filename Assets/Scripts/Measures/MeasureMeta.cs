using System;
using UnityEngine;

namespace TTT.Measures
{
    [Serializable]
    public abstract class MeasureMeta : ScriptableObject
    {
        public MeasureMeta() { }
        public MeasureMeta(MeasureMeta node)
        {
        }
        public abstract MeasureMeta DeepCopy();
        public abstract Measure Build();
    }

}

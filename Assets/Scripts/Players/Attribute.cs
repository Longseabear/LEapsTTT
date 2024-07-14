using System;
using TTT.Common;

namespace TTT.Players
{
    public abstract class Attribute
    {
    }

    [Serializable]
    public class PlaceAttribute : Attribute, ICloneable
    {
        public float Height = 1.5f;
        public Bounds2D PlaceNoiseIntensity;
        public float BaseSpeed = 8.0f;

        public object Clone()
        {
            var obj = new PlaceAttribute();
            obj.Height = Height;
            obj.PlaceNoiseIntensity = PlaceNoiseIntensity.Clone() as Bounds2D;
            obj.BaseSpeed = BaseSpeed;
            return obj;
        }
    }
}

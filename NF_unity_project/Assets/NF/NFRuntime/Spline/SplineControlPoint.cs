using UnityEngine;

namespace NFRuntime.Spline
{
    public class SplineControlPoint
    {
        public int Index { get; }

        public Vector3 Position;
        public Vector3 Normal;
        public float Distance;

        public SplineControlPoint(int index)
        {
            this.Index = index;
        }
    }
}

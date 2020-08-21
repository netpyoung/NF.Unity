using UnityEngine;

namespace NFRuntime.Spline
{
    public class LineInfo
    {
        public Vector3 BasePosition { get; private set; }
        public Vector3 TipPosition { get; private set; }
        public Vector3 Position { get; private set; }
        public Vector3 Normal { get; private set; }

        public void Init(Vector3 start, Vector3 end)
        {
            BasePosition = start;
            TipPosition = end;
            Position = (BasePosition + TipPosition) / 2f;
            Normal = (TipPosition - BasePosition);
        }
    }
}

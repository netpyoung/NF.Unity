using System;
using System.Collections.Generic;
using UnityEngine;

namespace NFRuntime.Spline
{
    public class Spliner
    {
        List<SplineControlPoint> mSplineControlPoints { get; } = new List<SplineControlPoint>();
        List<SplineControlPoint> mCache { get; } = new List<SplineControlPoint>();
        
        public int GetLineCount() => mSplineControlPoints.Count;

        public void Init(int maxPlayingLength)
        {
            mSplineControlPoints.Clear();
            mSplineControlPoints.Capacity = maxPlayingLength;

            mCache.Clear();
            mCache.Capacity = maxPlayingLength;

            for (int i = 0; i < maxPlayingLength; ++i)
            {
                mCache.Add(new SplineControlPoint(i));
            }
        }

        public void Refresh(NF.Collections.Generic.LinkedList<LineInfo> lineInfos, int desireRecordFrameCount)
        {
            mSplineControlPoints.Clear();

            if (desireRecordFrameCount == 0)
            {
                return;
            }

            int snapshotIndex = 0;
            for (var node = lineInfos.GetHeadNode(); node != null; node = node.Next)
            {
                if (snapshotIndex == desireRecordFrameCount)
                {
                    break;
                }
                if (snapshotIndex == mCache.Count)
                {
                    break;
                }

                LineInfo lineInfo = node.Item;
                SplineControlPoint cp = mCache[snapshotIndex];
                cp.Position = lineInfo.Position;
                cp.Normal = lineInfo.Normal;
                mSplineControlPoints.Add(cp);
                snapshotIndex++;
            }

            // recalculate distance.
            SplineControlPoint prev = mSplineControlPoints[0];
            prev.Distance = 0;
            for (int i = 1; i < mSplineControlPoints.Count; ++i)
            {
                var curr = mSplineControlPoints[i];
                curr.Distance = prev.Distance + (curr.Position - prev.Position).magnitude;
                prev = curr;
            }
        }

        public (Vector3 position, Vector3 normal) Interpolate(float t)
        {
            var (cp1, dist) = GetControlPointWithDist(Mathf.Clamp01(t));
            var cp0 = mSplineControlPoints[Math.Max(cp1.Index - 1, 0)];
            var cp2 = mSplineControlPoints[Math.Min(cp1.Index + 1, mSplineControlPoints.Count - 1)];
            var cp3 = mSplineControlPoints[Math.Min(cp1.Index + 2, mSplineControlPoints.Count - 1)];
            return (
                Util.Curve.CatmulRom(cp0.Position, cp1.Position, cp2.Position, cp3.Position, dist),
                Util.Curve.CatmulRom(cp0.Normal, cp1.Normal, cp2.Normal, cp3.Normal, dist)
            );
        }

        (SplineControlPoint cp, float dist) GetControlPointWithDist(float t)
        {
            float dist = mSplineControlPoints[mSplineControlPoints.Count - 1].Distance * t;

            SplineControlPoint curr = null;
            for (int i = 0; i < mSplineControlPoints.Count; ++i)
            {
                curr = mSplineControlPoints[i];
                if (curr.Distance >= dist)
                {
                    break;
                }
            }
            if (curr.Index == 0)
            {
                return (curr, 0);
            }

            SplineControlPoint prev = mSplineControlPoints[curr.Index - 1];
            return (prev, Mathf.Clamp01((dist - prev.Distance) / (curr.Distance - prev.Distance)));
        }
    }
}

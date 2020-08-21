using NFRuntime.Spline;
using UnityEngine;

namespace NFRuntime.WeaponTrailer
{
    public class TrailRecorder
    {
        public void Record(TrailRecordInfo info, float dt)
        {
            Record(info, info.Config.TransformBase.position, info.Config.TransformTip.position, dt);
        }

        public void Record(TrailRecordInfo info, Vector3 basePosition, Vector3 tipPosition, float dt)
        {
            if (info.RecordElements.Count < info.Config.MaxPlayingRecordLength)
            {
                var snapshot = new LineInfo();
                snapshot.Init(basePosition, tipPosition);
                info.RecordElements.AddHead(snapshot);
            }
            else
            {
                info.RecordElements.TryRemoveTail(out var tail);
                tail.Init(basePosition, tipPosition);
                info.RecordElements.AddHead(tail);
            }
        }
    }
}

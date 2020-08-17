using NFRuntime.Shape;
using NFRuntime.Spline;
using System;

namespace NFRuntime.WeaponTrailer
{
    public class TrailPlayer
    {
        Spliner mSpliner = new Spliner();

        public void Init(TrailRecordInfo recordInfo)
        {
            mSpliner.Init(recordInfo.Config.MaxPlayingRecordLength * recordInfo.Config.SplineDetail);
        }

        public void Play(TrailRecordInfo recordInfo, int recordNum)
        {
            TrailMeshObject tmo = recordInfo.tmo;
            NF.Collections.Generic.LinkedList<LineInfo> records = recordInfo.RecordElements;
            SplineMeshInfo meshInfo = recordInfo.MeshInfo;
            TrailRecordConfig config = recordInfo.Config;

            switch (config.ELinePerVertexCount)
            {
                case TrailRecordConfig.E_LinePerVertexCount.TWO:
                    UpdateMeshObjectWithSpline2(tmo, recordInfo.mMeshModifier, records, meshInfo, config);
                    break;
                case TrailRecordConfig.E_LinePerVertexCount.THREE:
                    UpdateMeshObjectWithSpline3(tmo, recordInfo.mMeshModifier, records, meshInfo, config);
                    break;
            }
        }

        void UpdateMeshObjectWithSpline3(TrailMeshObject tmo, MeshModifier meshModifier, NF.Collections.Generic.LinkedList<LineInfo> records, SplineMeshInfo meshInfo, TrailRecordConfig config)
        {
            int desireRecordFrameCount = Math.Min(records.Count, config.MaxPlayingRecordLength);
            if (desireRecordFrameCount < 2)
            {
                return;
            }

            var line = mSpliner.Refresh(records, desireRecordFrameCount);
            meshModifier.FillSplineVUC3(mSpliner, config.ColorStart, config.ColorEnd, ref meshInfo.mMeshInfo);
            tmo.UpdateMesh(line, 3, ref meshInfo.mMeshInfo);
        }

        void UpdateMeshObjectWithSpline2(TrailMeshObject tmo, MeshModifier meshModifier, NF.Collections.Generic.LinkedList<LineInfo> records, SplineMeshInfo meshInfo, TrailRecordConfig config)
        {
            int desireRecordFrameCount = Math.Min(records.Count, config.MaxPlayingRecordLength);
            if (desireRecordFrameCount < 2)
            {
                return;
            }

            var line = mSpliner.Refresh(records, desireRecordFrameCount);
            meshModifier.FillSplineVUC2(mSpliner, config.ColorStart, config.ColorEnd, ref meshInfo.mMeshInfo);
            tmo.UpdateMesh(line, 2, ref meshInfo.mMeshInfo);
        }
    }
}

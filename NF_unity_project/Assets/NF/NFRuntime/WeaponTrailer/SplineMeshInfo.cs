using NFRuntime.Shape;
using System;
using Unity.Collections;
using UnityEngine;

namespace NFRuntime.WeaponTrailer
{
    public class SplineMeshInfo : IDisposable
    {
        public NativeMeshInfo mMeshInfo = new NativeMeshInfo();

        public SplineMeshInfo(TrailRecordConfig config, MeshModifier meshModifier)
        {
            var linePerVertexCount = (int)config.ELinePerVertexCount;
            int line = config.MaxPlayingRecordLength * config.SplineDetail;
            var totalVertexCount = line * linePerVertexCount;

            mMeshInfo.Vertices = new NativeArray<Vector3>(new Vector3[totalVertexCount], Allocator.Persistent);
            mMeshInfo.UVs = new NativeArray<Vector2>(new Vector2[totalVertexCount], Allocator.Persistent);
            mMeshInfo.Colors = new NativeArray<Color>(new Color[totalVertexCount], Allocator.Persistent);
            mMeshInfo.Indices = new NativeArray<int>(new int[(line - 1) * linePerVertexCount * (linePerVertexCount + 1)], Allocator.Persistent);

            if (config.ColorStart == config.ColorEnd)
            {
                switch (config.ELinePerVertexCount)
                {
                    case TrailRecordConfig.E_LinePerVertexCount.TWO:
                        meshModifier.FillSplineCI2(config.ColorStart, ref mMeshInfo);
                        break;
                    case TrailRecordConfig.E_LinePerVertexCount.THREE:
                        meshModifier.FillSplineCI3(config.ColorStart, ref mMeshInfo);
                        break;
                }
            }
            else
            {
                switch (config.ELinePerVertexCount)
                {
                    case TrailRecordConfig.E_LinePerVertexCount.TWO:
                        meshModifier.FillSplineI2(ref mMeshInfo);
                        break;
                    case TrailRecordConfig.E_LinePerVertexCount.THREE:
                        meshModifier.FillSplineI3(ref mMeshInfo);
                        break;
                }
            }
        }

        public void Dispose()
        {
            this.mMeshInfo.Vertices.Dispose();
            this.mMeshInfo.Colors.Dispose();
            this.mMeshInfo.UVs.Dispose();
            this.mMeshInfo.Indices.Dispose();
        }
    }
}

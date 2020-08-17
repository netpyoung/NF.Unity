using NFRuntime.Shape;
using NFRuntime.Spline;
using System;

namespace NFRuntime.WeaponTrailer
{
    public class TrailRecordInfo : IDisposable
    {
        public TrailMeshObject tmo;
        public NF.Collections.Generic.LinkedList<LineInfo> RecordElements = new NF.Collections.Generic.LinkedList<LineInfo>();
        public TrailRecordConfig Config;
        public SplineMeshInfo MeshInfo { get; }
        //public FadingInfo FadingInfo { get; } = new FadingInfo();
        public MeshModifier mMeshModifier = new MeshModifier();

        public TrailRecordInfo(TrailRecordConfig config)
        {
            this.Config = config;
            this.MeshInfo = new SplineMeshInfo(config, mMeshModifier);
            tmo = new TrailMeshObject();
            tmo.Init(config.Materials, config.Layer, config.SortingLayerName, config.SortingOrder);
        }

        public void Dispose()
        {
            this.MeshInfo.Dispose();
        }
    }
}

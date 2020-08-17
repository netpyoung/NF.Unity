using NFRuntime.Shape;
using NFRuntime.Spline;
using System;
using System.Collections;
using System.Collections.Generic;
using NF.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace NFExample
{
    public class SplineMeshInfo : IDisposable
    {
        public NativeMeshInfo mMeshInfo = new NativeMeshInfo();
        MeshModifier mMeshModifier = new MeshModifier();
        public bool IsPrefilledColor { get; private set; }

        public SplineMeshInfo(TrailRecordConfig config)
        {
            if (config.IsUsingSpline)
            {
                var linePerVertexCount = (int)config.ELinePerVertexCount;
                int line = config.MaxPlayingRecordLength * config.SplineDetail;
                var totalVertexCount = line * linePerVertexCount;
                mMeshInfo.Vertices = new NativeArray<Vector3>(new Vector3[totalVertexCount], Allocator.Persistent);
                mMeshInfo.UVs = new NativeArray<Vector2>(new Vector2[totalVertexCount], Allocator.Persistent);
                mMeshInfo.Colors = new NativeArray<Color>(new Color[totalVertexCount], Allocator.Persistent);
                mMeshInfo.Indices = new NativeArray<int>(new int[(line - 1) * linePerVertexCount * (linePerVertexCount + 1)], Allocator.Persistent);
                IsPrefilledColor = (config.StartColor == config.EndColor);
            }
            else
            {
                var linePerVertexCount = (int)config.ELinePerVertexCount;
                int line = config.MaxPlayingRecordLength;
                var totalVertexCount = line * linePerVertexCount;
                mMeshInfo.Vertices = new NativeArray<Vector3>(new Vector3[totalVertexCount], Allocator.Persistent);
                mMeshInfo.UVs = new NativeArray<Vector2>(new Vector2[totalVertexCount], Allocator.Persistent);
                mMeshInfo.Colors = new NativeArray<Color>(new Color[totalVertexCount], Allocator.Persistent);
                mMeshInfo.Indices = new NativeArray<int>(new int[(line - 1) * linePerVertexCount * (linePerVertexCount + 1)], Allocator.Persistent);
                IsPrefilledColor = (config.StartColor == config.EndColor);
            }

            switch (config.ELinePerVertexCount)
            {
                case TrailRecordConfig.E_LinePerVertexCount.TWO:
                    FillPreMeshInfo2(config);
                    break;
                case TrailRecordConfig.E_LinePerVertexCount.THREE:
                    FillPreMeshInfo3(config);
                    break;
            }
        }

        void FillPreMeshInfo2(TrailRecordConfig config)
        {
            if (IsPrefilledColor)
            {
                mMeshModifier.FillSplineCI2(config.StartColor, ref mMeshInfo);
            }
            else
            {
                mMeshModifier.FillSplineI2(ref mMeshInfo);
            }
        }

        void FillPreMeshInfo3(TrailRecordConfig config)
        {
            if (IsPrefilledColor)
            {
                mMeshModifier.FillSplineCI3(config.StartColor, ref mMeshInfo);
            }
            else
            {
                mMeshModifier.FillSplineI3(ref mMeshInfo);
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
    public class TrailRecordInfo : IDisposable
    {
        public TrailMeshObject tmo;
        public NF.Collections.Generic.LinkedList<LineInfo> RecordElements = new NF.Collections.Generic.LinkedList<LineInfo>();
        public TrailRecordConfig Config;
        public SplineMeshInfo MeshInfo { get; }
        //public FadingInfo FadingInfo { get; } = new FadingInfo();

        public TrailRecordInfo(TrailRecordConfig config)
        {
            this.Config = config;
            this.MeshInfo = new SplineMeshInfo(config);
            tmo = new TrailMeshObject();
            tmo.Init(config.Materials, config.Layer, config.SortingLayerName, config.SortingOrder);
        }

        public void Dispose()
        {
            this.MeshInfo.Dispose();
        }
    }
    public class TrailRecorder
    {
        public void Record(TrailRecordInfo info, float dt)
        {
            Record(info, info.Config.BaseTransform.position, info.Config.TipTransform.position, dt);
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

    [Serializable]
    public class TrailRecordConfig
    {
        public enum E_LinePerVertexCount
        {
            TWO = 2,
            THREE = 3,
        }
        public Transform TipTransform;
        public Transform BaseTransform;
        public Material[] Materials;
        public Color StartColor;
        public Color EndColor;
        public bool IsUsingSpline;

        public int MaxPlayingRecordLength;
        public int SplineDetail;
        public E_LinePerVertexCount ELinePerVertexCount;

        public string SortingLayerName { get; private set; }
        public int SortingOrder { get; private set; }
        public int Layer { get; private set; }

        public void RegisterLayerInfo(int objectLayer, string rendererSortingLayerName, int rendererSortingOrder)
        {
            this.SortingLayerName = rendererSortingLayerName;
            this.SortingOrder = rendererSortingOrder;
            this.Layer = objectLayer;
        }
    }

    [RequireComponent(typeof(MeshRenderer))]
    public class GoWeaponTrail : MonoBehaviour
    {
        #region Public
        public TrailRecordConfig Config;
        #endregion Public
        TrailRecorder mRecorder = new TrailRecorder();
        TrailPlayer mPlayer = new TrailPlayer();

        TrailRecordInfo mInfo;
        void Start()
        {
            mInfo = PlayBegin();
        }

        void Update()
        {
            RecordAndPlay(mInfo, Time.deltaTime);
        }

        private void OnDisable()
        {
            mInfo.Dispose();
        }

        public TrailRecordInfo PlayBegin()
        {
            var mr = GetComponent<MeshRenderer>();
            Config.RegisterLayerInfo(gameObject.layer, mr.sortingLayerName, mr.sortingOrder);
            var recordInfo = new TrailRecordInfo(Config);
            mPlayer.Init(recordInfo);
            return recordInfo;
        }

        public void RecordAndPlay(TrailRecordInfo recordInfo, float dt)
        {
            mRecorder.Record(recordInfo, dt);
            mPlayer.Play(recordInfo, 0);
        }

        void OnDrawGizmos()
        {
            if (Config.BaseTransform == null || Config.TipTransform == null)
            {
                return;
            }


            float dist = (Config.BaseTransform.position - Config.TipTransform.position).magnitude;
            if (dist < Mathf.Epsilon)
            {
                return;
            }

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(Config.BaseTransform.position, dist * 0.04f);

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(Config.TipTransform.position, dist * 0.04f);
        }
    }

    public class TrailMeshObject
    {
        // TODO(pyoung): root를 기준으로하는 GameObjectPool이 필요하겠구나.
        public GameObject GameObject { get; }
        public Mesh Mesh { get; }
        public MeshFilter MeshFilter { get; }
        public MeshRenderer MeshRenderer { get; }

        public TrailMeshObject()
        {
            GameObject = new GameObject("TrailMesh: uninitialized");
            Mesh = new Mesh();
            Mesh.name = "mesh";
            MeshFilter = GameObject.AddComponent<MeshFilter>();
            MeshRenderer = GameObject.AddComponent<MeshRenderer>();
            MeshFilter.sharedMesh = Mesh;
            GameObject.SetActive(false);
        }

        public void Init(Material[] materials, int objectLayer, string rendererSortingLayerName, int rendererSortingOrder)
        {
            GameObject.name = "TrailMesh: material={material.name}";
            GameObject.transform.position = Vector3.zero;
            GameObject.transform.rotation = Quaternion.identity;
            GameObject.layer = objectLayer;

            MeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            MeshRenderer.receiveShadows = false;
            MeshRenderer.materials = materials;
            MeshRenderer.sortingLayerName = rendererSortingLayerName;
            MeshRenderer.sortingOrder = rendererSortingOrder;

            GameObject.SetActive(true);
        }

        public void UpdateMesh(int line, int linePerVertexCount, ref NativeMeshInfo info)
        {
            int vertCount = line * linePerVertexCount;
            int indicesCount = (line - 1) * linePerVertexCount * (linePerVertexCount + 1);
            Mesh.SetVertices(info.Vertices.GetSubArray(0, vertCount));
            Mesh.SetColors(info.Colors.GetSubArray(0, vertCount));
            Mesh.SetUVs(0, info.UVs.GetSubArray(0, vertCount));
            Mesh.SetIndices(info.Indices.GetSubArray(0, indicesCount), MeshTopology.Triangles, 0);
        }
    }

    public class TrailPlayer
    {
        MeshModifier mMeshModifier = new MeshModifier();

        Spliner mSpliner;

        public void Init(TrailRecordInfo recordInfo)
        {
            if (recordInfo.Config.IsUsingSpline)
            {
                mSpliner = new Spliner();
                mSpliner.Init(recordInfo.Config.MaxPlayingRecordLength * recordInfo.Config.SplineDetail);
            }
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
                    if (config.IsUsingSpline)
                    {
                        UpdateMeshObjectWithSpline2(tmo, records, meshInfo, config);
                    }
                    else
                    {
                        UpdateMeshObjectWithoutSpline2(tmo, records, meshInfo, config);
                    }
                    break;
                case TrailRecordConfig.E_LinePerVertexCount.THREE:
                    if (config.IsUsingSpline)
                    {
                        UpdateMeshObjectWithSpline3(tmo, records, meshInfo, config);
                    }
                    else
                    {
                        UpdateMeshObjectWithoutSpline3(tmo, records, meshInfo, config);
                    }
                    break;
            }
        }

        void UpdateMeshObjectWithSpline3(TrailMeshObject tmo, NF.Collections.Generic.LinkedList<LineInfo> records, SplineMeshInfo meshInfo, TrailRecordConfig config)
        {
            int desireRecordFrameCount = Math.Min(records.Count, config.MaxPlayingRecordLength);
            if (desireRecordFrameCount < 2)
            {
                return;
            }

            var line = mSpliner.Refresh(records, desireRecordFrameCount);
            mMeshModifier.FillSplineVUC3(mSpliner, config.StartColor, config.EndColor, ref meshInfo.mMeshInfo);
            tmo.UpdateMesh(line, 3, ref meshInfo.mMeshInfo);
        }

        void UpdateMeshObjectWithoutSpline3(TrailMeshObject tmo, NF.Collections.Generic.LinkedList<LineInfo> records, SplineMeshInfo meshInfo, TrailRecordConfig config)
        {
            var line = mMeshModifier.FillTrackVUC3(records, config.MaxPlayingRecordLength, config.StartColor, config.EndColor, ref meshInfo.mMeshInfo);
            tmo.UpdateMesh(line, 3, ref meshInfo.mMeshInfo);
        }

        void UpdateMeshObjectWithSpline2(TrailMeshObject tmo, NF.Collections.Generic.LinkedList<LineInfo> records, SplineMeshInfo meshInfo, TrailRecordConfig config)
        {
            int desireRecordFrameCount = Math.Min(records.Count, config.MaxPlayingRecordLength);
            if (desireRecordFrameCount < 2)
            {
                return;
            }

            var line = mSpliner.Refresh(records, desireRecordFrameCount);
            mMeshModifier.FillSplineVUC2(mSpliner, config.StartColor, config.EndColor, ref meshInfo.mMeshInfo);
            tmo.UpdateMesh(line, 2, ref meshInfo.mMeshInfo);
        }

        void UpdateMeshObjectWithoutSpline2(TrailMeshObject tmo, NF.Collections.Generic.LinkedList<LineInfo> records, SplineMeshInfo meshInfo, TrailRecordConfig config)
        {
            var line = mMeshModifier.FillTrackVUC2(records, config.MaxPlayingRecordLength, config.StartColor, config.EndColor, ref meshInfo.mMeshInfo);
            tmo.UpdateMesh(line, 2, ref meshInfo.mMeshInfo);
        }
    }
}

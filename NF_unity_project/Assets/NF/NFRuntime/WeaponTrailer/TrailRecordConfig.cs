using System;
using UnityEngine;

namespace NFRuntime.WeaponTrailer
{
    [Serializable]
    public class TrailRecordConfig
    {
        public enum E_LinePerVertexCount
        {
            TWO = 2,
            THREE = 3,
        }
        public Transform TransformTip;
        public Transform TransformBase;
        public Material[] Materials;
        public Color ColorStart;
        public Color ColorEnd;

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
}

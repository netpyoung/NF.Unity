using NFRuntime.Shape;
using UnityEngine;

namespace NFRuntime.WeaponTrailer
{
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
}

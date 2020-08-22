using NFRuntime.Shape;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[DisallowMultipleComponent]
public class GoRangeMesh : MonoBehaviour
{
    MeshModifier mm = new MeshModifier();
    MeshInfo mMeshInfo;
    Mesh mMesh;

    private void Awake()
    {
        var mf = GetComponent<MeshFilter>();
        mMesh = new Mesh();
        mf.sharedMesh = mMesh;
    }

    public void Init(int angle)
    {
        mMeshInfo = mm.FillCircle(angle);
        for (int i = 0; i < mMeshInfo.Vertices.Length; ++i)
        {
            mMeshInfo.Vertices[i] *= 3;
        }
        mMesh.vertices = mMeshInfo.Vertices;
        mMesh.uv = mMeshInfo.UVs;
        mMesh.triangles = mMeshInfo.Indices;
    }
}

using Unity.Collections;
using UnityEngine;

namespace NFRuntime.Shape
{
    public struct NativeMeshInfo
    {
        public NativeArray<Vector3> Vertices;
        public NativeArray<Vector2> UVs;
        public NativeArray<Color> Colors;
        public NativeArray<int> Indices;
    }
}
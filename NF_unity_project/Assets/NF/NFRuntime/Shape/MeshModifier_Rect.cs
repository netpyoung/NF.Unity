using UnityEngine;
using UnityEngine.Assertions;

namespace NFRuntime.Shape
{
    public partial class MeshModifier
    {
        public void FillRect(int width, int height, ref MeshInfo info)
        {
            Assert.IsNotNull(info.Vertices);
            Assert.IsNotNull(info.UVs);
            Assert.IsNotNull(info.Indices);

            Assert.AreEqual(info.Vertices.Length, 4);
            Assert.AreEqual(info.UVs.Length, 4);
            Assert.AreEqual(info.Indices.Length, 6);

            float halfWidth = width / 2;
            float halfHeight = height / 2;

            info.Vertices[0] = new Vector3(-halfWidth, 0, halfHeight);
            info.Vertices[1] = new Vector3(halfWidth, 0, halfHeight);
            info.Vertices[2] = new Vector3(-halfWidth, 0, -halfHeight);
            info.Vertices[3] = new Vector3(halfWidth, 0, -halfHeight);

            info.UVs[0] = new Vector2(0, 0);
            info.UVs[1] = new Vector2(1, 0);
            info.UVs[2] = new Vector2(0, 1);
            info.UVs[3] = new Vector2(1, 1);

            info.Indices[0] = 0;
            info.Indices[1] = 1;
            info.Indices[2] = 3;

            info.Indices[3] = 3;
            info.Indices[4] = 2;
            info.Indices[5] = 0;
        }

        public void FillRect(int width, int height, ref NativeMeshInfo info)
        {
            Assert.IsTrue(info.Vertices.IsCreated);
            Assert.IsTrue(info.UVs.IsCreated);
            Assert.IsTrue(info.Indices.IsCreated);

            Assert.AreEqual(info.Vertices.Length, 4);
            Assert.AreEqual(info.UVs.Length, 4);
            Assert.AreEqual(info.Indices.Length, 6);

            float halfWidth = width / 2;
            float halfHeight = height / 2;

            info.Vertices[0] = new Vector3(-halfWidth, 0, halfHeight);
            info.Vertices[1] = new Vector3(halfWidth, 0, halfHeight);
            info.Vertices[2] = new Vector3(-halfWidth, 0, -halfHeight);
            info.Vertices[3] = new Vector3(halfWidth, 0, -halfHeight);

            info.UVs[0] = new Vector2(0, 0);
            info.UVs[1] = new Vector2(1, 0);
            info.UVs[2] = new Vector2(0, 1);
            info.UVs[3] = new Vector2(1, 1);

            info.Indices[0] = 0;
            info.Indices[1] = 1;
            info.Indices[2] = 3;

            info.Indices[3] = 3;
            info.Indices[4] = 2;
            info.Indices[5] = 0;
        }
    }
}
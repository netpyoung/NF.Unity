using UnityEngine;
using UnityEngine.Assertions;

namespace NFRuntime.Shape
{
    public partial class MeshModifier
    {
        public MeshInfo FillCircle(int angle)
        {
            var info = new MeshInfo();

            var initialAngle = -angle / 2;
            var seg = (angle / 10);
            int numVerts = seg + 2;
            int numIndices = 3 * seg;

            info.Vertices = new Vector3[numVerts];
            info.UVs = new Vector2[numVerts];
            info.Indices = new int[numIndices];

            info.Vertices[0] = Vector3.zero;
            info.UVs[0] = new Vector2(0.5f, 0.5f);

            float detailAngle = 10;

            for (int i = 1; i < numVerts; ++i)
            {
                info.Vertices[i] = Quaternion.AngleAxis(initialAngle + (detailAngle * (i - 1)), Vector3.up) * Vector3.forward;

                float normedHorizontal = (info.Vertices[i].x + 1.0f) * 0.5f;
                float normedVertical = (info.Vertices[i].x + 1.0f) * 0.5f;
                info.UVs[i] = new Vector2(normedHorizontal, normedVertical);
            }

            for (int i = 0; i < seg; ++i)
            {
                int index = i * 3;
                info.Indices[index + 0] = 0;
                info.Indices[index + 1] = i + 1;
                info.Indices[index + 2] = i + 2;
            }
            return info;
        }

        public void FillCircle(int angle, ref MeshInfo info)
        {
            Assert.IsNotNull(info.Vertices);
            Assert.IsNotNull(info.UVs);
            Assert.IsNotNull(info.Indices);

            var initialAngle = -angle / 2;
            var seg = (angle / 10);
            int numVerts = seg + 2;
            int numIndices = 3 * seg;

            Assert.AreEqual(info.Vertices.Length, numVerts);
            Assert.AreEqual(info.UVs.Length, numVerts);
            Assert.AreEqual(info.Indices.Length, numIndices);

            info.Vertices[0] = Vector3.zero;
            info.UVs[0] = new Vector2(0.5f, 0.5f);

            float detailAngle = 10;

            for (int i = 1; i < numVerts; ++i)
            {
                info.Vertices[i] = Quaternion.AngleAxis(initialAngle + (detailAngle * (i - 1)), Vector3.up) * Vector3.forward;

                float normedHorizontal = (info.Vertices[i].x + 1.0f) * 0.5f;
                float normedVertical = (info.Vertices[i].x + 1.0f) * 0.5f;
                info.UVs[i] = new Vector2(normedHorizontal, normedVertical);
            }

            for (int i = 0; i < seg; ++i)
            {
                int index = i * 3;
                info.Indices[index + 0] = 0;
                info.Indices[index + 1] = i + 1;
                info.Indices[index + 2] = i + 2;
            }
        }

        public void FillCircle(int angle, ref NativeMeshInfo info)
        {
            Assert.IsTrue(info.Vertices.IsCreated);
            Assert.IsTrue(info.UVs.IsCreated);
            Assert.IsTrue(info.Indices.IsCreated);

            var initialAngle = -angle / 2;
            var seg = (angle / 10);
            int numVerts = seg + 2;
            int numIndices = 3 * seg;

            Assert.AreEqual(info.Vertices.Length, numVerts);
            Assert.AreEqual(info.UVs.Length, numVerts);
            Assert.AreEqual(info.Indices.Length, numIndices);

            info.Vertices[0] = Vector3.zero;
            info.UVs[0] = new Vector2(0.5f, 0.5f);

            float detailAngle = 10;

            for (int i = 1; i < numVerts; ++i)
            {
                info.Vertices[i] = Quaternion.AngleAxis(initialAngle + (detailAngle * (i - 1)), Vector3.up) * Vector3.forward;

                float normedHorizontal = (info.Vertices[i].x + 1.0f) * 0.5f;
                float normedVertical = (info.Vertices[i].x + 1.0f) * 0.5f;
                info.UVs[i] = new Vector2(normedHorizontal, normedVertical);
            }

            for (int i = 0; i < seg; ++i)
            {
                int index = i * 3;
                info.Indices[index + 0] = 0;
                info.Indices[index + 1] = i + 1;
                info.Indices[index + 2] = i + 2;
            }
        }
    }
}
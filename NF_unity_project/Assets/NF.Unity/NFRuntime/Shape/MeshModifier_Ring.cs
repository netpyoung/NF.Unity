using UnityEngine;
using UnityEngine.Assertions;

namespace NFRuntime.Shape
{
    public partial class MeshModifier
    {
        public MeshInfo FillRing(float innerRadius, float outerRadius, int angle)
        {
            var info = new MeshInfo();
            const int detailAngle = 10;
            var initialAngle = -angle / 2;
            var seg = (angle / detailAngle);
            int numVerts = 2 + (2 * seg);
            int numIndices = 6 * seg;
            info.Vertices = new Vector3[numVerts];
            info.UVs = new Vector2[numVerts];
            info.Indices = new int[numIndices];

            float totalRadius = innerRadius + outerRadius;

            for (int i = 0; i <= seg; ++i)
            {
                var v = Quaternion.AngleAxis(initialAngle + (detailAngle * i), Vector3.up) * Vector3.forward;
                int index = i * 2;
                info.Vertices[index + 0] = v * innerRadius;
                info.Vertices[index + 1] = v * totalRadius;

                float normedHorizontal = (info.Vertices[index].x + 1.0f) * 0.5f;
                float normedVertical = (info.Vertices[index].x + 1.0f) * 0.5f;
                info.UVs[index + 0] = new Vector2(normedHorizontal, normedVertical);
                info.UVs[index + 1] = new Vector2(normedHorizontal, normedVertical);
            }

            for (int i = 0; i < seg; ++i)
            {
                int index = i * 6;
                int vi = i * 2;
                info.Indices[index + 0] = vi + 0;
                info.Indices[index + 1] = vi + 1;
                info.Indices[index + 2] = vi + 2;

                info.Indices[index + 3] = vi + 2;
                info.Indices[index + 4] = vi + 1;
                info.Indices[index + 5] = vi + 3;
            }
            return info;
        }

        public void FillRing(float innerRadius, float outerRadius, int angle, ref MeshInfo info)
        {
            Assert.IsNotNull(info.Vertices);
            Assert.IsNotNull(info.UVs);
            Assert.IsNotNull(info.Indices);

            const int detailAngle = 10;
            var initialAngle = -angle / 2;
            var seg = (angle / detailAngle);
            int numVerts = 2 + (2 * seg);
            int numIndices = 6 * seg;

            Assert.AreEqual(info.Vertices.Length, numVerts);
            Assert.AreEqual(info.UVs.Length, numVerts);
            Assert.AreEqual(info.Indices.Length, numIndices);

            float totalRadius = innerRadius + outerRadius;

            for (int i = 0; i <= seg; ++i)
            {
                var v = Quaternion.AngleAxis(initialAngle + (detailAngle * i), Vector3.up) * Vector3.forward;
                int index = i * 2;
                info.Vertices[index + 0] = v * innerRadius;
                info.Vertices[index + 1] = v * totalRadius;

                float normedHorizontal = (info.Vertices[index].x + 1.0f) * 0.5f;
                float normedVertical = (info.Vertices[index].x + 1.0f) * 0.5f;
                info.UVs[index + 0] = new Vector2(normedHorizontal, normedVertical);
                info.UVs[index + 1] = new Vector2(normedHorizontal, normedVertical);
            }

            for (int i = 0; i < seg; ++i)
            {
                int index = i * 6;
                int vi = i * 2;
                info.Indices[index + 0] = vi + 0;
                info.Indices[index + 1] = vi + 1;
                info.Indices[index + 2] = vi + 2;

                info.Indices[index + 3] = vi + 2;
                info.Indices[index + 4] = vi + 1;
                info.Indices[index + 5] = vi + 3;
            }
        }

        public void FillRing(float innerRadius, float outerRadius, int angle, ref NativeMeshInfo info)
        {
            Assert.IsTrue(info.Vertices.IsCreated);
            Assert.IsTrue(info.UVs.IsCreated);
            Assert.IsTrue(info.Indices.IsCreated);

            const int detailAngle = 10;
            var initialAngle = -angle / 2;
            var seg = (angle / detailAngle);
            int numVerts = 2 + (2 * seg);
            int numIndices = 6 * seg;

            Assert.AreEqual(info.Vertices.Length, numVerts);
            Assert.AreEqual(info.UVs.Length, numVerts);
            Assert.AreEqual(info.Indices.Length, numIndices);

            float totalRadius = innerRadius + outerRadius;

            for (int i = 0; i <= seg; ++i)
            {
                var v = Quaternion.AngleAxis(initialAngle + (detailAngle * i), Vector3.up) * Vector3.forward;
                int index = i * 2;
                info.Vertices[index + 0] = v * innerRadius;
                info.Vertices[index + 1] = v * totalRadius;

                float normedHorizontal = (info.Vertices[index].x + 1.0f) * 0.5f;
                float normedVertical = (info.Vertices[index].x + 1.0f) * 0.5f;
                info.UVs[index + 0] = new Vector2(normedHorizontal, normedVertical);
                info.UVs[index + 1] = new Vector2(normedHorizontal, normedVertical);
            }

            for (int i = 0; i < seg; ++i)
            {
                int index = i * 6;
                int vi = i * 2;
                info.Indices[index + 0] = vi + 0;
                info.Indices[index + 1] = vi + 1;
                info.Indices[index + 2] = vi + 2;

                info.Indices[index + 3] = vi + 2;
                info.Indices[index + 4] = vi + 1;
                info.Indices[index + 5] = vi + 3;
            }
        }
    }
}
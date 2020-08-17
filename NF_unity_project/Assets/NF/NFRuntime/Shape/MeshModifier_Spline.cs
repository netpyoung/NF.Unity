using NFRuntime.Spline;
using UnityEngine;
using UnityEngine.Assertions;

namespace NFRuntime.Shape
{
    public partial class MeshModifier
    {
        public int FillSplineVUCI2(Spliner spliner, Color colorStart, Color colorEnd, ref MeshInfo meshInfo)
        {
            Assert.IsNotNull(meshInfo.Vertices);
            Assert.IsNotNull(meshInfo.UVs);
            Assert.IsNotNull(meshInfo.Colors);
            Assert.IsNotNull(meshInfo.Indices);

            int line = spliner.GetLineCount();
            Assert.AreEqual(meshInfo.Vertices.Length, line * 2);
            Assert.AreEqual(meshInfo.UVs.Length, line * 2);
            Assert.AreEqual(meshInfo.Colors.Length, line * 2);
            Assert.AreEqual(meshInfo.Indices.Length, (line - 1) * 6);

            float addPerc = 1f / line;
            float acc = 0;
            for (int i = 0; i < line; ++i)
            {
                (Vector3 p1, Vector3 up) = spliner.Interpolate(acc);
                Vector3 width = (up * 0.5f);
                Vector3 p0 = p1 - width;
                Vector3 p2 = p1 + width;

                // p0                    p2
                // |---------------------|
                int index = (i * 2);
                meshInfo.Vertices[index + 0] = p0;
                meshInfo.Vertices[index + 1] = p2;

                meshInfo.UVs[index + 0] = new Vector2(0, acc);
                meshInfo.UVs[index + 1] = new Vector2(1, acc);

                Color c = Color.Lerp(colorStart, colorEnd, acc);
                meshInfo.Colors[index + 0] = c;
                meshInfo.Colors[index + 1] = c;

                acc += addPerc;
            }

            for (int i = 0; i < line - 1; ++i)
            {
                int ci = i * 2;
                int ni = ci + 2;

                int index = i * 6;

                // c0                    c1
                // 2---------------------|
                // |          |          |
                // 0---------------------1
                // n0                    n1
                meshInfo.Indices[index + 0] = ni + 0;
                meshInfo.Indices[index + 1] = ni + 1;
                meshInfo.Indices[index + 2] = ci + 0;

                // c0                    c1
                // 5---------------------4
                // |          |          |
                // |---------------------3
                // n0                    n1
                meshInfo.Indices[index + 3] = ni + 1;
                meshInfo.Indices[index + 4] = ci + 1;
                meshInfo.Indices[index + 5] = ci + 0;
            }
            return line;
        }

        public int FillSplineVUCI3(Spliner spliner, Color colorStart, Color colorEnd, ref MeshInfo meshInfo)
        {
            Assert.IsNotNull(meshInfo.Vertices);
            Assert.IsNotNull(meshInfo.UVs);
            Assert.IsNotNull(meshInfo.Colors);
            Assert.IsNotNull(meshInfo.Indices);

            int line = spliner.GetLineCount();
            Assert.AreEqual(meshInfo.Vertices.Length, line * 3);
            Assert.AreEqual(meshInfo.UVs.Length, line * 3);
            Assert.AreEqual(meshInfo.Colors.Length, line * 3);
            Assert.AreEqual(meshInfo.Indices.Length, (line - 1) * 12);

            float addPerc = 1f / line;
            float acc = 0;
            for (int i = 0; i < line; ++i)
            {
                (Vector3 p1, Vector3 up) = spliner.Interpolate(acc);
                Vector3 width = (up * 0.5f);
                Vector3 p0 = p1 - width;
                Vector3 p2 = p1 + width;

                // p0         p1         p2
                // |----------|----------|
                int index = (i * 3);
                meshInfo.Vertices[index + 0] = p0;
                meshInfo.Vertices[index + 1] = p1;
                meshInfo.Vertices[index + 2] = p2;

                meshInfo.UVs[index + 0] = new Vector2(0, acc);
                meshInfo.UVs[index + 1] = new Vector2(0.5f, acc);
                meshInfo.UVs[index + 2] = new Vector2(1, acc);

                Color c = Color.Lerp(colorStart, colorEnd, acc);
                meshInfo.Colors[index + 0] = c;
                meshInfo.Colors[index + 1] = c;
                meshInfo.Colors[index + 2] = c;

                acc += addPerc;
            }

            for (int i = 0; i < line - 1; ++i)
            {
                int ci = i * 3;
                int ni = ci + 3;

                int index = i * 12;

                // c0         c1         c2
                // 2---------------------|
                // |          |          |
                // 0----------1----------|
                // n0         n1         n2
                meshInfo.Indices[index + 0] = ni + 0;
                meshInfo.Indices[index + 1] = ni + 1;
                meshInfo.Indices[index + 2] = ci + 0;

                // c0         c1         c2
                // 5----------4----------|
                // |          |          |
                // |----------3----------|
                // n0         n1         n2
                meshInfo.Indices[index + 3] = ni + 1;
                meshInfo.Indices[index + 4] = ci + 1;
                meshInfo.Indices[index + 5] = ci + 0;

                // c0         c1         c2
                // |----------8----------|
                // |          |          |
                // |----------6----------7
                // n0         n1         n2
                meshInfo.Indices[index + 6] = ni + 1;
                meshInfo.Indices[index + 7] = ni + 2;
                meshInfo.Indices[index + 8] = ci + 1;

                // c0         c1         c2
                // |----------11---------10
                // |          |          |
                // |----------|----------9
                // n0         n1         n2
                meshInfo.Indices[index + 9] = ni + 2;
                meshInfo.Indices[index + 10] = ci + 2;
                meshInfo.Indices[index + 11] = ci + 1;
            }
            return line;
        }

        // ==============
        public int FillSplineVUCI2(Spliner spliner, Color colorStart, Color colorEnd, ref NativeMeshInfo meshInfo)
        {
            Assert.IsTrue(meshInfo.Vertices.IsCreated);
            Assert.IsTrue(meshInfo.UVs.IsCreated);
            Assert.IsTrue(meshInfo.Colors.IsCreated);
            Assert.IsTrue(meshInfo.Indices.IsCreated);

            int line = spliner.GetLineCount();
            Assert.AreEqual(meshInfo.Vertices.Length, line * 2);
            Assert.AreEqual(meshInfo.UVs.Length, line * 2);
            Assert.AreEqual(meshInfo.Colors.Length, line * 2);
            Assert.AreEqual(meshInfo.Indices.Length, (line - 1) * 6);

            float addPerc = 1f / line;
            float acc = 0;
            for (int i = 0; i < line; ++i)
            {
                (Vector3 p1, Vector3 up) = spliner.Interpolate(acc);
                Vector3 width = (up * 0.5f);
                Vector3 p0 = p1 - width;
                Vector3 p2 = p1 + width;

                // p0                    p2
                // |---------------------|
                int index = (i * 2);
                meshInfo.Vertices[index + 0] = p0;
                meshInfo.Vertices[index + 1] = p2;

                meshInfo.UVs[index + 0] = new Vector2(0, acc);
                meshInfo.UVs[index + 1] = new Vector2(1, acc);

                Color c = Color.Lerp(colorStart, colorEnd, acc);
                meshInfo.Colors[index + 0] = c;
                meshInfo.Colors[index + 1] = c;

                acc += addPerc;
            }

            for (int i = 0; i < line - 1; ++i)
            {
                int ci = i * 2;
                int ni = ci + 2;

                int index = i * 6;

                // c0                    c1
                // 2---------------------|
                // |          |          |
                // 0---------------------1
                // n0                    n1
                meshInfo.Indices[index + 0] = ni + 0;
                meshInfo.Indices[index + 1] = ni + 1;
                meshInfo.Indices[index + 2] = ci + 0;

                // c0                    c1
                // 5---------------------4
                // |          |          |
                // |---------------------3
                // n0                    n1
                meshInfo.Indices[index + 3] = ni + 1;
                meshInfo.Indices[index + 4] = ci + 1;
                meshInfo.Indices[index + 5] = ci + 0;
            }
            return line;
        }

        public int FillSplineVUCI3(Spliner spliner, Color colorStart, Color colorEnd, ref NativeMeshInfo meshInfo)
        {
            Assert.IsTrue(meshInfo.Vertices.IsCreated);
            Assert.IsTrue(meshInfo.UVs.IsCreated);
            Assert.IsTrue(meshInfo.Colors.IsCreated);
            Assert.IsTrue(meshInfo.Indices.IsCreated);

            int line = spliner.GetLineCount();
            Assert.AreEqual(meshInfo.Vertices.Length, line * 3);
            Assert.AreEqual(meshInfo.UVs.Length, line * 3);
            Assert.AreEqual(meshInfo.Colors.Length, line * 3);
            Assert.AreEqual(meshInfo.Indices.Length, (line - 1) * 12);

            float addPerc = 1f / line;
            float acc = 0;
            for (int i = 0; i < line; ++i)
            {
                (Vector3 p1, Vector3 up) = spliner.Interpolate(acc);
                Vector3 width = (up * 0.5f);
                Vector3 p0 = p1 - width;
                Vector3 p2 = p1 + width;

                // p0         p1         p2
                // |----------|----------|
                int index = (i * 3);
                meshInfo.Vertices[index + 0] = p0;
                meshInfo.Vertices[index + 1] = p1;
                meshInfo.Vertices[index + 2] = p2;

                meshInfo.UVs[index + 0] = new Vector2(0, acc);
                meshInfo.UVs[index + 1] = new Vector2(0.5f, acc);
                meshInfo.UVs[index + 2] = new Vector2(1, acc);

                Color c = Color.Lerp(colorStart, colorEnd, acc);
                meshInfo.Colors[index + 0] = c;
                meshInfo.Colors[index + 1] = c;
                meshInfo.Colors[index + 2] = c;

                acc += addPerc;
            }

            for (int i = 0; i < line - 1; ++i)
            {
                int ci = i * 3;
                int ni = ci + 3;

                int index = i * 12;

                // c0         c1         c2
                // 2---------------------|
                // |          |          |
                // 0----------1----------|
                // n0         n1         n2
                meshInfo.Indices[index + 0] = ni + 0;
                meshInfo.Indices[index + 1] = ni + 1;
                meshInfo.Indices[index + 2] = ci + 0;

                // c0         c1         c2
                // 5----------4----------|
                // |          |          |
                // |----------3----------|
                // n0         n1         n2
                meshInfo.Indices[index + 3] = ni + 1;
                meshInfo.Indices[index + 4] = ci + 1;
                meshInfo.Indices[index + 5] = ci + 0;

                // c0         c1         c2
                // |----------8----------|
                // |          |          |
                // |----------6----------7
                // n0         n1         n2
                meshInfo.Indices[index + 6] = ni + 1;
                meshInfo.Indices[index + 7] = ni + 2;
                meshInfo.Indices[index + 8] = ci + 1;

                // c0         c1         c2
                // |----------11---------10
                // |          |          |
                // |----------|----------9
                // n0         n1         n2
                meshInfo.Indices[index + 9] = ni + 2;
                meshInfo.Indices[index + 10] = ci + 2;
                meshInfo.Indices[index + 11] = ci + 1;
            }
            return line;
        }

        // ==================================
        public int FillSplineVUC2(Spliner spliner, Color colorStart, Color colorEnd, ref NativeMeshInfo meshInfo)
        {
            Assert.IsTrue(meshInfo.Vertices.IsCreated);
            Assert.IsTrue(meshInfo.UVs.IsCreated);
            Assert.IsTrue(meshInfo.Colors.IsCreated);

            int line = spliner.GetLineCount();
            Assert.AreEqual(meshInfo.Vertices.Length, line * 2);
            Assert.AreEqual(meshInfo.UVs.Length, line * 2);
            Assert.AreEqual(meshInfo.Colors.Length, line * 2);

            float addPerc = 1f / line;
            float acc = 0;
            for (int i = 0; i < line; ++i)
            {
                (Vector3 p1, Vector3 up) = spliner.Interpolate(acc);
                Vector3 width = (up * 0.5f);
                Vector3 p0 = p1 - width;
                Vector3 p2 = p1 + width;

                // p0                    p2
                // |---------------------|
                int index = (i * 2);
                meshInfo.Vertices[index + 0] = p0;
                meshInfo.Vertices[index + 1] = p2;

                meshInfo.UVs[index + 0] = new Vector2(0, acc);
                meshInfo.UVs[index + 1] = new Vector2(1, acc);

                Color c = Color.Lerp(colorStart, colorEnd, acc);
                meshInfo.Colors[index + 0] = c;
                meshInfo.Colors[index + 1] = c;

                acc += addPerc;
            }
            return line;
        }

        public int FillSplineVUC3(Spliner spliner, Color colorStart, Color colorEnd, ref NativeMeshInfo meshInfo)
        {
            Assert.IsTrue(meshInfo.Vertices.IsCreated);
            Assert.IsTrue(meshInfo.UVs.IsCreated);
            Assert.IsTrue(meshInfo.Colors.IsCreated);

            int line = spliner.GetLineCount();
            Assert.IsTrue(meshInfo.Vertices.Length >= line * 3);
            Assert.IsTrue(meshInfo.UVs.Length >= line * 3);
            Assert.IsTrue(meshInfo.Colors.Length >= line * 3);

            float addPerc = 1f / line;
            float acc = 0;
            for (int i = 0; i < line; ++i)
            {
                (Vector3 p1, Vector3 up) = spliner.Interpolate(acc);
                Vector3 width = (up * 0.5f);
                Vector3 p0 = p1 - width;
                Vector3 p2 = p1 + width;

                // p0         p1         p2
                // |----------|----------|
                int index = (i * 3);
                meshInfo.Vertices[index + 0] = p0;
                meshInfo.Vertices[index + 1] = p1;
                meshInfo.Vertices[index + 2] = p2;

                meshInfo.UVs[index + 0] = new Vector2(0, acc);
                meshInfo.UVs[index + 1] = new Vector2(0.5f, acc);
                meshInfo.UVs[index + 2] = new Vector2(1, acc);

                Color c = Color.Lerp(colorStart, colorEnd, acc);
                meshInfo.Colors[index + 0] = c;
                meshInfo.Colors[index + 1] = c;
                meshInfo.Colors[index + 2] = c;

                acc += addPerc;
            }
            return line;
        }

        public int FillSplineVU2(Spliner spliner, Color colorStart, Color colorEnd, ref NativeMeshInfo meshInfo)
        {
            Assert.IsTrue(meshInfo.Vertices.IsCreated);
            Assert.IsTrue(meshInfo.UVs.IsCreated);
            Assert.IsTrue(meshInfo.Colors.IsCreated);

            int line = spliner.GetLineCount();
            Assert.AreEqual(meshInfo.Vertices.Length, line * 2);
            Assert.AreEqual(meshInfo.UVs.Length, line * 2);
            Assert.AreEqual(meshInfo.Colors.Length, line * 2);

            float addPerc = 1f / line;
            float acc = 0;
            for (int i = 0; i < line; ++i)
            {
                (Vector3 p1, Vector3 up) = spliner.Interpolate(acc);
                Vector3 width = (up * 0.5f);
                Vector3 p0 = p1 - width;
                Vector3 p2 = p1 + width;

                // p0                    p2
                // |---------------------|
                int index = (i * 2);
                meshInfo.Vertices[index + 0] = p0;
                meshInfo.Vertices[index + 1] = p2;

                meshInfo.UVs[index + 0] = new Vector2(0, acc);
                meshInfo.UVs[index + 1] = new Vector2(1, acc);

                acc += addPerc;
            }
            return line;
        }

        public int FillSplineVU3(Spliner spliner, Color colorStart, Color colorEnd, ref NativeMeshInfo meshInfo)
        {
            Assert.IsTrue(meshInfo.Vertices.IsCreated);
            Assert.IsTrue(meshInfo.UVs.IsCreated);
            Assert.IsTrue(meshInfo.Colors.IsCreated);

            int line = spliner.GetLineCount();
            Assert.AreEqual(meshInfo.Vertices.Length, line * 3);
            Assert.AreEqual(meshInfo.UVs.Length, line * 3);
            Assert.AreEqual(meshInfo.Colors.Length, line * 3);

            float addPerc = 1f / line;
            float acc = 0;
            for (int i = 0; i < line; ++i)
            {
                (Vector3 p1, Vector3 up) = spliner.Interpolate(acc);
                Vector3 width = (up * 0.5f);
                Vector3 p0 = p1 - width;
                Vector3 p2 = p1 + width;

                // p0         p1         p2
                // |----------|----------|
                int index = (i * 3);
                meshInfo.Vertices[index + 0] = p0;
                meshInfo.Vertices[index + 1] = p1;
                meshInfo.Vertices[index + 2] = p2;

                meshInfo.UVs[index + 0] = new Vector2(0, acc);
                meshInfo.UVs[index + 1] = new Vector2(0.5f, acc);
                meshInfo.UVs[index + 2] = new Vector2(1, acc);

                acc += addPerc;
            }
            return line;
        }

        public void FillSplineI2(ref NativeMeshInfo meshInfo)
        {
            Assert.IsTrue(meshInfo.Indices.IsCreated);

            int indexLoopCount = meshInfo.Indices.Length / 6;
            for (int i = 0; i < indexLoopCount; ++i)
            {
                int ci = i * 2;
                int ni = ci + 2;

                int index = i * 6;

                // c0                    c1
                // 2---------------------|
                // |          |          |
                // 0---------------------1
                // n0                    n1
                meshInfo.Indices[index + 0] = ni + 0;
                meshInfo.Indices[index + 1] = ni + 1;
                meshInfo.Indices[index + 2] = ci + 0;

                // c0                    c1
                // 5---------------------4
                // |          |          |
                // |---------------------3
                // n0                    n1
                meshInfo.Indices[index + 3] = ni + 1;
                meshInfo.Indices[index + 4] = ci + 1;
                meshInfo.Indices[index + 5] = ci + 0;
            }
        }

        public void FillSplineI3(ref NativeMeshInfo meshInfo)
        {
            Assert.IsTrue(meshInfo.Indices.IsCreated);

            int indexLoopCount = meshInfo.Indices.Length / 12;
            for (int i = 0; i < indexLoopCount; ++i)
            {
                int ci = i * 3;
                int ni = ci + 3;

                int index = i * 12;

                // c0         c1         c2
                // 2---------------------|
                // |          |          |
                // 0----------1----------|
                // n0         n1         n2
                meshInfo.Indices[index + 0] = ni + 0;
                meshInfo.Indices[index + 1] = ni + 1;
                meshInfo.Indices[index + 2] = ci + 0;

                // c0         c1         c2
                // 5----------4----------|
                // |          |          |
                // |----------3----------|
                // n0         n1         n2
                meshInfo.Indices[index + 3] = ni + 1;
                meshInfo.Indices[index + 4] = ci + 1;
                meshInfo.Indices[index + 5] = ci + 0;

                // c0         c1         c2
                // |----------8----------|
                // |          |          |
                // |----------6----------7
                // n0         n1         n2
                meshInfo.Indices[index + 6] = ni + 1;
                meshInfo.Indices[index + 7] = ni + 2;
                meshInfo.Indices[index + 8] = ci + 1;

                // c0         c1         c2
                // |----------11---------10
                // |          |          |
                // |----------|----------9
                // n0         n1         n2
                meshInfo.Indices[index + 9] = ni + 2;
                meshInfo.Indices[index + 10] = ci + 2;
                meshInfo.Indices[index + 11] = ci + 1;
            }
        }

        public void FillSplineCI2(Color color, ref NativeMeshInfo meshInfo)
        {
            Assert.IsTrue(meshInfo.Colors.IsCreated);
            Assert.IsTrue(meshInfo.Indices.IsCreated);

            for (int i = 0; i < meshInfo.Colors.Length; ++i)
            {
                meshInfo.Colors[i] = color;
            }

            int indexLoopCount = meshInfo.Indices.Length / 6;
            for (int i = 0; i < indexLoopCount; ++i)
            {
                int ci = i * 2;
                int ni = ci + 2;

                int index = i * 6;

                // c0                    c1
                // 2---------------------|
                // |          |          |
                // 0---------------------1
                // n0                    n1
                meshInfo.Indices[index + 0] = ni + 0;
                meshInfo.Indices[index + 1] = ni + 1;
                meshInfo.Indices[index + 2] = ci + 0;

                // c0                    c1
                // 5---------------------4
                // |          |          |
                // |---------------------3
                // n0                    n1
                meshInfo.Indices[index + 3] = ni + 1;
                meshInfo.Indices[index + 4] = ci + 1;
                meshInfo.Indices[index + 5] = ci + 0;
            }
        }

        public void FillSplineCI3(Color color, ref NativeMeshInfo meshInfo)
        {
            Assert.IsTrue(meshInfo.Colors.IsCreated);
            Assert.IsTrue(meshInfo.Indices.IsCreated);
            for (int i = 0; i < meshInfo.Colors.Length; ++i)
            {
                meshInfo.Colors[i] = color;
            }

            int indexLoopCount = meshInfo.Indices.Length / 12;
            for (int i = 0; i < indexLoopCount; ++i)
            {
                int ci = i * 3;
                int ni = ci + 3;

                int index = i * 12;

                // c0         c1         c2
                // 2---------------------|
                // |          |          |
                // 0----------1----------|
                // n0         n1         n2
                meshInfo.Indices[index + 0] = ni + 0;
                meshInfo.Indices[index + 1] = ni + 1;
                meshInfo.Indices[index + 2] = ci + 0;

                // c0         c1         c2
                // 5----------4----------|
                // |          |          |
                // |----------3----------|
                // n0         n1         n2
                meshInfo.Indices[index + 3] = ni + 1;
                meshInfo.Indices[index + 4] = ci + 1;
                meshInfo.Indices[index + 5] = ci + 0;

                // c0         c1         c2
                // |----------8----------|
                // |          |          |
                // |----------6----------7
                // n0         n1         n2
                meshInfo.Indices[index + 6] = ni + 1;
                meshInfo.Indices[index + 7] = ni + 2;
                meshInfo.Indices[index + 8] = ci + 1;

                // c0         c1         c2
                // |----------11---------10
                // |          |          |
                // |----------|----------9
                // n0         n1         n2
                meshInfo.Indices[index + 9] = ni + 2;
                meshInfo.Indices[index + 10] = ci + 2;
                meshInfo.Indices[index + 11] = ci + 1;
            }
        }
    }
}
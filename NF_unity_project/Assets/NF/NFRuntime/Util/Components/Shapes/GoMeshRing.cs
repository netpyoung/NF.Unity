using UnityEngine;

namespace NFRuntime.Util.Components.Shapes
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class GoMeshRing : MonoBehaviour
    {
        public float InnerRadious;
        public float OuterRadious;
        public int Angle;

        public Vector2 Offset;

        public MeshRenderer MeshRenderer { get; private set; }
        public MeshFilter MeshFilter { get; private set; }
        public Mesh Mesh { get; private set; }
        bool IsAwakend;
        void Awake()
        {
            this.MeshRenderer = GetComponent<MeshRenderer>();
            this.MeshFilter = GetComponent<MeshFilter>();
            this.Mesh = new Mesh();
            this.MeshFilter.sharedMesh = Mesh;
            IsAwakend = true;
            Init();
        }
        void Init()
        {
            var angle = this.Angle;
            var initialAngle = -angle / 2;
            var seg = (angle / 10);
            int numVerts = 2 + (2 * seg);
            int numIndices = 6 * seg;

            Vector3[] verts = new Vector3[numVerts];
            Vector2[] uvs = new Vector2[numVerts];
            int[] tris = new int[numIndices];

            float detailAngle = 10;

            for (int i = 0; i <= seg; ++i)
            {
                var v = Quaternion.AngleAxis(initialAngle + (detailAngle * i), Vector3.up) * Vector3.forward;
                int index = i * 2;
                verts[index + 0] = v * InnerRadious;
                verts[index + 1] = v * (InnerRadious + OuterRadious);

                float normedHorizontal = (verts[index].x + 1.0f) * 0.5f;
                float normedVertical = (verts[index].x + 1.0f) * 0.5f;
                uvs[index + 0] = new Vector2(normedHorizontal, normedVertical);
                uvs[index + 1] = new Vector2(normedHorizontal, normedVertical);
            }

            for (int i = 0; i < seg; ++i)
            {
                int index = i * 6;
                int vi = i * 2;
                tris[index + 0] = vi + 0;
                tris[index + 1] = vi + 1;
                tris[index + 2] = vi + 2;

                tris[index + 3] = vi + 2;
                tris[index + 4] = vi + 1;
                tris[index + 5] = vi + 3;
            }

            Mesh.vertices = verts;
            Mesh.triangles = tris;
            //Mesh.uv = uvs;
        }

        private void OnValidate()
        {
            if (!IsAwakend)
            {
                return;
            }
            Init();
        }
    }
}
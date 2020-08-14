using UnityEngine;

namespace NFRuntime.Util.Components.Shapes
{
    public class GoMeshCircle : MonoBehaviour
    {
        public float Radious;
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
            int numVerts = seg + 2;
            int numIndices = 3 * seg;

            Vector3[] verts = new Vector3[numVerts];
            Vector2[] uvs = new Vector2[numVerts];
            int[] tris = new int[numIndices];

            verts[0] = Vector3.zero;
            uvs[0] = new Vector2(0.5f, 0.5f);

            float detailAngle = 10;

            for (int i = 1; i < numVerts; ++i)
            {
                verts[i] = Quaternion.AngleAxis(initialAngle + (detailAngle * (i - 1)), Vector3.up) * Vector3.forward;

                float normedHorizontal = (verts[i].x + 1.0f) * 0.5f;
                float normedVertical = (verts[i].x + 1.0f) * 0.5f;
                uvs[i] = new Vector2(normedHorizontal, normedVertical);
            }

            for (int i = 0; i < seg; ++i)
            {
                int index = i * 3;
                tris[index + 0] = 0;
                tris[index + 1] = i + 1;
                tris[index + 2] = i + 2;
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

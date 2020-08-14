using UnityEngine;

namespace NFRuntime.Util.Components.Shapes
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class GoMeshRectangle : MonoBehaviour
    {
        public float Width;
        public float Height;
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

        private void Init()
        {
            var vertices = new Vector3[4];
            var uvs = new Vector2[4];
            var indices = new int[6];
            float halfWidth = Width / 2;
            float halfHeight = Height / 2;

            vertices[0] = new Vector3(-halfWidth, 0, halfHeight);
            vertices[1] = new Vector3(halfWidth, 0, halfHeight);
            vertices[2] = new Vector3(-halfWidth, 0, -halfHeight);
            vertices[3] = new Vector3(halfWidth, 0, -halfHeight);

            uvs[0] = new Vector2(0, 0);
            uvs[1] = new Vector2(1, 0);
            uvs[2] = new Vector2(0, 1);
            uvs[3] = new Vector2(1, 1);

            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 3;

            indices[3] = 3;
            indices[4] = 2;
            indices[5] = 0;

            Mesh.vertices = vertices;
            Mesh.uv = uvs;
            Mesh.triangles = indices;
            this.Mesh.RecalculateBounds();
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
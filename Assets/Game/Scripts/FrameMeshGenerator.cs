using UnityEngine;

namespace Game
{
    public class FrameMeshGenerator : MonoBehaviour
    {
        [SerializeField] private Color _frameColor = Color.white;
        [SerializeField] private float _borderThickness = 0.1f;

        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;

        private void OnValidate()
        {
            GenerateFrameMesh();
        }

        private void GenerateFrameMesh()
        {
            if (_meshFilter == null)
                _meshFilter = gameObject.AddComponent<MeshFilter>();

            if (_meshRenderer == null)
                _meshRenderer = gameObject.AddComponent<MeshRenderer>();

            Mesh mesh = new Mesh();

            // Define the vertices for the frame
            Vector3[] vertices = new Vector3[8];
            vertices[0] = new Vector3(0, 0, 0);
            vertices[1] = new Vector3(1, 0, 0);
            vertices[2] = new Vector3(1, 1, 0);
            vertices[3] = new Vector3(0, 1, 0);
            vertices[4] = new Vector3(_borderThickness, _borderThickness, 0);
            vertices[5] = new Vector3(1 - _borderThickness, _borderThickness, 0);
            vertices[6] = new Vector3(1 - _borderThickness, 1 - _borderThickness, 0);
            vertices[7] = new Vector3(_borderThickness, 1 - _borderThickness, 0);

            // Define the triangles for the frame
            int[] triangles = new int[]
            {
                // Outer frame
                0, 1, 4,
                1, 5, 4,
                1, 2, 5,
                2, 6, 5,
                2, 3, 6,
                3, 7, 6,
                3, 0, 7,
                0, 4, 7
            };

            // Assign vertices and triangles to the mesh
            mesh.vertices = vertices;
            mesh.triangles = triangles;

            // Recalculate normals and bounds
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            // Assign the mesh to the MeshFilter
            _meshFilter.mesh = mesh;

            // Create a URP-compatible material
            Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            material.color = _frameColor;

            // Assign the material to the MeshRenderer
            _meshRenderer.material = material;
        }
    }
}
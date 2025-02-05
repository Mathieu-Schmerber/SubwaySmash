using System;
using Game.Entities.Ai;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Game.Systems.Waypoint
{
    public class Exit : MonoBehaviour
    {
        private static readonly int Open = Animator.StringToHash("Open");
        [SerializeField] private float _radius = .2f;
        [SerializeField] private Vector3 _offset = Vector3.zero; // Offset field
        [SerializeField] private bool _debug = true;
        private float _arrowLength = 1f; // Arrow length
        private float _arrowHeadLength = 0.3f; // Arrowhead length
        private float _arrowHeadRadius = 0.11f; // Arrowhead base radius

        public static event Action OnEscaped; 
        
        private Mesh _arrowHeadMesh = null;
        private Animator _animator;
        private MMF_Player _feedback;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _feedback = GetComponent<MMF_Player>();
        }

        private void Update() => CheckForAiBrain();

        private void CheckForAiBrain()
        {
            var hits = Physics.OverlapSphere(transform.position + _offset, _radius);

            foreach (var hit in hits)
            {
                var aiBrain = hit.GetComponent<AiBrain>();
                if (aiBrain != null && TargetIsThis(aiBrain.Target))
                {
                    _animator.SetTrigger(Open);
                    _feedback.PlayFeedbacks();
                    OnEscaped?.Invoke();
                    Destroy(aiBrain.gameObject);
                }
            }
        }

        private bool TargetIsThis(Transform aiBrainTarget)
            => Vector3.Distance(transform.position + _offset, aiBrainTarget.position) <= _radius;

        private void OnDrawGizmos()
        {
            if (!_debug)
                return;
            
            _arrowHeadMesh = CreateConeMesh(_arrowHeadRadius, _arrowHeadLength, 12);
            
            // Draw sphere
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position + Vector3.Scale(new Vector3(1, .001f, 1), _offset), new Vector3(1, 0, 1) * _radius);

            // Draw arrow pointing in the forward direction
            Vector3 arrowStart = transform.position + _offset;
            Vector3 arrowEnd = arrowStart + transform.forward * _arrowLength;

            // Draw arrow shaft
            Gizmos.DrawLine(arrowStart, arrowEnd);

            // Draw 3D arrowhead using Gizmos.DrawMesh
            if (_arrowHeadMesh != null)
            {
                Gizmos.DrawMesh(_arrowHeadMesh, arrowEnd, Quaternion.LookRotation(transform.forward));
            }
        }

        private Mesh CreateConeMesh(float radius, float height, int segments)
        {
            var mesh = new Mesh();

            // Vertices
            Vector3[] vertices = new Vector3[segments + 2];
            vertices[0] = Vector3.zero; // Cone tip
            vertices[1] = Vector3.forward * height; // Center of the base

            float angleStep = 2 * Mathf.PI / segments;

            for (int i = 0; i < segments; i++)
            {
                float angle = i * angleStep;
                float x = Mathf.Cos(angle) * radius;
                float y = Mathf.Sin(angle) * radius;
                vertices[i + 2] = new Vector3(x, y, 0);
            }

            // Triangles
            int[] triangles = new int[segments * 3 * 2];

            // Side triangles
            for (int i = 0; i < segments; i++)
            {
                int current = i + 2;
                int next = i + 3 > segments + 1 ? 2 : i + 3;

                triangles[i * 3] = 0;       // Cone tip
                triangles[i * 3 + 1] = next;
                triangles[i * 3 + 2] = current;
            }

            // Base triangles
            int offset = segments * 3;
            for (int i = 0; i < segments; i++)
            {
                int current = i + 2;
                int next = i + 3 > segments + 1 ? 2 : i + 3;

                triangles[offset + i * 3] = 1; // Center of the base
                triangles[offset + i * 3 + 1] = current;
                triangles[offset + i * 3 + 2] = next;
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}

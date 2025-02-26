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
        [SerializeField] private Vector3 _offset = Vector3.zero;
        [SerializeField] private bool _debug = true;
        
        private const float ARROW_LENGTH = 1f;
        private const float ARROW_HEAD_LENGTH = 0.3f;
        private const float ARROW_HEAD_RADIUS = 0.11f;

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
            var positionWithOffset = transform.position + transform.TransformDirection(_offset);
            var hits = Physics.OverlapSphere(positionWithOffset, _radius);

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
        {
            var positionWithOffset = transform.position + transform.TransformDirection(_offset);
            return Vector3.Distance(positionWithOffset, aiBrainTarget.position) <= _radius;
        }

        private void OnDrawGizmos()
        {
            if (!_debug)
                return;

            _arrowHeadMesh = CreateConeMesh(ARROW_HEAD_RADIUS, ARROW_HEAD_LENGTH, 12);

            // Draw sphere
            Gizmos.color = Color.cyan;
            var positionWithOffset = transform.position + transform.TransformDirection(_offset);
            Gizmos.DrawWireCube(positionWithOffset, new Vector3(1, 0, 1) * _radius);

            // Draw arrow pointing in the forward direction
            var arrowStart = positionWithOffset;
            var arrowEnd = arrowStart + transform.forward * ARROW_LENGTH;

            // Draw arrow shaft
            Gizmos.DrawLine(arrowStart, arrowEnd);

            // Draw 3D arrowhead using Gizmos.DrawMesh
            if (_arrowHeadMesh != null)
                Gizmos.DrawMesh(_arrowHeadMesh, arrowEnd, Quaternion.LookRotation(transform.forward));
        }


        private Mesh CreateConeMesh(float radius, float height, int segments)
        {
            var mesh = new Mesh();

            // Vertices
            var vertices = new Vector3[segments + 2];
            vertices[0] = Vector3.zero; // Cone tip
            vertices[1] = Vector3.forward * height; // Center of the base

            var angleStep = 2 * Mathf.PI / segments;

            for (var i = 0; i < segments; i++)
            {
                var angle = i * angleStep;
                var x = Mathf.Cos(angle) * radius;
                var y = Mathf.Sin(angle) * radius;
                vertices[i + 2] = new Vector3(x, y, 0);
            }

            // Triangles
            var triangles = new int[segments * 3 * 2];

            // Side triangles
            for (var i = 0; i < segments; i++)
            {
                var current = i + 2;
                var next = i + 3 > segments + 1 ? 2 : i + 3;

                triangles[i * 3] = 0;       // Cone tip
                triangles[i * 3 + 1] = next;
                triangles[i * 3 + 2] = current;
            }

            // Base triangles
            var offset = segments * 3;
            for (var i = 0; i < segments; i++)
            {
                var current = i + 2;
                var next = i + 3 > segments + 1 ? 2 : i + 3;

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

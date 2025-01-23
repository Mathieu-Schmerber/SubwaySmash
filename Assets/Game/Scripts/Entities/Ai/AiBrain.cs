using System;
using System.Linq;
using Game.Inputs;
using LemonInc.Core.Input;
using LemonInc.Core.Utilities.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Entities.Ai
{
    public class AiBrain : MonoBehaviour, IInputProvider
    {
        [Header("Ai Avoidance")]
        [SerializeField] private int _avoidancePrecision = 3;
        [SerializeField] private float _avoidanceAngle = 20;
        [SerializeField] private float _avoidanceRadius = .2f;
        [SerializeField] private LayerMask _layerMask;
        
        [Header("Patrol")]
        [SerializeField] private WaypointPath _waypointPath;
        
        private Transform _target;
        private NavMeshPath _path;
        private int _wayPointIndex = 0;

        private Vector3 _movementDirection;
        public Vector3 MovementDirection => _movementDirection;
        
        public Vector3 AimDirection => MovementDirection.normalized;
        public InputState Dash { get; }
        public InputState Push { get; }

        public Transform Target => _target;
        
        private void Awake()
        {
            _path = new NavMeshPath();
        }

        private void FixedUpdate()
        {
            if (_target == null)
            {
                _movementDirection = Vector3.zero;
                return;
            }

            if (HasWaypoints() && _waypointPath.Contains(_target)) 
                _target = _waypointPath.GetCurrentOrNewPoint(transform, _target);
            
            NavMesh.CalculatePath(transform.position, _target.position, NavMesh.AllAreas, _path);
            
            var targetPos = _path.corners.Length <= 1 ? _target.position : _path.corners[1];
            var direction = (targetPos - transform.position).normalized;
            var adjustedDirection = CalculateAvoidanceDirection(direction, Vector3.Distance(transform.position, targetPos));

            _movementDirection = adjustedDirection;
        }

        private Vector3 CalculateAvoidanceDirection(Vector3 direction, float distanceToTarget)
        {
            if (!Physics.Raycast(transform.position, direction, out var notClear, distanceToTarget, _layerMask))
                return direction;
            
            var bestDirection = direction;
            var bestScore = float.MinValue;
            var space = _avoidanceAngle / _avoidancePrecision;

            for (var i = -_avoidancePrecision; i <= _avoidancePrecision; i++)
            {
                var rotatedDirection = direction.Rotate(Vector3.up * space * i);
                var alignmentScore = Vector3.Dot(rotatedDirection, direction);

                if (Physics.Raycast(transform.position, rotatedDirection, out var hit, _avoidanceRadius, _layerMask))
                {
                    var obstaclePenalty = hit.distance / _avoidanceRadius;
                    var score = alignmentScore - (1 - obstaclePenalty);

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestDirection = rotatedDirection;
                    }
                }
                else
                {
                    var score = alignmentScore + 1;
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestDirection = rotatedDirection;
                    }
                }
            }

            return bestDirection;
        }

        public bool IsNavClear(Vector3 destination)
        {
            return !NavMesh.Raycast(transform.position, destination, out _, NavMesh.AllAreas);
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public bool HasWaypoints() => _waypointPath;
        
        public void ResumeFollowWaypoints()
        {
            if (!HasWaypoints())
                return;
            
            _wayPointIndex = _waypointPath.GetClosestIndexToPosition(transform.position);
            _target = _waypointPath.GetAt(_wayPointIndex);
        }

        private void OnDrawGizmosSelected()
        {
            var aim = Application.isPlaying ? AimDirection : transform.forward;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _avoidanceRadius);
            Gizmos.color = Color.blue;

            var space = _avoidanceAngle / _avoidancePrecision;
            for (var i = 0; i < _avoidancePrecision; i++)
            {
                var dir = aim.Rotate(Vector3.up * space * (i / 2f));
                Gizmos.DrawRay(transform.position + Vector3.up, dir * _avoidanceRadius);
                dir = aim.Rotate(Vector3.up * space * (-i / 2f));
                Gizmos.DrawRay(transform.position + Vector3.up, dir * _avoidanceRadius);
            }
        }
    }
}
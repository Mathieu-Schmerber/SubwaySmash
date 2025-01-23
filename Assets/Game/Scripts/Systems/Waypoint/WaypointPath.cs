using System.Collections.Generic;
using LemonInc.Core.Utilities.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Systems.Waypoint
{
    [ExecuteAlways]
    public class WaypointPath : MonoBehaviour
    {
        [Header("Debug")] [SerializeField, ReadOnly]
        private List<Transform> _wayPoints = new();

        [SerializeField] private Color _debugColor = Color.red;

        [Header("Pathfinding")] [SerializeField]
        private float _wayPointRadius = .1f;

        private void Awake()
        {
            UpdateWaypoints();
        }

        private void UpdateWaypoints()
        {
            _wayPoints.Clear();

            for (int i = 0; i < transform.childCount; i++)
            {
                _wayPoints.Add(transform.GetChild(i));
            }

            _lastChildCount = transform.childCount;
        }

#if UNITY_EDITOR

        private int _lastChildCount;

        private void Update()
        {
            if (!Application.isPlaying)
            {
                if (transform.childCount != _lastChildCount)
                {
                    UpdateWaypoints();
                }
            }
        }

#endif

        public int GetClosestIndexToPosition(Vector3 position)
        {
            var closestDistance = float.MaxValue;
            var closestIndex = 0;

            for (var i = 0; i < _wayPoints.Count; i++)
            {
                if (_wayPoints[i] == null) continue;

                var distance = Vector3.Distance(position, _wayPoints[i].position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIndex = i;
                }
            }

            return closestIndex;
        }

        public bool Contains(Transform tr) => _wayPoints.Contains(tr);

        public Transform GetCurrentOrNewPoint(Transform tr, Transform wayPoint)
        {
            if (!Contains(wayPoint))
                return wayPoint;

            if (Vector3.Distance(tr.position, wayPoint.position.WithY(tr.position.y)) <= _wayPointRadius)
            {
                var index = _wayPoints.IndexOf(wayPoint);
                index++;
                index %= _wayPoints.Count;
                return _wayPoints[index];
            }

            return wayPoint;
        }

        public Transform GetAt(int wayPointIndex) => _wayPoints[wayPointIndex];

        private void OnDrawGizmos()
        {
            if (_wayPoints != null && _wayPoints.Count > 0)
            {
                Gizmos.color = _debugColor;

                for (var i = 0; i < _wayPoints.Count; i++)
                {
                    if (_wayPoints[i] == null) continue;

                    var position = _wayPoints[i].position;
                    Gizmos.DrawSphere(position, _wayPointRadius);

                    var nextIndex = (i + 1) % _wayPoints.Count;
                    if (_wayPoints[nextIndex] != null)
                    {
                        var start = position;
                        var end = _wayPoints[nextIndex].position;

                        // Adjust the line to account for the sphere radius
                        var direction = (end - start).normalized;
                        var startOffset = start + direction * _wayPointRadius;
                        var endOffset = end - direction * _wayPointRadius;

                        // Draw the line
                        Gizmos.DrawLine(startOffset, endOffset);

                        // Draw the arrowhead at the `endOffset` position
                        var arrowDirection = (end - start).normalized; // Reverse the direction for the arrowhead
                        var right = Quaternion.LookRotation(arrowDirection) * Quaternion.Euler(0, 150, 0) *
                                    Vector3.forward * 0.2f;
                        var left = Quaternion.LookRotation(arrowDirection) * Quaternion.Euler(0, -150, 0) *
                                   Vector3.forward * 0.2f;

                        Gizmos.DrawRay(endOffset, right);
                        Gizmos.DrawRay(endOffset, left);
                    }
                }
            }
        }
    }
}
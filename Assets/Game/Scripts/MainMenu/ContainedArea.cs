using System;
using LemonInc.Core.Utilities.Extensions;
using UnityEngine;

namespace Game.MainMenu
{
    [RequireComponent(typeof(BoxCollider))]
    public class ContainedArea : MonoBehaviour
    {
        private BoxCollider _boxCollider;

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
            _boxCollider.isTrigger = true;
        }

        private void OnTriggerExit(Collider other)
        {
            var rb = other.attachedRigidbody;
            if (rb == null) return;

            var direction = rb.linearVelocity;
            if (direction == Vector3.zero) return;

            var position = other.transform.position;

            var localToWorld = _boxCollider.transform.localToWorldMatrix;
            var localPosition = localToWorld.inverse.MultiplyPoint3x4(position);
            var localDirection = localToWorld.inverse.MultiplyVector(direction);
            var localRay = new Ray(localPosition, -localDirection.normalized);
            var localBounds = new Bounds(_boxCollider.center, _boxCollider.size);

            if (localBounds.IntersectRay(localRay, out var localDistance))
            {
                var localIntersectionPoint = localRay.GetPoint(localDistance);
                var worldIntersectionPoint = localToWorld.MultiplyPoint3x4(localIntersectionPoint);

                Debug.Log($"Intersection Point (World): {worldIntersectionPoint}");
            }
        }

        private void OnTriggerStay(Collider other)
        {
            var rb = other.attachedRigidbody;
            if (rb == null) return;

            var direction = rb.linearVelocity;
            if (direction == Vector3.zero) return;

            var position = other.transform.position;

            var worldCenter = _boxCollider.transform.TransformPoint(_boxCollider.center);
            var worldSize = Vector3.Scale(_boxCollider.size, _boxCollider.transform.lossyScale);
            var worldBounds = new Bounds(worldCenter, worldSize);
            var ray = new Ray(position, direction.normalized);

            if (worldBounds.IntersectRay(ray, out var distance))
            {
                var intersectionPoint = ray.GetPoint(distance);
                Debug.DrawLine(position, intersectionPoint, Color.red);
            }
        }
    }
}
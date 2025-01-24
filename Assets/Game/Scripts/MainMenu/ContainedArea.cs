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

            var direction = rb.linearVelocity.normalized.WithY(0);
            var position = other.transform.position;
            var back = position - direction * 100f;

            var colliders = Physics.RaycastAll(back, direction, 200);
            foreach (var hit in colliders)
            {
                if (hit.collider != _boxCollider)
                    return;
                
                rb.MovePosition(hit.point);
                return;
            }
        }
    }
}
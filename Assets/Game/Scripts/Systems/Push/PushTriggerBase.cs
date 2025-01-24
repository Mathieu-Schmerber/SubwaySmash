using System;
using UnityEngine;

namespace Game.Systems.Push
{
    public abstract class PushTriggerBase : MonoBehaviour
    {
        [SerializeField] private LayerMask _ignore;
        [SerializeField] private float _velocityThreshold;

        public event Action OnTrigger; 
        
        private static float CalculateImpactForce(Collision collision)
        {
            var relativeVelocity = collision.relativeVelocity;
            var otherMass = collision.rigidbody ? collision.rigidbody.mass : 1f;
            var impactForce = otherMass * relativeVelocity.magnitude;

            return impactForce;
        }
        
        private void OnCollisionEnter(Collision other)
        {
            Debug.Log($"{name} collide {other.gameObject.name}");
            if (((1 << other.gameObject.layer) & _ignore) != 0)
                return;
            
            var impactForce = CalculateImpactForce(other);
            if (impactForce > _velocityThreshold)
            {
                OnTrigger?.Invoke();
                Trigger(other.transform);
            }
        }

        public abstract void Trigger(Transform actor);
    }
}
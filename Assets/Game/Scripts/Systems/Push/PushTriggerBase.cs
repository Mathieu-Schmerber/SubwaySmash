using System;
using FMODUnity;
using Game.Systems.Audio;
using UnityEngine;

namespace Game.Systems.Push
{
    public abstract class PushTriggerBase : MonoBehaviour
    {
        [SerializeField] private LayerMask _canTriggerWith;
        [SerializeField] private float _velocityThreshold;

        [SerializeField] private EventReference _audio;
        
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
            // Check if the layer is part of the inclusion layer mask
            if (((1 << other.gameObject.layer) & _canTriggerWith) == 0)
                return;

            var impactForce = CalculateImpactForce(other);
            if (impactForce > _velocityThreshold)
            {
                OnTrigger?.Invoke();
                AudioManager.PlayOneShot(_audio);
                Trigger(other.transform);
            }
        }

        public abstract void Trigger(Transform actor);
    }
}
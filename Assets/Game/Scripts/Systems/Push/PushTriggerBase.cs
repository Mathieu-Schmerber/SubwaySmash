using UnityEngine;

namespace Game.Systems.Push
{
    public abstract class PushTriggerBase : MonoBehaviour
    {
        [SerializeField] private LayerMask _ignore;
        [SerializeField] private float _velocityThreshold;
        
        private float CalculateImpactForce(Collision collision)
        {
            var relativeVelocity = collision.relativeVelocity;
            var otherMass = collision.rigidbody ? collision.rigidbody.mass : 1f;
            var impactForce = otherMass * relativeVelocity.magnitude;

            return impactForce;
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (((1 << other.gameObject.layer) & _ignore) != 0)
                return;
            
            var myPushable = GetComponent<Pushable>();
            var otherPushable = other.transform.GetComponent<Pushable>();
            if (myPushable == null && otherPushable == null) return;
            var impactForce = CalculateImpactForce(other);
            
            Debug.Log($"Impact Force ({name} vs {other.transform.name}): {impactForce}");
            if (impactForce > _velocityThreshold)
                Trigger(myPushable ?? otherPushable);
        }

        public abstract void Trigger(Pushable actor);
    }
}
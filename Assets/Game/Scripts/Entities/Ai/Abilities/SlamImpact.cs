using Game.Entities.Player;
using Game.Systems.Push;
using UnityEngine;

namespace Game.Entities.Ai.Abilities
{
    public class SlamImpact : MonoBehaviour
    {
        [SerializeField] private float impactRadius = 5f;

        public void PerformImpact(float pushForce)
        {
            var hits = Physics.OverlapSphere(transform.position, impactRadius);

            foreach (var hitCollider in hits)
            {
                if (hitCollider.gameObject.layer == gameObject.layer)
                    continue;
                
                var pushTrigger = hitCollider.GetComponent<PushTriggerBase>();
                if (pushTrigger != null)
                    pushTrigger.Trigger(GetComponent<Pushable>());

                var killable = hitCollider.GetComponent<PlayerStateMachine>();
                if (killable != null)
                {
                    killable.Kill((hitCollider.transform.position - transform.position).normalized, pushForce);
                    continue;
                }

                var rb = hitCollider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce((hitCollider.transform.position - transform.position).normalized * pushForce, ForceMode.Impulse);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, impactRadius);
        }
    }
}
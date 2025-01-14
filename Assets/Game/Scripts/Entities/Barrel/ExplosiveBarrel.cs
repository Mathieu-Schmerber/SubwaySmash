using System.Collections.Generic;
using Game.Systems.Push;
using UnityEngine;

namespace Game.Entities.Barrel
{
    public class ExplosiveBarrel : PushTriggerBase
    {
        [SerializeField] private float _explosionRadius = 5f;
        [SerializeField] private float _explosionForce = 10f;
        [SerializeField] private Vector3 _explosionOffset;

        public override void Trigger(Pushable actor)
        {
            Explode();
        }

        private void Explode()
        {
            var explosionCenter = transform.position + _explosionOffset;
            var hitColliders = Physics.OverlapSphere(explosionCenter, _explosionRadius);
            var affected = new HashSet<Pushable>();

            foreach (var col in hitColliders)
            {
                var pushable = col.GetComponent<Pushable>();
                if (pushable == null || !affected.Add(pushable)) continue;
                
                var pushDirection = (pushable.transform.position - explosionCenter).normalized;
                pushable.ApplyPush(pushDirection, _explosionForce);
            }

            Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + _explosionOffset, _explosionRadius);
        }
    }
}

using System;
using System.Collections.Generic;
using Game.Systems.Push;
using LemonInc.Core.Pooling;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Game.Entities.Barrel
{
    public class ExplosiveBarrel : PushTriggerBase
    {
        [SerializeField] private float _explosionRadius = 5f;
        [SerializeField] private float _explosionForce = 10f;
        [SerializeField] private Vector3 _explosionOffset;
        [SerializeField] private Pool _explosion;
        
        private MMF_Player _feedback;

        private void Awake()
        {
            _feedback = GetComponent<MMF_Player>();
        }

        public override void Trigger(Pushable actor)
        {
            _feedback.PlayFeedbacks();
            Explode();
        }

        private void Explode()
        {
            GetComponent<Collider>().enabled = false;
            
            var explosionCenter = transform.position + _explosionOffset;
            var hitColliders = Physics.OverlapSphere(explosionCenter, _explosionRadius);

            foreach (var col in hitColliders)
            {
                var rb = col.GetComponent<Rigidbody>();
                if (rb == null) continue;
                
                Debug.Log(col.gameObject.name);
                rb.AddExplosionForce(_explosionForce, explosionCenter, _explosionRadius);
            }

            Core.Pooling.From(_explosion).Get(null, explosionCenter, Quaternion.identity);
            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + _explosionOffset, _explosionRadius);        
        }
    }
}

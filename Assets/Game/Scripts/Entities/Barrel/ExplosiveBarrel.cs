using System;
using Game.Systems.Push;
using LemonInc.Core.Pooling;
using LemonInc.Core.Utilities.Extensions;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Game.Entities.Barrel
{
    public class ExplosiveBarrel : PushTriggerBase, IKillable
    {
        [SerializeField] private float _explosionRadius = 5f;
        [SerializeField] private float _explosionForce = 10f;
        [SerializeField] private Vector3 _explosionOffset;
        [SerializeField] private Pool _explosion;
        
        private MMF_Player _feedback;
        private Pushable _pushable;

        private bool _hasExploded = false;

        private void Awake()
        {
            _feedback = GetComponent<MMF_Player>();
            _pushable = GetComponent<Pushable>();
        }

        public override void Trigger(Pushable actor)
        {
            Explode();
        }

        private void Explode()
        {
            if (!transform || _hasExploded)
                return;

            OnDeath?.Invoke();
            _hasExploded = true;
            var explosionCenter = transform.position + _explosionOffset;
            var hitColliders = Physics.OverlapSphere(explosionCenter, _explosionRadius);

            foreach (var col in hitColliders)
            {
                if (col.transform == transform)
                    continue;
                
                col.GetComponent<IKillable>()?.Kill((col.transform.position - transform.position).normalized, 30);
                
                var pushable = col.GetComponent<Pushable>();
                if (pushable != null && pushable != _pushable)
                {
                    var dir = (pushable.transform.position - explosionCenter).normalized;
                    pushable.ApplyPush(dir, 0); // Pushed by explosion force, but trigger anyway
                }

                var pushTrigger = col.GetComponent<PushTriggerBase>();
                if (pushTrigger != null && pushTrigger != this)
                    pushTrigger.Trigger(_pushable);
                
                var rb = col.GetComponent<Rigidbody>();
                if (rb == null) continue;
                
                rb.AddExplosionForce(_explosionForce, explosionCenter.WithY(col.transform.position.y), _explosionRadius);
            }

            _feedback.PlayFeedbacks();
            Core.Pooling.From(_explosion).Get(null, explosionCenter, Quaternion.identity);
            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + _explosionOffset, _explosionRadius);        
        }

        public bool IsDead => _hasExploded;
        public event Action OnDeath;

        public void Kill(Vector3 direction, float force)
        {
            Explode();
        }
    }
}

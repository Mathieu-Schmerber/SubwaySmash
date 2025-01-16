using System;
using Game.Systems.Push;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Entities.GPE.BBQ
{
    [Serializable]
    public struct FireSettings
    {
        public float TimeToKill;
        public float IgniteTime;
        public float SpreadRadius;
        public bool CapSpreadChain;
        [ShowIf(nameof(CapSpreadChain))] public int MaxSpreadChain;
    }
    
    public class Barbecue : PushTriggerBase
    {
        [Header("BBQ")]
        [SerializeField] private float _initialSpreadRadius;
        [SerializeField] private Vector3 _offset;
        
        [Header("Fire spread")]
        [SerializeField] private FireSettings _fireSettings;

        private MMF_Player _activateFeedback;
        private bool _isSpreading = false;

        public override void Trigger(Pushable actor)
        {
            if (_isSpreading)
                return;
            StartSpreadingFlames();
        }

        private void Awake()
        {
            _activateFeedback = GetComponent<MMF_Player>();
        }

        private void StartSpreadingFlames()
        {
            _isSpreading = true;
            _activateFeedback.PlayFeedbacks();
        }

        private void FixedUpdate()
        {
            if (!_isSpreading)
                return;

            OnFire.IgniteSurroundings(transform.position + _offset, _initialSpreadRadius, _fireSettings);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + _offset, _initialSpreadRadius);
        }
    }
}
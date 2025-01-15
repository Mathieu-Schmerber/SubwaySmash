using System;
using System.Collections;
using Databases;
using Game.Systems.Push;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Game.Entities.Player.Abilities
{
    public class PushAbility : AbilityBase
    {
        private static readonly int Attack = Animator.StringToHash("Attack");
        
        [SerializeField] private float _sphereCheckRadius;
        [SerializeField] private Vector3 _sphereCheckOffset;
        
        [SerializeField] private MMF_Player _feedback;
        
        private IInputProvider _input;
        private Animator _animator;

        private void Awake()
        {
            _input = GetComponent<IInputProvider>();
            _animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            SetCooldown(RuntimeDatabase.Data.PlayerData.PushCooldown);
        }

        protected override IEnumerator OnPerform(Action performed)
        {
            _animator.SetTrigger(Attack);
            Push();
            yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
            performed?.Invoke();
        }

        private void Push()
        {
            var hit = false;
            var aim = _input.AimDirection;
            var front = transform.position + aim * _sphereCheckRadius + _sphereCheckOffset;
            var force = RuntimeDatabase.Data.PlayerData.PushForce;
            
            // Find all colliders within the sphere
            var hitColliders = Physics.OverlapSphere(front, _sphereCheckRadius);

            foreach (var coll in hitColliders)
            {
                var pushable = coll.GetComponent<Pushable>();
                if (pushable == null) continue;

                pushable.ApplyPush(aim, force);
                hit = true;
            }
            
            if (hit)
                _feedback.PlayFeedbacks();
        }

        private void OnDrawGizmosSelected()
        {
            var aim = Application.isPlaying ? _input.AimDirection : transform.forward;
            var front = transform.position + aim * _sphereCheckRadius + _sphereCheckOffset;
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(front, _sphereCheckRadius);
        }
    }
}
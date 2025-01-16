using System;
using System.Collections;
using System.Collections.Generic;
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
        private bool _alreadyPlayedFeedback;
        private HashSet<Pushable> _alreadyPushed;
        private bool _attackCheck;

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
            _alreadyPushed = new HashSet<Pushable>();
            _alreadyPlayedFeedback = false;
            _attackCheck = true;
            
            var animationLength = _animator.GetCurrentAnimatorStateInfo(0).length;
            var elapsedTime = 0f;
            var interval = 0.1f;

            while (elapsedTime < animationLength)
            {
                if (_attackCheck)
                    Push();
                elapsedTime += interval;
                yield return new WaitForSeconds(interval);
            }
            
            _alreadyPushed.Clear();
            performed?.Invoke();
        }

        private void Push()
        {
            var aim = _input.AimDirection;
            var front = transform.position + aim * _sphereCheckRadius + _sphereCheckOffset;
            var force = RuntimeDatabase.Data.PlayerData.PushForce;

            // Find all colliders within the sphere
            var hitColliders = Physics.OverlapSphere(front, _sphereCheckRadius);

            foreach (var coll in hitColliders)
            {
                var pushable = coll.GetComponent<Pushable>();
                if (pushable == null || _alreadyPushed.Contains(pushable)) continue;

                pushable.ApplyPush(aim, force);
                _alreadyPushed.Add(pushable);
            }

            if (_alreadyPushed.Count > 0 && !_alreadyPlayedFeedback)
            {
                _alreadyPlayedFeedback = true;
                _feedback.PlayFeedbacks();
            }
        }

        public void OnAnimationEventReceived(string eventName)
        {
            if (eventName == "StopAttack")
            {
                _attackCheck = false;
            }
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
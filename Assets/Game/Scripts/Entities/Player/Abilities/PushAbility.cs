using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Databases;
using FMODUnity;
using Game.Entities.Ai.Abilities;
using Game.Systems.Audio;
using Game.Systems.Inputs;
using Game.Systems.Push;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Game.Entities.Player.Abilities
{
    public class PushAbility : AbilityBase, IAnimationEventListener
    {
        private static readonly int Attack = Animator.StringToHash("Attack");

        [SerializeField] private EventReference _audio;
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
        
        AnimationClip GetAnimationClipByStateName(Animator animator, string stateName)
        {
            var controller = animator.runtimeAnimatorController;
            return controller.animationClips.FirstOrDefault(x => x.name.Equals(stateName));
        }

        protected override IEnumerator OnPerform(Action performed)
        {
            AudioManager.PlayOneShot(_audio);
            _animator.SetTrigger(Attack);
            _alreadyPushed = new HashSet<Pushable>();
            _alreadyPlayedFeedback = false;
            _attackCheck = true;
            
            var animationLength = GetAnimationClipByStateName(_animator, "Attack").length;
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
                if (pushable.enabled)
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
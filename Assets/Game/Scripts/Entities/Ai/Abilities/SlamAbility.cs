using System;
using System.Collections;
using System.Linq;
using Game.Entities.Player;
using Game.Entities.Player.Abilities;
using Game.Inputs;
using Game.Systems.Push;
using MoreMountains.Feedbacks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Entities.Ai.Abilities
{
    public class SlamAbility : AbilityBase
    {
        private const string ATTACK_NAME = "PoliceAttack";
        private static readonly int Attack = Animator.StringToHash("Attack");

        [SerializeField] private MMF_Player _feedback;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _attackRadius;
        [SerializeField] private float _killForce = 20;
        [SerializeField] private float _pushForce = 40;
        [SerializeField] private float _recoverTime;
        
        private Animator _animator;
        private Controller _controller;
        private IInputProvider _input;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _controller = GetComponent<Controller>();
            _input = GetComponent<IInputProvider>();
        }

        protected override IEnumerator OnPerform(Action performed)
        {
            var clip = GetAnimationClipByStateName(_animator, ATTACK_NAME);
            
            _animator.SetTrigger(Attack);
            yield return new WaitForSeconds(_recoverTime);
            performed?.Invoke();
        }

        public void OnAnimationEventReceived(string eventName)
        {
            if (eventName == "Slam")
            {
                Slam();
            }
        }

        AnimationClip GetAnimationClipByStateName(Animator animator, string stateName)
        {
            var controller = animator.runtimeAnimatorController;
            return controller.animationClips.FirstOrDefault(x => x.name.Equals(stateName));
        }

        private void Slam()
        {
            _controller.SetSpeed(0);
            _feedback.PlayFeedbacks();

            var origin = transform.position + _input.AimDirection * _attackRadius + _offset;
            var hitColliders = Physics.OverlapSphere(origin, _attackRadius);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.transform == transform)
                    continue;

                bool isRegularRb = true;
                
                // Check if the collider has a PushTriggerBase component
                var pushTrigger = hitCollider.GetComponent<PushTriggerBase>();
                if (pushTrigger != null)
                    pushTrigger.Trigger(GetComponent<Pushable>());

                var killable = hitCollider.GetComponent<PlayerStateMachine>();
                if (killable != null)
                    killable.Kill((hitCollider.transform.position - origin).normalized, _killForce);
                
                var rb = hitCollider.GetComponent<Rigidbody>();
                if (!killable && rb)
                    rb.AddForce((hitCollider.transform.position - origin).normalized * _pushForce, ForceMode.Impulse);
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            var dir = Application.isPlaying ? _input.AimDirection : transform.forward;
            Gizmos.color = Color.red;
            var origin = transform.position + dir * _attackRadius + _offset;
            Gizmos.DrawWireSphere(origin, _attackRadius);
        }
    }
}
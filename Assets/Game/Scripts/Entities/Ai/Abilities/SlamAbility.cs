using System;
using System.Collections;
using System.Linq;
using Game.Entities.Player;
using Game.Entities.Player.Abilities;
using Game.Inputs;
using Game.Systems.Push;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Game.Entities.Ai.Abilities
{
    public class SlamAbility : AbilityBase
    {
        private const string ATTACK_NAME = "PoliceAttack";
        private static readonly int Attack = Animator.StringToHash("Attack");

        [SerializeField] private MMF_Player _feedback;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _radius;
        
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
            yield return new WaitForSeconds(clip.length);
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

            var origin = transform.position + _input.AimDirection * _radius + _offset;
            var hitColliders = Physics.OverlapSphere(origin, _radius);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.transform == transform)
                    continue;
                
                // Check if the collider has a PushTriggerBase component
                var pushTrigger = hitCollider.GetComponent<PushTriggerBase>();
                if (pushTrigger != null)
                    pushTrigger.Trigger(GetComponent<Pushable>());

                var killable = hitCollider.GetComponent<PlayerStateMachine>();
                if (killable != null)
                    killable.Kill(Vector3.up, 20);
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            var dir = Application.isPlaying ? _input.AimDirection : transform.forward;
            Gizmos.color = Color.red;
            var origin = transform.position + dir * _radius + _offset;
            Gizmos.DrawWireSphere(origin, _radius);
        }
    }
}
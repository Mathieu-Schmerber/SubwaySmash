using System;
using System.Collections;
using Game.Entities.Player;
using Game.Entities.Player.Abilities;
using Game.Systems.Push;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Game.Entities.Ai.Abilities
{
    public class SlamAbility : AbilityBase
    {
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
            _animator.SetTrigger(Attack);
            yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
            performed?.Invoke();
        }

        public void OnAnimationEventReceived(string eventName)
        {
            if (eventName == "Slam")
            {
                Slam();
            }
        }

        private void Slam()
        {
            _controller.SetSpeed(0);
            _feedback.PlayFeedbacks();

            var origin = transform.position + _input.AimDirection * _radius + _offset;
            var hitColliders = Physics.OverlapSphere(origin, _radius);

            foreach (var hitCollider in hitColliders)
            {
                // Check if the collider has a PushTriggerBase component
                var pushTrigger = hitCollider.GetComponent<PushTriggerBase>();
                if (pushTrigger != null)
                    pushTrigger.Trigger(GetComponent<Pushable>());
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
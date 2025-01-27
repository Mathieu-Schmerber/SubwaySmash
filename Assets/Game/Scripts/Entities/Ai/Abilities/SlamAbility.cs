using System;
using System.Collections;
using Game.Entities.Player.Abilities;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Game.Entities.Ai.Abilities
{
    public class SlamAbility : AbilityBase
    {
        private static readonly int Attack = Animator.StringToHash("Attack");

        [SerializeField] private MMF_Player _feedback;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _pushForce = 20;
        [SerializeField] private float _recoverTime;
        
        private SlamImpact _slamImpact;
        private Animator _animator;
        private Controller _controller;

        private void Awake()
        {
            _slamImpact = GetComponentInChildren<SlamImpact>();
            _animator = GetComponentInChildren<Animator>();
            _controller = GetComponent<Controller>();
        }

        protected override IEnumerator OnPerform(Action performed)
        {
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

        private void Slam()
        {
            _controller.SetSpeed(0);
            _slamImpact.PerformImpact(_pushForce);
            _feedback.PlayFeedbacks();
        }
    }
}
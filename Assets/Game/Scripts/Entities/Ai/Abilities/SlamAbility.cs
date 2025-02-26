using System;
using System.Collections;
using FMODUnity;
using Game.Entities.Player.Abilities;
using Game.Entities.Shared;
using Game.Systems.Audio;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Game.Entities.Ai.Abilities
{
    public interface IAnimationEventListener
    {
        void OnAnimationEventReceived(string eventName);
    }

    public class SlamAbility : AbilityBase, IAnimationEventListener
    {
        private static readonly int Attack = Animator.StringToHash("Attack");

        [SerializeField] private EventReference _leapAudio;
        [SerializeField] private EventReference _slamAudio;
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
            AudioManager.PlayOneShot(_leapAudio);
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
            AudioManager.PlayOneShot(_slamAudio); 
            _controller.SetSpeed(0);
            _slamImpact.PerformImpact(_pushForce);
            _feedback.PlayFeedbacks();
        }
    }
}
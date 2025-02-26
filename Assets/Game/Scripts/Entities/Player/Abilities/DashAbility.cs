using System;
using System.Collections;
using Databases;
using Game.Entities.Shared;
using Game.Systems.Inputs;
using UnityEngine;

namespace Game.Entities.Player.Abilities
{
    public class DashAbility : AbilityBase
    {
        private static readonly int Dash = Animator.StringToHash("Dash");
        
        private Controller _controller;
        private IInputProvider _input;
         private Animator _animator;

        public bool IsDashing { get; private set; }

        private void Awake()
        {
            _controller = GetComponent<Controller>();
            _input = GetComponent<IInputProvider>();
            _animator = GetComponentInChildren<Animator>();
        }
        
        protected override IEnumerator OnPerform(Action performed)
        {
            var distance = RuntimeDatabase.Data.PlayerData.DashDistance;
            var time = RuntimeDatabase.Data.PlayerData.DashDuration;
            var speed = _controller.CalculateTargetSpeed(distance, time);
            
            _animator.SetTrigger(Dash);
            
            _controller.SetSpeed(speed);
            _controller.SetDirection(_input.MovementDirection, true);
            _controller.LockAim(true, _input.AimDirection);
            IsDashing = true;
            
            yield return new WaitForSeconds(time);
            
            IsDashing = false;
            _controller.LockAim(false);
            performed?.Invoke();
        }
    }
}
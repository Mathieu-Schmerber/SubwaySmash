using System;
using System.Collections;
using Databases;
using JetBrains.Annotations;
using UnityEngine;

namespace Game.Entities.Player.Abilities
{
    public class DashAbility : AbilityBase
    {
        private Controller _controller;
        private IInputProvider _input;
        
        public bool IsDashing { get; private set; }

        private void Awake()
        {
            _controller = GetComponent<Controller>();
            _input = GetComponent<IInputProvider>();
        }
        
        protected override IEnumerator OnPerform(Action performed)
        {
            var distance = RuntimeDatabase.Data.PlayerData.DashDistance;
            var time = RuntimeDatabase.Data.PlayerData.DashDuration;
            var speed = _controller.CalculateTargetSpeed(distance, time);
            
            _controller.SetSpeed(speed);
            _controller.SetDirection(_input.MovementDirection);
            _controller.LockAim(true, _input.AimDirection);
            IsDashing = true;
            
            yield return new WaitForSeconds(time);
            
            IsDashing = false;
            _controller.LockAim(false);
            performed?.Invoke();
        }
    }
}
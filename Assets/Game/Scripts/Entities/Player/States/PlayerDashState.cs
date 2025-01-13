using Databases;
using Game.Systems.StateMachine;
using LemonInc.Core.Utilities;
using UnityEngine;

namespace Game.Entities.Player.States
{
    public class PlayerDashState : State<PlayerStates>
    {
        private IInputProvider _input;
        private Controller _controller;
        private Animator _animator;

        private static readonly int Dash = Animator.StringToHash("Dash");

        public override void Awake()
        {
            _animator = StateMachine.Owner.GetComponentInChildren<Animator>();
            _input = StateMachine.Owner.GetComponent<IInputProvider>();
            _controller = StateMachine.Owner.GetComponent<Controller>();
        }

        public override void Enter()
        {
            var distance = RuntimeDatabase.Data.PlayerData.DashDistance;
            var time = RuntimeDatabase.Data.PlayerData.DashDuration;
            var speed = _controller.CalculateTargetSpeed(distance, time);
            
            _controller.SetSpeed(speed);
            _controller.SetDirection(_input.AimDirection);
            _controller.LockAim(true, _input.AimDirection);
            _animator.SetTrigger(Dash);
            Awaiter.WaitAndExecute(time, () => StateMachine.SwitchState(Payload.Idle));
        }
        
        public override void Exit()
        {
            _controller.LockAim(false);
            _controller.RestartMoveAbilityCooldown();
        }
    }
}
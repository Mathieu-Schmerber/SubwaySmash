using Databases;
using Game.Systems.StateMachine;
using LemonInc.Core.Utilities;

namespace Game.Entities.Player.States
{
    public class PlayerDashState : State<PlayerStates>
    {
        private IInputProvider _input;
        private Controller _controller;
        
        public override void Awake()
        {
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
            Awaiter.WaitAndExecute(.2f, () => StateMachine.SwitchState(Payload.Idle));
            
        }
        
        public override void Exit()
        {
            _controller.LockAim(false);
            _controller.RestartMoveAbilityCooldown();
        }
    }
}
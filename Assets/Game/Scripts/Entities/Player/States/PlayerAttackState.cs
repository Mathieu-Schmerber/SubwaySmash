using Databases;
using Game.Entities.Player.Abilities;
using Game.Entities.Shared;
using Game.Systems.Inputs;
using Game.Systems.StateMachine;

namespace Game.Entities.Player.States
{
    public class PlayerAttackState : State<PlayerStates>
    {
        private PushAbility _push;
        private Controller _controller;
        private IInputProvider _input;

        public override void Awake()
        {
            _push = StateMachine.Owner.GetComponent<PushAbility>();
            _controller = StateMachine.Owner.GetComponent<Controller>();
            _input = StateMachine.Owner.GetComponent<IInputProvider>();
        }

        public override void Enter()
        {
            _controller.SetSpeed(RuntimeDatabase.Data.PlayerData.MovementSpeed);
            _controller.LockAim(true, _input.AimDirection);
            _push.Perform(() =>
            {
                StateMachine.SwitchState(Payload.Run);
            });
        }

        public override void Exit()
        {
            _controller.LockAim(false);
        }
        
        public override void Update()
        {
            _controller.SetDirection(_input.MovementDirection);
        }
    }
}
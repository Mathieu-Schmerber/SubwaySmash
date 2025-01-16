using Game.Entities.Ai.Abilities;
using Game.Entities.Player;
using Game.Systems.StateMachine;

namespace Game.Entities.Ai.States
{
    public class AttackState : State<AiStates>
    {
        private SlamAbility _slam;
        private Controller _controller;
        private IInputProvider _input;

        public override void Awake()
        {
            _input = StateMachine.Owner.GetComponent<IInputProvider>();
            _slam = StateMachine.Owner.GetComponent<SlamAbility>();
            _controller = StateMachine.Owner.GetComponent<Controller>();
        }

        public override void Enter()
        {
            _controller.LockAim(true, _input.AimDirection);
            _slam.Perform(() => StateMachine.SwitchState(Payload.ChaseState));
        }

        public override void Exit()
        {
            _controller.LockAim(false);
        }
    }
}
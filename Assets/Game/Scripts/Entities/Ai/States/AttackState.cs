using Game.Entities.Ai.Abilities;
using Game.Entities.Player;
using Game.Inputs;
using Game.Systems.Push;
using Game.Systems.StateMachine;

namespace Game.Entities.Ai.States
{
    public class AttackState : State<AiStates>
    {
        private SlamAbility _slam;
        private Controller _controller;
        private AiBrain _brain;
        private Pushable _pushable;

        public override void Awake()
        {
            _brain = StateMachine.Owner.GetComponent<AiBrain>();
            _slam = StateMachine.Owner.GetComponent<SlamAbility>();
            _controller = StateMachine.Owner.GetComponent<Controller>();
            _pushable = StateMachine.Owner.GetComponent<Pushable>();
        }

        public override void Enter()
        {
            var leapDir = (_brain.Target.position - _brain.transform.position).normalized;
            _pushable.enabled = false;
            _controller.LockAim(true, leapDir);
            _controller.SetDirection(leapDir, true);
            _slam.Perform(() => StateMachine.SwitchState(Payload.ChaseState));
        }

        public override void Exit()
        {
            _pushable.enabled = true;
            _controller.LockAim(false);
        }
    }
}
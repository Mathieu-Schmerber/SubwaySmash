using Game.Systems.StateMachine;

namespace Game.Entities.Player.States
{
    public class PlayerIdleState : State<PlayerStates>
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
            _controller.SetSpeed(0);
        }

        public override void Update()
        {
            if (_input.MovementDirection.magnitude > 0)
                StateMachine.SwitchState(Payload.Run);
        }
    }
}
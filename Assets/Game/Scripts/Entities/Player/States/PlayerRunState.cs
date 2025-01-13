using Databases;
using Game.Systems.StateMachine;

namespace Game.Entities.Player.States
{
    public class PlayerRunState : State<PlayerStates>
    {
        private IInputProvider _input;
        private Controller _controller;
        
        public override void Awake()
        {
            base.Awake();
            _input = StateMachine.Owner.GetComponent<IInputProvider>();
            _controller = StateMachine.Owner.GetComponent<Controller>();
        }

        public override void Enter()
        {
            _controller.SetSpeed(RuntimeDatabase.Data.PlayerData.MovementSpeed);
        }

        public override void Update()
        {
            _controller.SetDirection(_input.MovementDirection);

            if (_input.MovementDirection.magnitude == 0)
                StateMachine.SwitchState(Payload.Idle);
        }
    }
}
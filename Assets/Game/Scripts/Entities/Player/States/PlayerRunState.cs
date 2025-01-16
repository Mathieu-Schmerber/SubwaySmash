using Databases;
using Game.Inputs;
using Game.Systems.StateMachine;
using UnityEngine;

namespace Game.Entities.Player.States
{
    public class PlayerRunState : State<PlayerStates>
    {
        private IInputProvider _input;
        private Controller _controller;
        private Animator _animator;
        
        private static readonly int Speed = Animator.StringToHash("Speed");
        
        public override void Awake()
        {
            _animator = StateMachine.Owner.GetComponentInChildren<Animator>();
            _input = StateMachine.Owner.GetComponent<IInputProvider>();
            _controller = StateMachine.Owner.GetComponent<Controller>();
        }

        public override void Enter()
        {
            _animator.SetFloat(Speed, _input.MovementDirection.magnitude);
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
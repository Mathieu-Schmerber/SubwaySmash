using Game.Entities.Ai.Abilities;
using Game.Entities.Player;
using Game.Entities.Shared;
using Game.Systems.Inputs;
using Game.Systems.StateMachine;
using UnityEngine;

namespace Game.Entities.Ai.States
{
    public class ChaseState : State<AiStates>
    {
        private static readonly int Speed = Animator.StringToHash("Speed");
        private Animator _animator;
        private PlayerStateMachine _player;
        private IInputProvider _input;
        private AiBrain _aiBrain;
        private Controller _controller;
        private SlamAbility _slam;

        public override void Awake()
        {
            _animator = StateMachine.Owner.GetComponentInChildren<Animator>();
            _player = Object.FindFirstObjectByType<PlayerStateMachine>();
            _aiBrain = StateMachine.Owner.GetComponent<AiBrain>();
            _input = StateMachine.Owner.GetComponent<IInputProvider>();
            _controller = StateMachine.Owner.GetComponent<Controller>();
            _slam = StateMachine.Owner.GetComponent<SlamAbility>();
        }

        public override void Enter()
        {
            if (!Payload.IsAggressive)
            {
                StateMachine.SwitchState(Payload.PatrolState);
                return;
            }
            
            _controller.SetSpeed(Payload.StatData.RunSpeed);
            _animator.SetFloat(Speed, 1);
            if (_player)
                _aiBrain.SetTarget(_player.transform);
            else
                StateMachine.SwitchState(Payload.PatrolState);
        }

        public override void Exit()
        {
            _animator.SetFloat(Speed, 0);
        }

        public override void Update()
        {
            if (!_player)
            {
                StateMachine.SwitchState(Payload.PatrolState);
                return;
            }
            
            _controller.SetDirection(_input.MovementDirection);
            _controller.SetSpeed(Payload.StatData.RunSpeed);

            if (_slam && _slam.IsReady() && _aiBrain.IsNavClear(_player.transform.position) &&
                Vector3.Distance(_player.transform.position, StateMachine.Owner.position) <= Payload.StatData.AttackTriggerDistance)
            {
                StateMachine.SwitchState(Payload.AttackState);
            }
        }
    }
}
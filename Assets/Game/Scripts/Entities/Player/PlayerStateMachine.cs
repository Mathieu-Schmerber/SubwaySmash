using System;
using Databases;
using Game.Entities.Player.Abilities;
using Game.Entities.Player.States;
using Game.Inputs;
using Game.Systems.StateMachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Entities.Player
{
    public class PlayerStateMachine : KillableBase
    {
        private PlayerStates _payload;
        private readonly StateMachine<PlayerStates> _stateMachine = new();
        private IInputProvider _input;
        private PushAbility _push;
        private Controller _controller;

        public static event Action<Transform> OnPlayerDeath;

        [ShowInInspector]
        public string CurrentStateName => _stateMachine.CurrentState?.GetType().Name ?? "None";
        
        private void Awake()
        {
            _controller = GetComponent<Controller>();
            _input = GetComponent<IInputProvider>();
            
            _push = GetComponent<PushAbility>();
            _push.SetCooldown(RuntimeDatabase.Data.PlayerData.PushCooldown);
            
            _stateMachine.SetOwnership(transform);
            _payload = new PlayerStates
            {
                Idle = PlayerIdleState.Init<PlayerIdleState>(_stateMachine),
                Run = PlayerRunState.Init<PlayerRunState>(_stateMachine),
                Dash = PlayerDashState.Init<PlayerDashState>(_stateMachine),
                Attack = PlayerAttackState.Init<PlayerAttackState>(_stateMachine),
                Dead = PlayerDeadState.Init<PlayerDeadState>(_stateMachine),
            };
            _stateMachine.SetPayload(_payload);
        }

        private void OnEnable()
        {
            _input.Push.OnPressed += OnPushPressed;
        }

        private void OnDisable()
        {
            _input.Push.OnPressed -= OnPushPressed;
        }
		
        private void OnPushPressed()
        {
            if (_push.IsReady())
                _stateMachine.SwitchState(_payload.Attack);
        }
        
        private void Start()
        {
            _stateMachine.SwitchState(_payload.Idle);
            _controller.SetMaxSpeed(RuntimeDatabase.Data.PlayerData.MovementSpeed);
        }

        private void Update()
        {
            _stateMachine.Update();
        }

        protected override void OnKill(Vector3 direction, float force)
        {
            var payload = _stateMachine.Payload;
            payload.DeathForce = force;
            payload.DeathDirection = direction;
            _stateMachine.SetPayload(payload);
            _stateMachine.LockState(_stateMachine.Payload.Dead);
            OnPlayerDeath?.Invoke(transform);
        }
    }
}
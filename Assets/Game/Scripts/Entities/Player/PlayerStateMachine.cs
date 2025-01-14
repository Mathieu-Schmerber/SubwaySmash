using System;
using Databases;
using Game.Entities.Player.Abilities;
using Game.Entities.Player.States;
using Game.Systems.StateMachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Entities.Player
{
    public class PlayerStateMachine : MonoBehaviour
    {
        private PlayerStates _payload;
        private readonly StateMachine<PlayerStates> _stateMachine = new();
        private IInputProvider _input;
        private DashAbility _dash;

        [ShowInInspector]
        public string CurrentStateName => _stateMachine.CurrentState?.GetType().Name ?? "None";
        
        private void Awake()
        {
            _input = GetComponent<IInputProvider>();
            _dash = GetComponent<DashAbility>();
            _dash.SetCooldown(RuntimeDatabase.Data.PlayerData.DashCooldown);
            
            _stateMachine.SetOwnership(transform);
            _payload = new PlayerStates
            {
                Idle = PlayerIdleState.Init<PlayerIdleState>(_stateMachine),
                Run = PlayerRunState.Init<PlayerRunState>(_stateMachine),
                Dash = PlayerDashState.Init<PlayerDashState>(_stateMachine),
            };
            _stateMachine.SetPayload(_payload);
        }

        private void OnEnable()
        {
            _input.Dash.OnPressed += OnDashPressed;
        }

        private void OnDisable()
        {
            _input.Dash.OnPressed -= OnDashPressed;
        }
		
        private void OnDashPressed()
        {
            if (_dash.IsReady())
                _stateMachine.SwitchState(_payload.Dash);
        }
        
        private void Start()
        {
            _stateMachine.SwitchState(_payload.Idle);
        }

        private void Update()
        {
            _stateMachine.Update();
        }
    }
}
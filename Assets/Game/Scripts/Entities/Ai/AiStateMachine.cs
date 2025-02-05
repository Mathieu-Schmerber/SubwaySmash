using Game.Entities.Ai.Abilities;
using Game.Entities.Ai.States;
using Game.Systems.StateMachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Entities.Ai
{
    public class AiStateMachine : KillableBase
    {
        [SerializeField] private AiStatData _stat;
        [SerializeField] private bool _isAggressive;
        
        private readonly StateMachine<AiStates> _stateMachine = new();
        private SlamAbility _slam;
        private Controller _controller;
        
        [ShowInInspector]
        public string CurrentStateName => _stateMachine.CurrentState?.GetType().Name ?? "None";

        private void Awake()
        {
            _controller = GetComponent<Controller>();
            _slam = GetComponent<SlamAbility>();
            _stateMachine.SetOwnership(transform);
            _stateMachine.SetPayload(new AiStates
            {
                PatrolState = PatrolState.Init<PatrolState>(_stateMachine),
                ChaseState = ChaseState.Init<ChaseState>(_stateMachine),
                StunState = StunState.Init<StunState>(_stateMachine),
                DeadState = DeadState.Init<DeadState>(_stateMachine),
                AttackState = AttackState.Init<AttackState>(_stateMachine),
                EscapeState = EscapeState.Init<EscapeState>(_stateMachine),
                StatData = _stat,
                IsAggressive = _isAggressive
            });
        }

        private void Start()
        {
            _stateMachine.SwitchState(_stateMachine.Payload.PatrolState);
            _slam?.SetCooldown(_stat.AttackCooldown);
            _controller.SetMaxSpeed(_stateMachine.Payload.StatData.RunSpeed);
        }

        private void Update()
        {
            _stateMachine.Update();
        }

        public void Stun(float time)
        {
            if (IsDead)
                return;
            
            var payload = _stateMachine.Payload;
            payload.StunTime = time;
            _stateMachine.SetPayload(payload);
            _stateMachine.SwitchState(_stateMachine.Payload.StunState);
        }
        
        protected override void OnKill(Vector3 direction, float force)
        {
            var payload = _stateMachine.Payload;
            payload.DeathForce = force;
            payload.DeathDirection = direction;
            _stateMachine.SetPayload(payload);
            _stateMachine.LockState(_stateMachine.Payload.DeadState);
            Core.AlertSystem.RaiseAlert();
        }
    }
}
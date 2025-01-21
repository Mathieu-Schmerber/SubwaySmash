using Game.Entities.Ai.Abilities;
using Game.Entities.Ai.States;
using Game.Systems.StateMachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Entities.Ai
{
    public class AiStateMachine : MonoBehaviour, IKillable
    {
        [SerializeField] private AiStatData _stat;
        [SerializeField] private bool _isAggressive;
        
        private StateMachine<AiStates> _stateMachine;
        private SlamAbility _slam;
        private Controller _controller;

        public bool IsDead => _stateMachine.CurrentState?.GetType() == typeof(DeadState);

        private void Awake()
        {
            _controller = GetComponent<Controller>();
            _slam = GetComponent<SlamAbility>();
            _stateMachine = new StateMachine<AiStates>();
            _stateMachine.SetOwnership(transform);
            _stateMachine.SetPayload(new AiStates
            {
                PatrolState = PatrolState.Init<PatrolState>(_stateMachine),
                ChaseState = _isAggressive ? ChaseState.Init<ChaseState>(_stateMachine) : null,
                StunState = StunState.Init<StunState>(_stateMachine),
                DeadState = DeadState.Init<DeadState>(_stateMachine),
                AttackState = AttackState.Init<AttackState>(_stateMachine),
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
        
        public void Kill(Vector3 direction, float force)
        {
            if (IsDead)
                return;
            
            var payload = _stateMachine.Payload;
            payload.DeathForce = force;
            payload.DeathDirection = direction;
            _stateMachine.SetPayload(payload);
            _stateMachine.LockState(_stateMachine.Payload.DeadState);
        }
    }
}
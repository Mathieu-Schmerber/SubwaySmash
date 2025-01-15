using Game.Systems.StateMachine;
using UnityEngine;

namespace Game.Entities.Ai
{
    public class AiStateMachine : MonoBehaviour, IKillable
    {
        [SerializeField] private AiStatData _stat;
   
        private StateMachine<AiStates> _stateMachine;

        public bool IsDead => _stateMachine.CurrentState?.GetType() == typeof(DeadState);

        private void Awake()
        {
            _stateMachine = new StateMachine<AiStates>();
            _stateMachine.SetOwnership(transform);
            _stateMachine.SetPayload(new AiStates
            {
                IdleState = IdleState.Init<IdleState>(_stateMachine),
                ChaseState = ChaseState.Init<ChaseState>(_stateMachine),
                StunState = StunState.Init<StunState>(_stateMachine),
                DeadState = DeadState.Init<DeadState>(_stateMachine),
                StatData = _stat
            });
        }

        private void Start()
        {
            _stateMachine.SwitchState(_stateMachine.Payload.IdleState);
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
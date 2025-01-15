using Game.Systems.Push;
using UnityEngine;

namespace Game.Entities.Ai
{
    public class PushableNpc : Pushable
    {
        [SerializeField] private bool _killOnPush = false;
        
        private AiStateMachine _aiStateMachine;
        private IKillable _killable;

        protected override void Awake()
        {
            base.Awake();
            _aiStateMachine = GetComponent<AiStateMachine>();
            _killable = GetComponent<IKillable>();
        }

        public override void ApplyPush(Vector3 direction, float force)
        {
            _aiStateMachine.Stun(1f);
            base.ApplyPush(direction, force);
            
            if (_killOnPush)
                _killable.Kill(direction, force);
        }
    }
}
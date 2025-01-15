using Game.Systems.StateMachine;
using LemonInc.Core.Utilities;
using UnityEngine;

namespace Game.Entities.Ai
{
    public class IdleState : State<AiStates>
    {
        private Timer _idleTimer;
        
        public override void Awake()
        {
            _idleTimer = new Timer();
        }

        public override void Enter()
        {
            var cooldown = Random.Range(Payload.StatData.IdleTimeRange.x, Payload.StatData.IdleTimeRange.y);
            _idleTimer.Start(cooldown, false, () => StateMachine.SwitchState(Payload.ChaseState));
        }
    }
}
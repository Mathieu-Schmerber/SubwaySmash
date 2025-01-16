using Game.Systems.StateMachine;
using LemonInc.Core.Utilities;
using UnityEngine;

namespace Game.Entities.Ai.States
{
    public class IdleState : State<AiStates>
    {
        private Timer _idleTimer;
        private Controller _controller;
        
        public override void Awake()
        {
            _idleTimer = new Timer();
            _controller = StateMachine.Owner.GetComponent<Controller>();
        }

        public override void Enter()
        {
            _controller.SetSpeed(0);
            
            var cooldown = Random.Range(Payload.StatData.IdleTimeRange.x, Payload.StatData.IdleTimeRange.y);
            _idleTimer.Start(cooldown, false, () => StateMachine.SwitchState(Payload.ChaseState));
        }
    }
}
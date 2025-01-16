using Game.Entities.Player;
using Game.Systems.StateMachine;
using LemonInc.Core.Utilities;
using UnityEngine;

namespace Game.Entities.Ai.States
{
    public class IdleState : State<AiStates>
    {
        private Timer _idleTimer;
        private Controller _controller;
        private PlayerStateMachine _player;
        
        public override void Awake()
        {
            _idleTimer = new Timer();
            _controller = StateMachine.Owner.GetComponent<Controller>();
            _player = Object.FindAnyObjectByType<PlayerStateMachine>();
        }

        public override void Enter()
        {
            _controller.SetSpeed(0);
            
            var cooldown = Random.Range(Payload.StatData.IdleTimeRange.x, Payload.StatData.IdleTimeRange.y);
            _idleTimer.Start(cooldown, false, () =>
            {
                if (_player)
                    StateMachine.SwitchState(Payload.ChaseState);
                else
                {
                    _idleTimer.Restart();
                }
            });
        }
    }
}
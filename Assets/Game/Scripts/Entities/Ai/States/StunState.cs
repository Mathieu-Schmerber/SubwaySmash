using Game.Systems.StateMachine;
using LemonInc.Core.Utilities;

namespace Game.Entities.Ai.States
{
    public class StunState : State<AiStates>
    {
        private Controller _controller;
        private Timer _timer;
        
        public override void Awake()
        {
            _timer = new Timer();
            _controller = StateMachine.Owner.GetComponent<Controller>();
        }

        public override void Enter()
        {
            _timer.Start(Payload.StunTime, false, () => StateMachine.SwitchState(Payload.ChaseState));
            _controller.SetSpeed(0);
        }
    }
}
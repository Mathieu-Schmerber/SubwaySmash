using Game.Systems.StateMachine;
using LemonInc.Core.Utilities;

namespace Game.Entities.Ai.States
{
    public class StunState : State<AiStates>
    {
        private Controller _controller;
        private Timer _timer;
        private StunEffect _stun;
        
        public override void Awake()
        {
            _timer = new Timer();
            _controller = StateMachine.Owner.GetComponent<Controller>();
            _stun = StateMachine.Owner.GetComponentInChildren<StunEffect>();
        }

        public override void Enter()
        {
            _stun.Play();
            _timer.Start(Payload.StunTime, false, () => StateMachine.SwitchState(Payload.ChaseState));
            _controller.SetSpeed(0);
        }
    }
}
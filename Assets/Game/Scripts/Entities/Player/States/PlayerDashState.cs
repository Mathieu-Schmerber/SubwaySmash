using Game.Entities.Player.Abilities;
using Game.Systems.StateMachine;

namespace Game.Entities.Player.States
{
    public class PlayerDashState : State<PlayerStates>
    {
        private DashAbility _dash;
        private PushAbility _push;

        public override void Awake()
        {
            _push = StateMachine.Owner.GetComponent<PushAbility>();
            _dash = StateMachine.Owner.GetComponent<DashAbility>();
        }

        public override void Enter()
        {
            _push.SetLocked(true);
            _dash.Perform(() => StateMachine.SwitchState(Payload.Run));
        }

        public override void Exit()
        {
            _push.SetLocked(false);
        }
    }
}
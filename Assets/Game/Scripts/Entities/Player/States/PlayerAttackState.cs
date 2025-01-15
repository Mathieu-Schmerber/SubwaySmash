using Game.Entities.Player.Abilities;
using Game.Systems.StateMachine;
using LemonInc.Core.Utilities.Extensions;
using UnityEngine;

namespace Game.Entities.Player.States
{
    public class PlayerAttackState : State<PlayerStates>
    {
        private PushAbility _push;
        private DashAbility _dash;
        private Controller _controller;
        private IInputProvider _input;

        public override void Awake()
        {
            _push = StateMachine.Owner.GetComponent<PushAbility>();
            _dash = StateMachine.Owner.GetComponent<DashAbility>();
            _controller = StateMachine.Owner.GetComponent<Controller>();
            _input = StateMachine.Owner.GetComponent<IInputProvider>();
        }

        public override void Enter()
        {
            _dash.SetLocked(true);
            _controller.LockAim(true, _input.AimDirection);
            _push.Perform(() =>
            {
                StateMachine.SwitchState(Payload.Run);
            });
        }

        public override void Exit()
        {
            _controller.LockAim(false);
            _dash.SetLocked(false);
        }
        
        public override void Update()
        {
            _controller.SetDirection(_input.MovementDirection);
        }
    }
}
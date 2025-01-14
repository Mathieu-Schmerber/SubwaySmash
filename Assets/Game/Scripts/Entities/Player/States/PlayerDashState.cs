using Game.Entities.Player.Abilities;
using Game.Systems.StateMachine;
using UnityEngine;

namespace Game.Entities.Player.States
{
    public class PlayerDashState : State<PlayerStates>
    {
        private DashAbility _dash;
        private Animator _animator;

        private static readonly int Dash = Animator.StringToHash("Dash");

        public override void Awake()
        {
            _animator = StateMachine.Owner.GetComponentInChildren<Animator>();
            _dash = StateMachine.Owner.GetComponent<DashAbility>();
        }

        public override void Enter()
        {
            _animator.SetTrigger(Dash);
            _dash.Perform(() => StateMachine.SwitchState(Payload.Idle));
        }
    }
}
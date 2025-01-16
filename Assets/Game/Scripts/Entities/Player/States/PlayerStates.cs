using UnityEngine;

namespace Game.Entities.Player.States
{
    public struct PlayerStates
    {
        public PlayerIdleState Idle { get; set; }
        public PlayerRunState Run { get; set; }
        public PlayerDashState Dash { get; set; }
        public PlayerAttackState Attack { get; set; }
        public PlayerDeadState Dead { get; set; }

        public Vector3 DeathDirection { get; set; }
        public float DeathForce { get; set; }
    }
}
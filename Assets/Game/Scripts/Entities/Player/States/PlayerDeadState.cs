using Game.Entities.Ai;
using Game.Systems.StateMachine;
using UnityEngine;

namespace Game.Entities.Player.States
{
    public class PlayerDeadState : State<PlayerStates> 
    {
        public override void Enter()
        {
            var ragdoll = StateMachine.Owner.GetComponent<RagdollSpawner>().SpawnRagdoll();
            
            ragdoll.AddForce(Payload.DeathDirection * Payload.DeathForce, ForceMode.Impulse);
            Object.Destroy(StateMachine.Owner.gameObject);
        }
    }
}
using Game.Systems.StateMachine;
using UnityEngine;

namespace Game.Entities.Ai
{
    public class DeadState : State<AiStates>
    {
        public override void Enter()
        {
            var ragdoll = StateMachine.Owner.GetComponent<RagdollSpawner>().SpawnRagdoll();
            
            ragdoll.AddForce(Payload.DeathDirection * Payload.DeathForce, ForceMode.Impulse);
            Object.Destroy(StateMachine.Owner.gameObject);
        }
    }
}
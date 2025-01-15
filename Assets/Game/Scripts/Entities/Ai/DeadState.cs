using Game.Systems.StateMachine;
using UnityEngine;

namespace Game.Entities.Ai
{
    public class DeadState : State<AiStates>
    {
        public override void Enter()
        {
            var velocity = StateMachine.Owner.GetComponent<Rigidbody>().linearVelocity;
            var ragdoll = Object.Instantiate(Payload.Ragdoll, StateMachine.Owner.transform.position, StateMachine.Owner.transform.rotation);
            var rb = ragdoll.GetComponent<Rigidbody>();
            
            Debug.Log($"Impulsed Ragdoll {Payload.DeathDirection * Payload.DeathForce}");
            rb.AddForce(Payload.DeathDirection * Payload.DeathForce, ForceMode.Impulse);
            Object.Destroy(StateMachine.Owner.gameObject);
        }
    }
}
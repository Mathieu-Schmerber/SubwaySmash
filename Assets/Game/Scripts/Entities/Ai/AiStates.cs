using UnityEngine;

namespace Game.Entities.Ai
{
    public struct AiStates
    {
        public IdleState IdleState;
        public ChaseState ChaseState;
        public StunState StunState;
        public DeadState DeadState;
        
        public AiStatData StatData;
        public float StunTime;
        public Transform Ragdoll;
        public Vector3 DeathDirection;
        public float DeathForce;
    }
}
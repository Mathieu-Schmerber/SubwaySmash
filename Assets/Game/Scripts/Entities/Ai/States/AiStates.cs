using JetBrains.Annotations;
using UnityEngine;

namespace Game.Entities.Ai.States
{
    public struct AiStates
    {
        public PatrolState PatrolState;
        public ChaseState ChaseState;
        public StunState StunState;
        public DeadState DeadState;
        public AttackState AttackState;

        public bool IsAggressive;
        
        public AiStatData StatData;
        public float StunTime;
        public Vector3 DeathDirection;
        public float DeathForce;
    }
}
using UnityEngine;

namespace Game.Entities.Ai
{
    [CreateAssetMenu(menuName = "Data/AiStat")]
    public class AiStatData : ScriptableObject
    {
        public float WalkSpeed;
        public float RunSpeed;
        public float AttackTriggerDistance;
        public float AttackCooldown;
    }
}
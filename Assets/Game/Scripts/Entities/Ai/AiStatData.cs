using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Entities.Ai
{
    [CreateAssetMenu(menuName = "Data/AiStat")]
    public class AiStatData : ScriptableObject
    {
        public Vector2 IdleTimeRange;
        public float WalkSpeed;
        [FormerlySerializedAs("MovementSpeed")] public float RunSpeed;
        public float AttackTriggerDistance;
        public float AttackCooldown;
    }
}
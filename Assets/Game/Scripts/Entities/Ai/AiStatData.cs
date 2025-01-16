using UnityEngine;

namespace Game.Entities.Ai
{
    [CreateAssetMenu(menuName = "Data/AiStat")]
    public class AiStatData : ScriptableObject
    {
        public Vector2 IdleTimeRange;
        public float MovementSpeed;
        public float AttackTriggerDistance;
        public float AttackCooldown;
    }
}
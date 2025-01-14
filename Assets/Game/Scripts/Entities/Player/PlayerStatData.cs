using UnityEngine;

namespace Game.Entities.Player
{
    [CreateAssetMenu(fileName = "PlayerStatData", menuName = "Data/PlayerStatData")]
    public class PlayerStatData : ScriptableObject
    {
        [Header("General")]
        public float MovementSpeed;
        
        [Header("Dash")]
        public float DashDistance;
        public float DashDuration;
        public float DashCooldown;
    }
}
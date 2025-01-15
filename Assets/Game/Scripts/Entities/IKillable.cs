using UnityEngine;

namespace Game.Entities
{
    public interface IKillable
    {
        bool IsDead { get; }
        void Kill(Vector3 direction, float force);
    }
}
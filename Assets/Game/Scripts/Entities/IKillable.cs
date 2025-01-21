using System;
using UnityEngine;

namespace Game.Entities
{
    public interface IKillable
    {
        bool IsDead { get; }
        event Action OnDeath;
        void Kill(Vector3 direction, float force);
    }
}
using System;
using UnityEngine;

namespace Game.Entities
{
    public abstract class KillableBase : MonoBehaviour, IKillable
    {
        public bool IsDead { get; private set; }
        public event Action OnDeath;
        public void Kill(Vector3 direction, float force)
        {
            if (IsDead)
                return;
            
            IsDead = true;
            OnDeath?.Invoke();
            OnKill(direction, force);
        }
        
        protected abstract void OnKill(Vector3 direction, float force);
    }
}
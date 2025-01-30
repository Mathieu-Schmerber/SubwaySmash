using System;
using FMODUnity;
using Game.Systems.Audio;
using UnityEngine;

namespace Game.Entities
{
    public abstract class KillableBase : MonoBehaviour, IKillable
    {
        [SerializeField] private EventReference _deathAudio;
        public bool IsDead { get; private set; }
        public event Action OnDeath;
        public void Kill(Vector3 direction, float force)
        {
            if (IsDead)
                return;
            
            IsDead = true;
            OnDeath?.Invoke();
            OnKill(direction, force);
            AudioManager.PlayOneShot(_deathAudio);
        }
        
        protected abstract void OnKill(Vector3 direction, float force);
    }
}
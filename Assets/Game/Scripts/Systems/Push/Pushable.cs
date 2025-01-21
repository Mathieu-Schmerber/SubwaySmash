using System;
using UnityEngine;

namespace Game.Systems.Push
{
    [RequireComponent(typeof(Rigidbody))]
    public class Pushable : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        
        public event Action OnPushed;
        
        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void ApplyPush(Vector3 direction, float force)
        {
            if (enabled)
            {
                Push(direction, force);
                OnPushed?.Invoke();
            }
        }

        protected virtual void Push(Vector3 direction, float force)
        {
            _rigidbody.AddForce(direction * force, ForceMode.Impulse);
        }
    }
}
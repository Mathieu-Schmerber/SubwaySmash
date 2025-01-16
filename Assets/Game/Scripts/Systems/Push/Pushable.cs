using System;
using Pixelplacement;
using UnityEngine;

namespace Game.Systems.Push
{
    [RequireComponent(typeof(Rigidbody))]
    public class Pushable : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        
        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void ApplyPush(Vector3 direction, float force)
        {
            if (enabled)
                Push(direction, force);
        }

        protected virtual void Push(Vector3 direction, float force)
        {
            _rigidbody.AddForce(direction * force, ForceMode.Impulse);
        }
    }
}
using System;
using UnityEngine;

namespace Game.Systems.Push
{
    [RequireComponent(typeof(Rigidbody))]
    public class Pushable : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public virtual void ApplyPush(Vector3 direction, float force)
        {
            _rigidbody.AddForce(direction * force, ForceMode.Impulse);
        }
    }
}
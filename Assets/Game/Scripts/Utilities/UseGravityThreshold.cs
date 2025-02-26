using UnityEngine;

namespace Game.Utilities
{
    public class UseGravityThreshold : MonoBehaviour
    {
        private Rigidbody _rb;
        
        [SerializeField] private float _threshold;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            var magnitude = _rb.linearVelocity.magnitude;
            _rb.useGravity = magnitude < _threshold;
        }
    }
}

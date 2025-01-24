using System;
using Game.Entities.Ai;
using LemonInc.Core.Utilities;
using UnityEngine;

namespace Game.Entities.GPE.Train
{
    public class TrainSpawner : MonoBehaviour
    {
        [SerializeField] private float _trainSpeed = 1.0f;
        [SerializeField] private Rigidbody _rb;
        private bool _isTriggered;
        [SerializeField] private Vector3 _startPos;
        [SerializeField] private Transform _train;
        private readonly Timer _timer = new();
        
        private BoxCollider _collider;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
        }

        private void Start()
        {
            _startPos = _train.position;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            var killable = other.GetComponent<IKillable>();
            if (killable == null || !other.GetComponent<RagdollSpawner>()) 
                return;

            other.attachedRigidbody.useGravity = true;
            other.attachedRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            other.attachedRigidbody.linearDamping = 0f;
            _isTriggered = true;
            _timer.Start(3f, false, ResetPosition);
        }
        
        private void FixedUpdate()
        {
            if (_isTriggered)
                _rb.MovePosition(_rb.transform.position + (transform.forward * _trainSpeed * Time.fixedDeltaTime));
        }

        private void ResetPosition()
        {
            _train.position = _startPos;
            _rb.linearVelocity = Vector3.zero;
            _isTriggered = false;
        }
    }
}
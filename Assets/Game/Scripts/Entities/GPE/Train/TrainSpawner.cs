using System;
using FMODUnity;
using Game.Systems.Audio;
using LemonInc.Core.Utilities;
using UnityEngine;

namespace Game.Entities.GPE.Train
{
    public class TrainSpawner : MonoBehaviour
    {
        [SerializeField] private float _trainSpeed = 1.0f;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private Transform _train;
        private bool _isTriggered;
        private Vector3 _startPos;
        private readonly Timer _timer = new();

        [SerializeField] private EventReference _audio;
        
        [SerializeField] private float _resetCooldown = 1f;

        private void Start()
        {
            _startPos = _train.position;
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (_isTriggered)
                return;
            
            var killable = other.GetComponent<IKillable>();
            if (killable == null) 
                return;

            AudioManager.PlayOneShot(_audio);
            other.attachedRigidbody.useGravity = true;
            other.attachedRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            other.attachedRigidbody.linearDamping = 0f;
            _isTriggered = true;
            _timer.Start(_resetCooldown, false, ResetPosition);
        }

        private void FixedUpdate()
        {
            if (_isTriggered)
                _rb.linearVelocity = transform.forward * _trainSpeed;
        }

        private void ResetPosition()
        {
            _train.position = _startPos;
            _rb.linearVelocity = Vector3.zero;
            _isTriggered = false;
        }
    }
}
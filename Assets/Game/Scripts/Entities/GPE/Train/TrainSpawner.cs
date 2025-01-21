using LemonInc.Core.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

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

        // Update is called once per frame
        private void Start()
        {
            _startPos = _train.position;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("NPC")) return;
            _isTriggered = true;
            _timer.Start(3f, false, ResetPosition);
        }
        private void FixedUpdate()
        {
            if (_isTriggered)
            {
                //_rb.linearVelocity = transform.forward * (_trainSpeed * Time.fixedDeltaTime);
                _rb.MovePosition(_rb.transform.position + (transform.forward * _trainSpeed * Time.fixedDeltaTime));
                Debug.Log(_trainSpeed);
            }
        }

        private void ResetPosition()
        {
            _train.position = _startPos;
            _rb.linearVelocity = Vector3.zero;
            _isTriggered = false;
        }
    }
}
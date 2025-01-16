using LemonInc.Core.Utilities;
using UnityEngine;

namespace Game.Entities.GPE.Train
{
    public class TrainSpawner : MonoBehaviour
    {
        [SerializeField] float _trainSpeed = 1.0f;
        [SerializeField] Rigidbody _rb;
        bool _isTriggered;
        [SerializeField] Vector3 _StartPos;
        [SerializeField] Transform _Train;
        Timer _timer = new();

        // Update is called once per frame
        private void Start()
        {
            _StartPos = _Train.position;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "NPC")
            {
                _isTriggered = true;
                _timer.Start(3f, false, ResetPosition);
            }

        }
        private void FixedUpdate()
        {
            if (_isTriggered)
            {
                _rb.linearVelocity = transform.forward * _trainSpeed * Time.fixedDeltaTime *10;
            }
        }
        void ResetPosition()
        {
            _Train.position = _StartPos;
            _rb.linearVelocity = Vector3.zero;
            _isTriggered = false;
        }
    }
}
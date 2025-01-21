using Game.Inputs;
using LemonInc.Core.Utilities.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Entities
{
    public class Controller : MonoBehaviour
    {
	    private static readonly int Speed = Animator.StringToHash("Speed");

	    [Header("Movement")] 
	    [SerializeField] private float _accelerationLerp;
	    [SerializeField] private bool _lerpTurns;
	    [SerializeField, ShowIf(nameof(_lerpTurns))] private float _turnLerp;
		
        [Header("Graphics")]
        [SerializeField] private float _turnSpeed;
        [SerializeField] private Transform _graphic;

        private Animator _animator;
		private Rigidbody _rb;
		private IInputProvider _inputProvider;

		private float _speed;
		private float _targetSpeed;
		private Vector3 _input;
		private Vector3 _targetInput;
		private Quaternion _targetRotation;
		private bool _aimLocked;
		private float _maxSpeed;

		private void Awake()
		{
			_rb = GetComponent<Rigidbody>();
			_inputProvider = GetComponent<IInputProvider>();
			_animator = _graphic.GetComponentInChildren<Animator>();
		}

        private void Update()
        {
	        // Graphics
	        if (_input.magnitude > 0 && !_aimLocked)
	        {
		        _targetRotation = Quaternion.LookRotation(_input.WithY(0));
		        _graphic.transform.rotation = Quaternion.Lerp(_graphic.transform.rotation, _targetRotation,
			        Mathf.Clamp(_turnSpeed * Time.deltaTime, 0, .99f));
	        }

	        // Lerp speed to target speed
	        _speed = Mathf.Lerp(_speed, _targetSpeed, _accelerationLerp * Time.deltaTime);
	        if (_maxSpeed > 0)
		        _animator.SetFloat(Speed, _speed / _maxSpeed);
	        else
	        {
		        _animator.SetFloat(Speed, _speed > 0 ? 1 : 0);
	        }

	        // Lerp input
	        _input = Vector3.Lerp(_input, _targetInput, _turnLerp * Time.deltaTime);
        }

        private void FixedUpdate()
        {
	        var velocity = _input * _speed;
			if (velocity.magnitude == 0)
				return;
			
			_rb.linearVelocity = new Vector3(velocity.x, _rb.linearVelocity.y, velocity.z);
        }

        public void SetMaxSpeed(float maxSpeed) => _maxSpeed = maxSpeed;
        
		public void SetSpeed(float speed)
		{
			if (_speed > speed)
				_speed = speed;

			_targetSpeed = speed;
		}

		public void SetDirection(Vector3 direction, bool forced = false)
		{
			if (_lerpTurns && !forced)
				_targetInput = direction;
			else
				_input = direction;
		}

		public void LockAim(bool lockAim, Vector3? forceRotation = null)
		{
			_aimLocked = lockAim;
			if (!_aimLocked) return;
			
			var dirInput = forceRotation ?? _inputProvider.AimDirection;
			_targetRotation = Quaternion.LookRotation(dirInput.WithY(0));
			_graphic.transform.rotation = _targetRotation;
		}

		public float CalculateTargetSpeed(float distance, float duration)
		{
			var tolerance = 0.01f;
			float low = 0f, high = 100f; // Adjust range as needed for target speed
			var targetSpeed = 0f;

			while (high - low > tolerance)
			{
				targetSpeed = (low + high) / 2f;
				var totalDistance = ComputeDistance(targetSpeed, duration, _accelerationLerp);

				if (totalDistance < distance)
					low = targetSpeed;
				else
					high = targetSpeed;
			}

			return targetSpeed;
		}

		private float ComputeDistance(float targetSpeed, float duration, float accelerationLerp)
		{
			var distance = 0f;
			var currentSpeed = 0f;
			var deltaTime = 0.01f; // Simulation step

			for (float t = 0; t < duration; t += deltaTime)
			{
				currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, accelerationLerp * deltaTime);
				distance += currentSpeed * deltaTime;
			}

			return distance;
		}
	}
}
using System;
using Game.Entities.Player;
using LemonInc.Core.Utilities;
using UnityEngine;

namespace Game.Entities
{
    public class Controller : MonoBehaviour
    {
	    [Header("Movement")] 
	    [SerializeField] private float _accelerationLerp;
		
        [Header("Graphics")]
        [SerializeField] private float _turnSpeed;
        [SerializeField] private Transform _graphic;

		private Rigidbody _rb;
		private IInputProvider _inputProvider;

		private readonly Timer _moveAbilityTimer = new();

		private float _speed;
		private float _targetSpeed;
		private Vector3 _input;
		private Quaternion _targetRotation;
		private bool _aimLocked;

		private void Awake()
		{
			_rb = GetComponent<Rigidbody>();
			_inputProvider = GetComponent<IInputProvider>();
		}

        private void Update()
        {
	        // Graphics
	        if (_input.magnitude > 0)
	        {
		        _targetRotation = Quaternion.LookRotation(_input);
		        _graphic.rotation = Quaternion.Lerp(_graphic.rotation, _targetRotation,
			        Mathf.Clamp(_turnSpeed * Time.deltaTime, 0, .99f));
	        }

	        // Lerp speed to target speed
	        _speed = Mathf.Lerp(_speed, _targetSpeed, _accelerationLerp * Time.deltaTime);
        }

        private void FixedUpdate()
		{
			var movement = _input * (_speed * Time.fixedDeltaTime);
			_rb.MovePosition(_rb.position + movement);
        }

		public void SetSpeed(float speed)
		{
			if (_speed > speed)
				_speed = speed;

			_targetSpeed = speed;
		}

		public void SetDirection(Vector3 direction)
		{
			_input = direction;
		}

		public void LockAim(bool lockAim, Vector3? forceRotation = null)
		{
			_aimLocked = lockAim;
			if (!_aimLocked) return;
			
			var dirInput = forceRotation ?? _inputProvider.AimDirection;
			_targetRotation = Quaternion.LookRotation(dirInput);
			_graphic.rotation = _targetRotation;
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
		
		public void SetMoveAbilityCooldown(float cooldown) => _moveAbilityTimer.Start(cooldown, false);
		public bool CanPerformMoveAbility() => _moveAbilityTimer.IsOver();
		public void RestartMoveAbilityCooldown() => _moveAbilityTimer.Restart();
	}
}
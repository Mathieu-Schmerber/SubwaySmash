using System;
using Game.Entities.Player;
using Game.Systems.Inputs;
using LemonInc.Core.Input;
using LemonInc.Core.Utilities.Extensions;
using UnityEngine;

namespace Game.Inputs
{
    internal class ControlledInputSet : InputSet
    {
        private Vector3[] _waypoints;
        private int _index;
        private float _minDistance = 0.15f;
        private readonly PlayerInputProvider _inputProvider;
        private Vector3 _movementDirection;
        private Vector3 _aimDirection;
        public event Action OnReleased;

        public ControlledInputSet(Transform transform) : base(transform)
        {
            _inputProvider = transform.GetComponent<PlayerInputProvider>();
        }

        public override Vector3 MovementDirection => _movementDirection;
        public override Vector3 AimDirection => _aimDirection;
        public override InputState Dash { get; }
        public override InputState Push { get; }

        public bool HasControl { get; private set; }

        public void Update()
        {
            if (_index >= _waypoints.Length)
            {
                HasControl = false;
                return;
            }

            GoTo(_waypoints[_index]);

            if (Vector3.Distance(Owner.position, _waypoints[_index]) < _minDistance)
            {
                _index++;
                if (_index >= _waypoints.Length)
                {
                    _inputProvider.ReleaseControl();
                    HasControl = false;
                    OnReleased?.Invoke();
                }
            }

        }

        private void GoTo(Vector3 targetPosition)
        {
            _movementDirection = (targetPosition.WithY(Owner.position.y) - Owner.position).normalized;
            _aimDirection = _movementDirection;
        }

        public void Init(Vector3[] waypoints)
        {
            _index = 1;
            HasControl = true;
            _waypoints = waypoints;
        }
    }
}
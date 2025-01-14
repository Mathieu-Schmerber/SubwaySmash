using System;
using LemonInc.Core.Input;
using LemonInc.Core.Utilities.Extensions;
using UnityEngine;

namespace Game.Entities.Player
{
    /// <summary>
    /// Provides player inputs.
    /// </summary>
    public class PlayerInputProvider : InputProviderBase<Controls>, IInputProvider
    {
        private readonly PhysicalInputValue<Vector2, Vector3> _movement = new(vector2 => vector2
            .ToVector3Xz()
            .ToIsometric()
            .normalized
        );
        
        private readonly PhysicalInput _dash = new();
        private PhysicalInputValue<Vector2, Vector3> _aim;

        public Vector3 MovementDirection => _movement.Value;
        public Vector3 AimDirection => _aim.Value;
        public InputState Dash => _dash;

        protected override void Awake()
        {
            base.Awake();
            _aim ??= new PhysicalInputValue<Vector2, Vector3>(CalculateAimDirection);
        }

        protected override void SubscribeInputs()
        {
            _movement.Subscribe(Controls.Player.Movement);
            _dash.Subscribe(Controls.Player.Dash);
            _aim.Subscribe(Controls.Player.Aim);
        }

        protected override void UnSubscribeInputs()
        {
            _movement.UnSubscribe();
            _dash.UnSubscribe();
            _aim.UnSubscribe();
        }
        
        private Vector3 CalculateAimPosition(Vector2 mousePosition)
        {
            var floorPlane = new Plane(Vector3.up, transform.position.y);
            var ray = Camera.main!.ScreenPointToRay(mousePosition);

            return floorPlane.Raycast(ray, out var enter) ? ray.GetPoint(enter) : Vector3.zero;
        }

        private Vector3 CalculateAimDirection(Vector2 mousePosition)
        {
            var worldMousePosition = CalculateAimPosition(mousePosition);
            return (worldMousePosition - transform.position).normalized;
        }
    }
}
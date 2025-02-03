using LemonInc.Core.Input;
using LemonInc.Core.Utilities.Extensions;
using UnityEngine;

namespace Game.Systems.Inputs
{
    public class PhysicalInputSet : InputSet
    {
        private readonly PhysicalInputValue<Vector2, Vector3> _movement = new(vector2 => vector2
            .ToVector3Xz()
            .ToIsometric()
            .normalized
        );
        
        private readonly PhysicalInput _dash = new();
        private readonly PhysicalInput _push = new();
        private readonly PhysicalInputValue<Vector2, Vector3> _aim;

        public override Vector3 MovementDirection => _movement.Value;
        public override Vector3 AimDirection => _aim.Value;
        public override InputState Dash => _dash;
        public override InputState Push => _push;

        public PhysicalInputSet(Transform owner) : base(owner)
        {
            _aim = new PhysicalInputValue<Vector2, Vector3>(CalculateAimDirection);
        }
        
        public void Subscribe(Controls controls)
        {
            _movement.Subscribe(controls.Player.Movement);
            _dash.Subscribe(controls.Player.Dash);
            _push.Subscribe(controls.Player.Push);
            _aim.Subscribe(controls.Player.Aim);
        }

        public void UnSubscribe()
        {
            _movement.UnSubscribe();
            _dash.UnSubscribe();
            _aim.UnSubscribe();
            _push.UnSubscribe();
        }
        
        private Vector3 CalculateAimPosition(Vector2 mousePosition)
        {
            var floorPlane = new Plane(Vector3.up, Owner.position);
            var ray = Core.Camera.ScreenPointToRay(mousePosition);
            
            return floorPlane.Raycast(ray, out var enter) ? ray.GetPoint(enter) : Vector3.zero;
        }

        private Vector3 CalculateAimDirection(Vector2 mousePosition)
        {
            var worldMousePosition = CalculateAimPosition(mousePosition);
            Debug.DrawRay(worldMousePosition, Vector3.up, Color.yellow);
            return (worldMousePosition - Owner.position).normalized;
        }
    }
}
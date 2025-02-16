using System;
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
        
        private readonly PhysicalInput _push = new();
        private readonly PhysicalInputValue<Vector2, Vector3> _aim;

        public override Vector3 MovementDirection => _movement.Value;
        public override Vector3 AimDirection => _aim.Value;
        public override InputState Push => _push;

        public PhysicalInputSet(Transform owner, Func<Vector2, Vector3> calculateAimDirection) : base(owner)
        {
            _aim = new PhysicalInputValue<Vector2, Vector3>(calculateAimDirection);
        }
        
        public void Subscribe(Controls controls)
        {
            _movement.Subscribe(controls.Player.Movement);
            _push.Subscribe(controls.Player.Push);
            _aim.Subscribe(controls.Player.Aim);
        }

        public void UnSubscribe()
        {
            _movement.UnSubscribe();
            _aim.UnSubscribe();
            _push.UnSubscribe();
        }
    }
}
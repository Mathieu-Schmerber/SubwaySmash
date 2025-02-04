using LemonInc.Core.Input;
using UnityEngine;

namespace Game.Systems.Inputs
{
    public abstract class InputSet : IInputProvider
    {
        protected readonly Transform Owner;

        public abstract Vector3 MovementDirection { get; }
        public abstract Vector3 AimDirection { get; }
        public abstract InputState Dash { get; }
        public abstract InputState Push { get; }

        protected InputSet(Transform owner)
        {
            Owner = owner;
        }
    }
}
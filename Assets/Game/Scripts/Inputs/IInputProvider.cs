using LemonInc.Core.Input;
using UnityEngine;

namespace Game.Inputs
{
    public interface IInputProvider
    {
        public Vector3 MovementDirection { get; }
        public Vector3 AimDirection { get; }
        public InputState Dash { get; }
        public InputState Push { get; }
    }
}
using LemonInc.Core.Input;
using UnityEngine;

namespace Game.Systems.Inputs
{
    public interface IInputProvider
    {
        public Vector3 MovementDirection { get; }
        public Vector3 AimDirection { get; }
        public InputState Push { get; }
    }
}
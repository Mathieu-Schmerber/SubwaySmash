using Game.Systems.Inputs;
using LemonInc.Core.Input;
using UnityEngine;

namespace Game.Entities.Player
{
    /// <summary>
    /// Provides player inputs.
    /// </summary>
    public class PlayerInputProvider : InputProviderBase<Controls>, IInputProvider
    {
        private PhysicalInputSet _physicalInputSet;
        private IInputProvider _currentInputSet;

        public Vector3 MovementDirection => _currentInputSet.MovementDirection;
        public Vector3 AimDirection => _currentInputSet.AimDirection;
        public InputState Dash => _currentInputSet.Dash;
        public InputState Push => _currentInputSet.Push;
        
        protected override void Awake()
        {
            base.Awake();
            _physicalInputSet = new PhysicalInputSet(transform);
            _currentInputSet = _physicalInputSet;
        }

        public void TakeControl(InputSet inputSet)
        {
            _currentInputSet = inputSet;
        }

        public void ReleaseControl()
        {
            _currentInputSet = _physicalInputSet;
        }
        
        protected override void SubscribeInputs()
        {
            _physicalInputSet.Subscribe(Controls);
        }

        protected override void UnSubscribeInputs()
        {
            _physicalInputSet.UnSubscribe();
        }
    }
}
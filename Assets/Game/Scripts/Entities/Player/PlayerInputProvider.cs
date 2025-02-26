using Game.Systems;
using Game.Systems.Inputs;
using LemonInc.Core.Input;
using LemonInc.Core.Utilities.Extensions;
using Sirenix.OdinInspector;
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
        public InputState Push => _currentInputSet.Push;
        
        [ReadOnly, ShowInInspector] public ControlType ControlType => InUseControlType;
        
        protected override void Awake()
        {
            base.Awake();
            _physicalInputSet = new PhysicalInputSet(transform, CalculateAimDirection);
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
        
        private Vector3 CalculateAimPosition(Vector2 input)
        {
            // Input is the mouse position in screen space
            if (InUseControlType == ControlType.KEYBOARD)
            {
                var floorPlane = new Plane(Vector3.up, transform.position);
                var ray = Core.Camera.ScreenPointToRay(input);

                return floorPlane.Raycast(ray, out var enter) ? ray.GetPoint(enter) : Vector3.zero;
            }
            
            // Input is a controller's joystick axis
            return transform.position + input.ToVector3Xz().ToIsometric();
        }

        private Vector3 CalculateAimDirection(Vector2 input)
        {
            var worldAimPoint = CalculateAimPosition(input);
            Debug.DrawRay(worldAimPoint, Vector3.up, Color.yellow);
            return (worldAimPoint - transform.position).normalized;
        }
    }
}
using Game.Systems.StateMachine;
using Game.Systems.Waypoint;
using UnityEngine;

namespace Game.Entities.Ai.States
{
    public class EscapeState : State<AiStates>
    {
        private static readonly int Speed = Animator.StringToHash("Speed");
        private AiBrain _brain;
        private Controller _controller;
        private Animator _animator;

        public override void Awake()
        {
            _brain = StateMachine.Owner.GetComponent<AiBrain>();
            _controller = StateMachine.Owner.GetComponent<Controller>();
            _animator = StateMachine.Owner.GetComponentInChildren<Animator>();
        }

        public override void Enter()
        {
            _controller.SetSpeed(Payload.StatData.RunSpeed);
            _animator.SetFloat(Speed, 1);

            var exit = GetNearestExit();
            if (exit) 
                _brain.SetTarget(exit.transform);
        }

        private Exit GetNearestExit()
        {
            var distance = float.MaxValue;
            Exit target = null;
            
            foreach (var exit in Core.LevelExists)
            {
                var distanceToExit = Vector3.Distance(exit.transform.position, StateMachine.Owner.transform.position);
                if (distanceToExit < distance)
                {
                    distance = distanceToExit;
                    target = exit;
                }
            }

            return target;
        }
        
        public override void Update()
        {
            _controller.SetDirection(_brain.MovementDirection);
        }

        public override void Exit()
        {
        }
    }
}
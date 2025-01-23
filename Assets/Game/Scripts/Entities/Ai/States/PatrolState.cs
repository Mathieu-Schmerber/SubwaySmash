using Game.Systems.Alert;
using Game.Systems.StateMachine;
using UnityEngine;

namespace Game.Entities.Ai.States
{
    public class PatrolState : State<AiStates>
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
            Core.AlertSystem.OnAlertRaised += OnAlertLevel;
            if (EvaluateAlertLevel())
                return;
            
            if (_brain.HasWaypoints())
            {
                _animator.SetFloat(Speed, .5f);
                _controller.SetSpeed(Payload.StatData.WalkSpeed);
                _brain.ResumeFollowWaypoints();
            }
            else
            {
                _animator.SetFloat(Speed, 0);
                _controller.SetSpeed(0);
            }
        }
        
        public override void Update()
        {
            _controller.SetDirection(_brain.MovementDirection);
        }
        
        private bool EvaluateAlertLevel()
        {
            if (Core.AlertSystem.AlertLevel == AlertLevel.LOW)
                return false;
            
            StateMachine.SwitchState(Payload.IsAggressive ? Payload.ChaseState : Payload.EscapeState);
            return true;
        }

        private void OnAlertLevel(AlertLevel level) => EvaluateAlertLevel();
        
        public override void Exit()
        {
            Core.AlertSystem.OnAlertRaised -= OnAlertLevel;
        }
    }
}
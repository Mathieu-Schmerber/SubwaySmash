using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Entities;
using Game.Entities.Player;
using Game.Systems.Inputs;
using Game.Systems.Waypoint;
using LemonInc.Core.Input;
using LemonInc.Core.Utilities;
using LemonInc.Core.Utilities.Extensions;
using Pixelplacement;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TMPro;
using Unity.VisualScripting;

namespace Game.Systems.Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private TutorialPart[] _tutorialParts;
        private int _tutorialPartIndex = 0;
        [SerializeField] private float _transitionTime;
        private PlayerInputProvider _playerInputProvider;
        private ControlledInputSet _controlledInput;

        private void Awake()
        {
            _playerInputProvider = Core.Player.GetComponent<PlayerInputProvider>();
            var current = _tutorialParts[_tutorialPartIndex];
            SubscribeAndInitialize(current);
            _controlledInput = new ControlledInputSet(Core.Player);
        }

        private void OnEnable()
        {
            _controlledInput.OnReleased += OnControlResumed;
        }

        private void OnControlResumed()
        {
            _tutorialParts[_tutorialPartIndex-1].CompletionText.SetActive(false);
            var current = _tutorialParts[_tutorialPartIndex];
            
            Core.AlertSystem.ResetAlert();
            Core.AlertSystem.LockAlert(current.InitialState.ForceAlertLevel,current.InitialState.AlertLevel);
            foreach (var ai in current.InitialState.NPCs)
            {
                ai.enabled = true;
            }
        }

        private void OnDisable()
        {
            _controlledInput.OnReleased -= OnControlResumed;
        }

        private void Update()
        {
            if (_controlledInput.HasControl)
                _controlledInput.Update();
        }

        private void Start()
        {
            
            foreach (var tutorialPart in _tutorialParts)
            {
                foreach (var ai in tutorialPart.InitialState.NPCs)
                {
                    ai.enabled = false;
                }
            }
            OnControlResumed();
            
        }

        private void SubscribeAndInitialize(TutorialPart tutorialPart)
        {
            tutorialPart.OnCompleted += OnCurrentPartCompleted;
            tutorialPart.Initialize();
        }

        private TutorialPart GoNextTutorialPart()
        {
            var current = _tutorialParts[_tutorialPartIndex];
            current.CompletionText.SetActive(true);
            current.OnCompleted -= OnCurrentPartCompleted;
            current.Exit();
            
            if (!IsLastPart())
            {
                _tutorialPartIndex++;
                var next = _tutorialParts[_tutorialPartIndex];
                SubscribeAndInitialize(next);
                return next; 
            }
            throw new Exception("Already reached the last part.");
        }

        private void OnCurrentPartCompleted()
        {
            if (!IsLastPart())
            {
                var current = _tutorialParts[_tutorialPartIndex];
                var next = GoNextTutorialPart();
                TransitionTo(current, next);
            }
            else
            {
                EndTutorial();
            }
            
        }

        private void EndTutorial()
        {
            Core.Instance.LoadNextStage();
        }

        private bool IsLastPart()
        {
            return _tutorialPartIndex >= _tutorialParts.Length - 1;
        }

        private void TransitionTo(TutorialPart previous, TutorialPart next)
        {
            var previousWaypoints = previous.InitialState.PlayerPositions;
            var nextWaypoint = next.InitialState.PlayerPositions.First();
            var waypoints = previousWaypoints.Concat(new []{nextWaypoint}).ToArray();
            
            _controlledInput.Init(waypoints);
            _playerInputProvider.TakeControl(_controlledInput);
            Tween.Position(Core.CameraRig, next.InitialState.CameraPosition, _transitionTime, 0, Tween.EaseOut);
        }


        private void OnDrawGizmosSelected()
        {
            for (int i = 0; i < _tutorialParts.Length; i++)
            {
                Gizmos.color = Color.yellow;
                var part = _tutorialParts[i];
                for (int j = 0; j < part.InitialState.PlayerPositions.Length; j++)
                {
                    Gizmos.DrawWireCube(part.InitialState.PlayerPositions[j],new Vector3(1,.1f,1));
                    if (j + 1 < part.InitialState.PlayerPositions.Length)
                    {
                        var nextpoint = part.InitialState.PlayerPositions[j + 1];
                        Gizmos.DrawLine(part.InitialState.PlayerPositions[j], nextpoint);
                    }
                }
                
                if (i + 1 < _tutorialParts.Length)
                {
                    var nextPart = _tutorialParts[i + 1];
                    if (nextPart.InitialState.PlayerPositions.Length > 0)
                    {
                        Gizmos.color = new Color(1f, 0.65f, 0f);
                        Gizmos.DrawLine(part.InitialState.PlayerPositions.Last(), nextPart.InitialState.PlayerPositions.First()); 
                    }
                }
            }
        }
    }

    internal class ControlledInputSet : InputSet
    {
        private Vector3[] _initialStatePlayerPositions;
        private int _index;
        private float _minDistance = 0.15f;
        private readonly PlayerInputProvider _inputProvider;
        private Vector3 _movementDirection;
        private Vector3 _aimDirection;
        public event Action OnReleased;
        

        public ControlledInputSet(Transform transform) : base(transform)
        {
            _inputProvider = transform.GetComponent<PlayerInputProvider>();
        }
        
        public override Vector3 MovementDirection => _movementDirection;
        public override Vector3 AimDirection => _aimDirection;
        public override InputState Dash { get; }
        public override InputState Push { get; }
        
        public bool HasControl { get; private set; }

        public void Update()
        {
            if (_index >= _initialStatePlayerPositions.Length)
            {
                HasControl = false;
                return;
            }
            
            GoTo(_initialStatePlayerPositions[_index]);
            
            if (Vector3.Distance(Owner.position, _initialStatePlayerPositions[_index]) < _minDistance)
            {
                _index++;
                if (_index >= _initialStatePlayerPositions.Length)
                {
                    _inputProvider.ReleaseControl();
                    HasControl = false;
                    OnReleased?.Invoke();
                }
            }
            
        }

        private void GoTo(Vector3 targetPosition)
        {
            _movementDirection = (targetPosition.WithY(Owner.position.y) - Owner.position).normalized;
            _aimDirection = _movementDirection;
        }

        public void Init(Vector3[] waypoints)
        {
            _index = 0;
            HasControl = true;
            _initialStatePlayerPositions = waypoints;
        }
    }
}


using System;
using Game.Entities.Ai;
using Game.Systems.Alert;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Systems.Tutorial
{
    [Serializable]
    public class InitialState
    {
        public Vector3[] PlayerPositions;
        public Vector3 CameraPosition;
        public bool ForceAlertLevel;
        [ShowIf(nameof(ForceAlertLevel))]
        public AlertLevel AlertLevel;

        public AiStateMachine[] NPCs;

#if UNITY_EDITOR
        [Button]
        private void TakeCameraPosition()
        {
            CameraPosition = Core.CameraRig.position;
        }
#endif
    }
    
    [Serializable]
    public class TutorialPart
    {
        public string Name;
        public GameObject CompletionText;
        public TutorialCondition[] Conditions;
        public InitialState InitialState;
        public event Action OnCompleted;
        

        public void Initialize()
        {
            foreach (var condition in Conditions)
            {
                condition.OnConditionChanged += OnConditionChanged;
            }
        }
        
        public void Exit()
        {
            foreach (var condition in Conditions)
            {
                condition.OnConditionChanged -= OnConditionChanged;
            }
        }
        
        private void OnConditionChanged(bool status)
        {
            foreach (var condition in Conditions)
            {
                if (!condition.IsVerified)
                    return;
            }
            OnCompleted?.Invoke();
        }
#if UNITY_EDITOR
        [Button]
        private void Preview()
        {
            Core.CameraRig.position = InitialState.CameraPosition;
            Core.Player.position = InitialState.PlayerPositions[0];
        }
#endif
        
    }
}
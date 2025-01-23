using System;
using Game.Entities.Player;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Systems.Alert
{
    public class AlertSystem : MonoBehaviour
    {
        private bool _locked;
        [ReadOnly] public AlertLevel AlertLevel { get; private set; }

        private void OnEnable()
        {
            PlayerStateMachine.OnPlayerDeath += OnPlayerDeath;
            Core.ScoreSystem.OnScoreUpdated += OnScoreUpdated;
        }

        private void OnPlayerDeath(Transform obj)
        {
            _locked = true;
            AlertLevel = AlertLevel.LOW;
        }

        private void OnDisable()
        {
            Core.ScoreSystem.OnScoreUpdated -= OnScoreUpdated;
            PlayerStateMachine.OnPlayerDeath -= OnPlayerDeath;
        }
        
        public void RaiseAlert()
        {
            if (_locked)
                return;
            
            AlertLevel = AlertLevel.HIGH;
        }
        
        private void OnScoreUpdated(float value)
        {
            RaiseAlert();
        }
    }
}
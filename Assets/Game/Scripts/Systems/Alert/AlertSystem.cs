using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Systems.Alert
{
    public class AlertSystem : MonoBehaviour
    {
        [ReadOnly] public AlertLevel AlertLevel { get; private set; }

        public event Action<AlertLevel> OnAlertRaised;

        private void OnEnable()
        {
            Core.ScoreSystem.OnScoreUpdated += OnScoreUpdated;
        }

        private void OnDisable()
        {
            Core.ScoreSystem.OnScoreUpdated -= OnScoreUpdated;
        }
        
        public void RaiseAlert()
        {
            AlertLevel = AlertLevel.HIGH;
            OnAlertRaised?.Invoke(AlertLevel);
        }
        
        private void OnScoreUpdated(float value)
        {
            RaiseAlert();
        }
    }
}
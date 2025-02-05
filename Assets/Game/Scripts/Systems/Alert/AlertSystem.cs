using System;
using Game.Entities.Player;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Systems.Alert
{
    public class AlertSystem : MonoBehaviour
    {
        private bool _locked;
        private AlertLight[] _alertLights;
        [ReadOnly] public AlertLevel AlertLevel { get; private set; }

        private void Awake()
        {
            _alertLights = FindObjectsByType<AlertLight>(FindObjectsSortMode.None);
        }

        private void Start()
        {
            if (_alertLights.Length == 0)
                Debug.LogError("No AlertLights in the scene.");
        }

        private void OnEnable()
        {
            PlayerStateMachine.OnPlayerDeath += OnPlayerDeath;
        }

        private void OnPlayerDeath(Transform obj)
        {
            _locked = true;
            AlertLevel = AlertLevel.LOW;
        }

        private void OnDisable()
        {
            PlayerStateMachine.OnPlayerDeath -= OnPlayerDeath;
        }

        public void RaiseAlert()
        {
            if (_locked || AlertLevel == AlertLevel.HIGH)
                return;

            foreach (var alert in _alertLights)
                alert.Play();
            AlertLevel = AlertLevel.HIGH;
        }

        public void ResetAlert()
        {
            AlertLevel = AlertLevel.LOW;
            foreach (var alert in _alertLights)
                alert.Stop();
        }

        public void LockAlert(bool state, AlertLevel value = AlertLevel.LOW)
        {
            _locked = state;
            if (state)
            {
                AlertLevel = value; 
            }
        }
    }
}
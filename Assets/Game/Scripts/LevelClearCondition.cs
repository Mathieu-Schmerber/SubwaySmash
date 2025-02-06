using System;
using Game.Entities;
using Game.Entities.Ai;
using Game.MainMenu;
using Game.Systems.Waypoint;
using UnityEngine;

namespace Game
{
    public enum FailReason
    {
        NPC_ESCAPED,
        PLAYER_DEATH
    }
    
    public class LevelClearCondition : MonoBehaviour
    {
        [SerializeField] private bool _handlePlayerDeath = true;
        [SerializeField] private bool _handleNpcEscape = true;
        [SerializeField] private bool _handleNpcDeath = true;
        
        private AiStateMachine[] _ais;
        private IKillable _player;

        private bool _loseRaised;
        
        public static event Action OnLevelCleared;
        public static event Action<FailReason> OnLevelFailed;
        
        private void Awake()
        {
            _player = Core.Player.GetComponent<IKillable>();
            _ais = FindObjectsByType<AiStateMachine>(FindObjectsSortMode.None);
        }

        private void OnEnable()
        {
            _player.OnDeath += OnPlayerDeath;
            Exit.OnEscaped += OnNpcEscape;
            foreach (var ai in _ais)
            {
                ai.OnDeath += OnNpcDeath;
            }
        }
        private void OnDisable()
        {
            if (_player != null)
                _player.OnDeath -= OnPlayerDeath;
            Exit.OnEscaped -= OnNpcEscape;
            foreach (var ai in _ais)
            {
                ai.OnDeath -= OnNpcDeath;
            }
        }

        private void OnPlayerDeath()
        {
            if (_handlePlayerDeath)
            {
                Core.NotificationManager.PushNotification(NotificationUi.NotificationType.DEATH);
                RaiseLoseCondition(FailReason.PLAYER_DEATH);
            }
        }

        private void OnNpcEscape()
        {
            if (_handleNpcEscape)
            {
                Core.NotificationManager.PushNotification(NotificationUi.NotificationType.ESCAPED);
                RaiseLoseCondition(FailReason.NPC_ESCAPED);
            }
        }

        private void OnNpcDeath()
        {
            if (!_handleNpcDeath)
                return;
            
            foreach (var ai in _ais)
            {
                if (ai.IsDead)
                    ai.OnDeath -= OnNpcDeath;
                else
                    return;
            }

            Core.NotificationManager.PushNotification(NotificationUi.NotificationType.LEVEL_CLEAR);
            RaiseWinCondition();
        }

        private void RaiseWinCondition()
        {
            if (_loseRaised)
                return;
            
            OnLevelCleared?.Invoke();
            Invoke(nameof(LoadNextStage), 3f);
        }

        private void RaiseLoseCondition(FailReason reason)
        {
            if (_loseRaised)
                return;
            
            _loseRaised = true;
            OnLevelFailed?.Invoke(reason);
            Invoke(nameof(RestartStage), 3f);
        }
        
        private void LoadNextStage()
        {
            if (_player.IsDead)
                RestartStage();
            else
                Core.Instance.LoadNextStage();
        }

        private void RestartStage()
        {
            Core.AlertSystem.ResetAlert();
            Core.Instance.RestartLevel();
        }
    }
}
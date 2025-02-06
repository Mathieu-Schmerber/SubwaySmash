using System;
using FMODUnity;
using Game.Systems.Audio;
using LemonInc.Core.Utilities;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.MainMenu
{
    public class NotificationUi : MonoBehaviour
    {
        public enum NotificationType
        {
            LEVEL_CLEAR = 0,
            ESCAPED = 1,
            DEATH = 2
        }
        
        [Serializable]
        public struct Notification
        {
            public EventReference Audio;
            public Color Color;
            public Sprite Picto;
            public string Text;
        }
        
        [Serializable]
        public class NotificationSettings : SerializedDictionary<NotificationType, Notification> { }
        
        [SerializeField] private NotificationSettings _notificationSettings;
        [SerializeField] private NotificationType _notificationType;

        [SerializeField] private Image _image;
        private TextMeshProUGUI _text;
        private MMF_Player _feedback;

        private void Awake()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _feedback = GetComponent<MMF_Player>();
        }

        public void BindType(NotificationType notificationType)
        {
            _feedback.PlayFeedbacks();
            _notificationType = notificationType;
            _image.sprite = _notificationSettings[_notificationType].Picto;
            _image.color = _notificationSettings[_notificationType].Color;
            _text.text = _notificationSettings[_notificationType].Text;
            AudioManager.PlayOneShot(_notificationSettings[_notificationType].Audio);
        }

        private void OnValidate()
        {
            if (_notificationSettings?.ContainsKey(_notificationType) == true)
            {
                _image.sprite = _notificationSettings[_notificationType].Picto;
                _image.color = _notificationSettings[_notificationType].Color;
                GetComponentInChildren<TextMeshProUGUI>().text = _notificationSettings[_notificationType].Text;
            }
        }
    }
}
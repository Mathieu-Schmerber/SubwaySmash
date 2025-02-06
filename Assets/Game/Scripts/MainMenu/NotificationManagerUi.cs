using UnityEngine;

namespace Game.MainMenu
{
    public class NotificationManagerUi : MonoBehaviour
    {
        [SerializeField] private GameObject _notificationPrefab;

        public void PushNotification(NotificationUi.NotificationType notificationType)
        {
            var instance = Instantiate(_notificationPrefab, transform);
            instance.GetComponent<NotificationUi>().BindType(notificationType);
        }
    }
}
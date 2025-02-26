using UnityEngine;

namespace Game.Ui.Notifications
{
    public class NotificationManagerUi : MonoBehaviour
    {
        [SerializeField] private GameObject _notificationPrefab;

        public void PushNotification(NotificationUi.NotificationType notificationType)
        {
            var instance = Instantiate(_notificationPrefab, transform);
            instance.GetComponent<NotificationUi>().BindType(notificationType);
        }

        public void UnPushNotification(NotificationUi.NotificationType notificationType)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                var notif = child.GetComponent<NotificationUi>();

                if (notif.NotifType == notificationType)
                    notif.UnBind();
            }
        }
    }
}
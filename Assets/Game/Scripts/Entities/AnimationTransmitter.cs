using UnityEngine;

namespace Game.Entities
{
    public class AnimationTransmitter : MonoBehaviour
    {
        public void OnAnimationEvent(string eventName)
        {
            transform.root.SendMessageUpwards("OnAnimationEventReceived", eventName, SendMessageOptions.DontRequireReceiver);
        }
    }
}
using System;
using Game.Entities.Ai.Abilities;
using UnityEngine;

namespace Game.Entities
{
    public class AnimationTransmitter : MonoBehaviour
    {
        private IAnimationEventListener[] _listeners;

        private void Awake()
        {
            _listeners = GetComponentsInParent<IAnimationEventListener>();
        }

        public void OnAnimationEvent(string eventName)
        {
            foreach (var listener in _listeners)
                listener.OnAnimationEventReceived(eventName);
        }
    }
}
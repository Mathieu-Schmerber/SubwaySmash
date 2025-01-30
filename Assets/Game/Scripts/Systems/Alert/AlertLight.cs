using FMODUnity;
using Game.Systems.Audio;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Systems.Alert
{
    public class AlertLight : MonoBehaviour
    {
        [SerializeField] private EventReference _audio;
        private MMF_Player[] _feedbacks;

        private void Awake()
        {
            _feedbacks = GetComponentsInChildren<MMF_Player>();
        }

        [Button]
        public void Play()
        {
            AudioManager.PlayOneShot(_audio);
            foreach (var player in _feedbacks)
                player.PlayFeedbacks();
        }
    }
}
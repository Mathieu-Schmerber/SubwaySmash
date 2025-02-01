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
            Hide();
        }

        public void Play()
        {
            AudioManager.PlayOneShot(_audio);
            foreach (var player in _feedbacks)
                player.PlayFeedbacks();
        }

        [Button]
        private void Show()
        {
            _feedbacks = GetComponentsInChildren<MMF_Player>();
            foreach (var player in _feedbacks)
            {
                var light = player.GetFeedbackOfType<MMF_Light>();
                light.BoundLight.intensity = light.RemapIntensityOne;
            }
        }

        [Button]
        private void Hide()
        {
            _feedbacks = GetComponentsInChildren<MMF_Player>();
            foreach (var player in _feedbacks)
            {
                var light = player.GetFeedbackOfType<MMF_Light>();
                light.BoundLight.intensity = 0;
            }
        }
    }
}
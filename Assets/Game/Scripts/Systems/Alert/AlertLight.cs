using FMOD.Studio;
using FMODUnity;
using Game.Systems.Audio;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Game.Systems.Alert
{
    public class AlertLight : MonoBehaviour
    {
        [SerializeField] private EventReference _audio;
        private MMF_Player[] _feedbacks;
        private EventInstance? _audioInstance;

        private void Awake()
        {
            _feedbacks = GetComponentsInChildren<MMF_Player>();
            Hide();
        }

        public void Play()
        {
            _audioInstance = AudioManager.PlayOneShot(_audio);
            foreach (var player in _feedbacks)
                player.PlayFeedbacks();
        }
        
        public void Stop()
        {
            AudioManager.StopSound(_audioInstance, STOP_MODE.ALLOWFADEOUT);
            foreach (var player in _feedbacks)
                player.ResetFeedbacks();
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
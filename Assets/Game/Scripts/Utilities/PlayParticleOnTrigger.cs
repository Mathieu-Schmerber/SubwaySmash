using FMODUnity;
using Game.Systems.Audio;
using UnityEngine;

namespace Game.Utilities
{
    public class PlayParticleOnTrigger : MonoBehaviour
    {
        [SerializeField] private LayerMask _triggerLayerMask;
        [SerializeField] private EventReference _audio;
        private ParticleSystem _ps;

        private float _lastAudioPlayTime;
        private const float AUDIO_COOLDOWN = 0.15f;

        private void Awake()
        {
            _ps = GetComponent<ParticleSystem>();
            _lastAudioPlayTime = -AUDIO_COOLDOWN;
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((_triggerLayerMask.value & (1 << other.gameObject.layer)) != 0)
            {
                if (_ps != null && Time.time - _lastAudioPlayTime >= AUDIO_COOLDOWN)
                {
                    AudioManager.PlayOneShot(_audio, transform.position);
                    _ps.Play();
                    _lastAudioPlayTime = Time.time;
                }
            }
        }
    }
}
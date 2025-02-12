using System.Collections;
using Databases;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Systems.Audio
{
    public class PhysicsSound : MonoBehaviour
    {
        #if UNITY_EDITOR
        [ValueDropdown(nameof(GetAudioEntry))]
        #endif
        [SerializeField] private string _materialName;
        [SerializeField] private LayerMask _ignoreOnStay;

        private Vector3 _lastContactNormal;
        private MaterialAudio _audio;
        private float _lastSoundPlayTime;
        private float _soundCooldown = 0.15f;

        private void Awake()
        {
            if (!RuntimeDatabase.Data.Audio.MaterialAudios.TryGetValue(_materialName, out _audio))
                Debug.LogError("Audio material not found: " + _materialName);
        }

        private void OnCollisionEnter(Collision collision)
        {
            foreach (var contact in collision.contacts)
            {
                if (contact.normal != _lastContactNormal)
                {
                    var impactIntensity = collision.relativeVelocity.magnitude;
                    
                    if (impactIntensity < _audio.MinForce * 2 || !CanPlaySound())
                        continue;

                    // Calculate mass-adjusted intensity
                    float massFactor = collision.rigidbody != null ? collision.rigidbody.mass : 1f;
                    float intensityWithMass = impactIntensity * massFactor;

                    // Normalize impact to 0 - 1, incorporating mass
                    var normalizedIntensity = Mathf.Clamp01((intensityWithMass - _audio.MinForce) / (_audio.MaxForce - _audio.MinForce));
                    AudioManager.PlayOneShot(_audio.FmodEvent, transform.position, normalizedIntensity);

                    _lastContactNormal = contact.normal;
                    _lastSoundPlayTime = Time.time;
                }
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            if (_ignoreOnStay == (_ignoreOnStay | (1 << collision.gameObject.layer)))
            {
                return; // Ignore collisions for objects on the specified layers
            }

            foreach (var contact in collision.contacts)
            {
                if (contact.normal != _lastContactNormal)
                {
                    var impactIntensity = collision.relativeVelocity.magnitude;

                    if (impactIntensity < _audio.MinForce / 2f || !CanPlaySound())
                        continue;

                    // Calculate mass-adjusted intensity
                    float massFactor = collision.rigidbody != null ? collision.rigidbody.mass : 1f;
                    float intensityWithMass = impactIntensity * massFactor;

                    // Normalize impact to 0 - 1, incorporating mass
                    var normalizedIntensity = Mathf.Clamp01((intensityWithMass - _audio.MinForce) / (_audio.MaxForce - _audio.MinForce));
                    AudioManager.PlayOneShot(_audio.FmodEvent, transform.position, normalizedIntensity);

                    _lastContactNormal = contact.normal;
                    _lastSoundPlayTime = Time.time;
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            _lastContactNormal = Vector3.zero;
        }

        private bool CanPlaySound()
        {
            return Time.time >= _lastSoundPlayTime + _soundCooldown;
        }
        
#if UNITY_EDITOR
        private IEnumerable GetAudioEntry()
        {
            var data = RuntimeDatabase.Data.Audio;
            return data.GetKeys();
        }
#endif
    }
}

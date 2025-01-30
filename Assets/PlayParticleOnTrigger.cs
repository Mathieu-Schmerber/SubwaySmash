using System;
using FMODUnity;
using Game.Systems.Audio;
using UnityEngine;

public class PlayParticleOnTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask triggerLayerMask;
    [SerializeField] private EventReference audio;
    private ParticleSystem _ps;

    private float _lastAudioPlayTime;
    private const float AudioCooldown = 0.15f;

    private void Awake()
    {
        _ps = GetComponent<ParticleSystem>();
        _lastAudioPlayTime = -AudioCooldown; // Allow immediate first play
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((triggerLayerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            if (_ps != null && Time.time - _lastAudioPlayTime >= AudioCooldown)
            {
                AudioManager.PlayOneShot(audio, transform.position);
                _ps.Play();
                _lastAudioPlayTime = Time.time;
            }
        }
    }
}
using System;
using FMODUnity;
using Game.Systems.Audio;
using UnityEngine;

public class PlayParticleOnTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask triggerLayerMask;
    [SerializeField] private EventReference audio;
    private ParticleSystem _ps;

    private void Awake()
    {
        _ps = GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((triggerLayerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            if (_ps != null)
            {
                AudioManager.PlayOneShot(audio, transform.position);
                _ps.Play();
            }
        }
    }
}
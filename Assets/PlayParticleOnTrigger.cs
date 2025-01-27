using System;
using UnityEngine;

public class PlayParticleOnTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask triggerLayerMask;
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
                _ps.Play();
            }
        }
    }
}
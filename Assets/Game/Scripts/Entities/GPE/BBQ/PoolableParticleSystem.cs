using System;
using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using FMODUnity;
using Game.Systems.Audio;
using LemonInc.Core.Pooling.Contracts;
using UnityEngine;

namespace Game.Entities.GPE.BBQ
{
    public class PoolableParticleSystem : PoolableBase
    {
        [SerializeField] private EventReference _audio;
        [SerializeField] private float _volume = .1f;
        private List<ParticleSystem> _ps;
        private EventInstance? _instance;

        private void Awake()
        {
            _ps = transform.GetComponentsInChildren<ParticleSystem>().ToList();
            _ps.Add(GetComponent<ParticleSystem>());
        }

        protected override void OnInitialize(object data)
        {
            _ps.ForEach(x => x.Play(true));
        }
        
        private void OnEnable()
        {
            _instance = AudioManager.PlayOneShot(_audio, transform.position, _volume);
        }

        private void OnDisable()
        {
            AudioManager.StopSound(_instance);
        }

        protected override void OnRelease()
        {
            AudioManager.StopSound(_instance);
        }

        public void StopThenRelease()
        {
            _ps.ForEach(x => x.Stop(true, ParticleSystemStopBehavior.StopEmitting));
            var time = _ps.Max(x => x.main.startLifetime.constantMax) + _ps.Max(x => x.main.startDelay.constantMax);
            Invoke(nameof(Release), time);
        }
    }
}
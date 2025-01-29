using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using Game.Systems.Audio;
using LemonInc.Core.Pooling.Contracts;
using UnityEngine;

namespace Game.Entities.GPE.BBQ
{
    public class PoolableParticleSystem : PoolableBase
    {
        [SerializeField] private EventReference _audio;
        private List<ParticleSystem> _ps;
        
        private void Awake()
        {
            _ps = transform.GetComponentsInChildren<ParticleSystem>().ToList();
            _ps.Add(GetComponent<ParticleSystem>());
        }

        protected override void OnInitialize(object data)
        {
            AudioManager.PlayOneShot(_audio);
            _ps.ForEach(x => x.Play(true));
        }

        protected override void OnRelease() { }

        public void StopThenRelease()
        {
            _ps.ForEach(x => x.Stop(true, ParticleSystemStopBehavior.StopEmitting));
            var time = _ps.Max(x => x.main.startLifetime.constantMax) + _ps.Max(x => x.main.startDelay.constantMax);
            Invoke(nameof(Release), time);
        }
    }
}
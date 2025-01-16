using System;
using Game.Systems.Push;
using LemonInc.Core.Pooling;
using LemonInc.Core.Pooling.Contracts;
using UnityEngine;

namespace Game.Entities.GPE.BBQ
{
    public class OnFire : MonoBehaviour
    {
        private PoolableParticleSystem _particle;
        private IKillable _killable;

        private float _killTimer;
        private float _igniteTimer;
        private FireSettings _settings;
        private float _startSpreadTimer;

        private void Awake()
        {
            _killable = GetComponent<IKillable>();
        }

        private void Configure(FireSettings fireSettings)
        {
            _settings = new FireSettings
            {
                TimeToKill = fireSettings.TimeToKill,
                IgniteTime = fireSettings.IgniteTime,
                CapSpreadChain = fireSettings.CapSpreadChain,
                SpreadRadius = fireSettings.SpreadRadius,
                MaxSpreadChain = fireSettings.MaxSpreadChain - 1,
                StartSpreadAfterTime = fireSettings.StartSpreadAfterTime,
            };
            
            _igniteTimer = fireSettings.IgniteTime;
            _killTimer = fireSettings.TimeToKill;
            _startSpreadTimer = fireSettings.StartSpreadAfterTime;
        }

        public static void IgniteSurroundings(Vector3 center, float radius, FireSettings fireSettings)
        {
            var hits = Physics.OverlapSphere(center, radius);

            foreach (var hit in hits)
            {
                var affected = hit.GetComponent<Pushable>() || hit.GetComponent<IKillable>() != null;
                if (!affected || hit.GetComponent<OnFire>() || hit.GetComponent<Barbecue>())
                    continue;

                var onFire = hit.gameObject.AddComponent<OnFire>();
                onFire.Configure(fireSettings);
            }
        }

        private void Start()
        {
            _particle = Core.Pooling.From(Pool.FX_OnFire).Get(null, fx =>
            {
                fx.Instance.transform.SetParent(transform);
                fx.Instance.transform.localPosition = Vector3.zero;
                fx.Instance.transform.localRotation = Quaternion.identity;
            }) as PoolableParticleSystem;
        }

        private void Update()
        {
            if (_startSpreadTimer > 0)
                _startSpreadTimer -= Time.deltaTime;

            if (_startSpreadTimer > 0)
                return;
            
            if (_killTimer > 0)
                _killTimer -= Time.deltaTime;
            if (_igniteTimer > 0)
                _igniteTimer -= Time.deltaTime;

            // Handle killing the object
            if (_killTimer <= 0 && _killable != null)
            {
                _killable.Kill(Vector3.zero, 0);
                _killTimer = -1; // Ensure this action doesn't repeat
            }

            // Handle stopping the particle system
            if (_igniteTimer <= 0)
            {
                if (_particle != null)
                {
                    _particle.StopThenRelease();
                }
                _igniteTimer = -1; // Ensure this action doesn't repeat
            }

            // Spread fire once ignite time is reached
            if (_igniteTimer > 0 && (!_settings.CapSpreadChain || _settings is { CapSpreadChain: true, MaxSpreadChain: > 0 }))
                IgniteSurroundings(transform.position, _settings.SpreadRadius, _settings);
        }
    }
}

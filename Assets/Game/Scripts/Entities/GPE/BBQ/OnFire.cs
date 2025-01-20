using LemonInc.Core.Pooling;
using UnityEngine;

namespace Game.Entities.GPE.BBQ
{
    public class OnFire : MonoBehaviour
    {
        private IKillable _killable;

        private float _killTimer;
        private float _igniteTimer;
        private FireSettings _settings;
        private float _startSpreadTimer;
        private Ignitable[] _ignitables;

        private void Awake()
        {
            _killable = GetComponent<IKillable>();
            _ignitables = GetComponents<Ignitable>();
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

        public void Copy(OnFire onfire)
        {
            _settings = onfire._settings;
            _igniteTimer = onfire._igniteTimer;
            _killTimer = onfire._killTimer;
            _startSpreadTimer = onfire._startSpreadTimer;
        }

        public static void IgniteSurroundings(Vector3 center, float radius, FireSettings fireSettings)
        {
            var hits = Physics.OverlapSphere(center, radius);

            foreach (var hit in hits)
            {
                var affected = hit.GetComponent<Ignitable>();
                if (!affected || hit.GetComponent<OnFire>() || hit.GetComponent<Barbecue>())
                    continue;

                var onFire = hit.gameObject.AddComponent<OnFire>();
                onFire.Configure(fireSettings);
            }
        }

        private void Start()
        {
            foreach (var ignitable in _ignitables)
            {
                ignitable.StartIgnite(_settings.IgniteTime);
            }
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

            // Spread fire once ignite time is reached
            if (_igniteTimer > 0 && (!_settings.CapSpreadChain || _settings is { CapSpreadChain: true, MaxSpreadChain: > 0 }))
                IgniteSurroundings(transform.position, _settings.SpreadRadius, _settings);
        }
    }
}

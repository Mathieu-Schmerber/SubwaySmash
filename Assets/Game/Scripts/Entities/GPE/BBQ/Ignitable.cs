using System.Collections.Generic;
using System.Linq;
using Databases;
using Game.Entities.Ai;
using UnityEngine;

namespace Game.Entities.GPE.BBQ
{
    public class Ignitable : MonoBehaviour
    {
        private const float BURN_TIME = 3f;
        
        [SerializeField, Range(0, 1)] private float _carbonatedProgress;
        private Color _targetBurnColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
        private GameObject _onFireFX;
        private ParticleSystem _fx;
        private float _burnTime;
        private bool _isBurning;
        private List<Renderer> _renderers;
        private List<Color[]> _originalColors;

        public float BurnTime => _burnTime;
        public float BurnProgress => _carbonatedProgress;

        private void Awake()
        {
            _onFireFX = RuntimeDatabase.Data.OnFireFx;
            _carbonatedProgress = 0;
            _renderers = GetComponentsInChildren<Renderer>().ToList();
            _renderers.RemoveAll(x => x is ParticleSystemRenderer or SpriteRenderer || x.tag.Equals("FX"));
            StoreOriginalColors();
        }

        private void StoreOriginalColors()
        {
            _originalColors = new List<Color[]>();
            foreach (var renderer in _renderers)
            {
                _originalColors.Add(renderer.materials.Select(mat => mat.color).ToArray());
            }
        }

        public void StartIgnite(float burnTime, float initialProgress = 0)
        {
            if (_isBurning) return;
            
            if (GetComponent<AiStateMachine>())
                Core.AlertSystem.RaiseAlert();
            
            var instance = Instantiate(_onFireFX, transform.position, transform.rotation, transform);
            _fx = instance.GetComponent<ParticleSystem>();
            _burnTime = burnTime;
            _isBurning = true;
            _carbonatedProgress = initialProgress;
        }

        private void Update()
        {
            if (!_isBurning) return;

            _carbonatedProgress += Time.deltaTime / BURN_TIME;
            _carbonatedProgress = Mathf.Clamp01(_carbonatedProgress);

            ApplyColorBlend();

            if (_carbonatedProgress >= 1)
            {
                _isBurning = false;
                _fx.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }
        
        private void ApplyColorBlend()
        {
            for (var i = 0; i < _renderers.Count; i++)
            {
                var render = _renderers[i];
                var originalColors = _originalColors[i];
                for (var j = 0; j < render.materials.Length; j++)
                {
                    render.materials[j].color = Color.Lerp(originalColors[j], _targetBurnColor, _carbonatedProgress);
                }
            }
        }
    }
}
using System;
using System.Linq;
using UnityEngine;

namespace Game.Utilities.Effects
{
    public class Glow : MonoBehaviour
    {
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

        [Serializable]
        private struct GlowLerp
        {
            public Color Color;
            public float EmissionIntensity;
            public float Time; // Time to wait at this state before lerping to the next
        }

        [SerializeField] private GlowLerp[] glowStates = new GlowLerp[2]
        {
            new() { Color = Color.white, EmissionIntensity = 0f, Time = .7f },
            new() { Color = Color.white, EmissionIntensity = 0.4f, Time = 0f }
        };
        [SerializeField] private float lerpDuration = .5f; // Duration of one lerp cycle
        [SerializeField] private Color defaultEmissionColor = Color.black; // Default emission color (no glow)
        [SerializeField] private float defaultEmissionIntensity = 0f; // Default emission intensity (no glow)
        [SerializeField] private float timeOffsetRange = 0.5f; // Maximum random offset for each object
        [SerializeField] private LayerMask _ignore;

        private Renderer[] _renderers;
        private float _lerpTime;
        private float _stateWaitTimer;
        private int _currentStateIndex;
        private bool _isWaiting = false;

        // Timer offset for each object
        private float _startOffset;

        private void Start()
        {
            // Randomize the start offset for each object to stagger the start times
            _startOffset = UnityEngine.Random.Range(0f, timeOffsetRange);

            // Get all MeshRenderers in the scene
            _renderers = GetComponentsInChildren<Renderer>()
                .Where(x => !IsLayerInLayerMask(x.gameObject.layer, _ignore))
                .ToArray(); // To include children objects
            
            // Enable the emission keyword to modify emission properties for all renderers
            foreach (var renderer in _renderers)
            {
                if (renderer != null && renderer.material != null)
                {
                    renderer.material.EnableKeyword("_EMISSION");
                }
            }

            if (glowStates.Length > 0)
            {
                // Initialize the material with the first state's emission color and intensity
                GlowLerp initialGlow = glowStates[_currentStateIndex];
                ApplyGlowToRenderers(initialGlow.Color, initialGlow.EmissionIntensity);

                // Apply random offset to the wait timer to stagger the glow start times
                _stateWaitTimer = initialGlow.Time + _startOffset;
            }
        }

        private bool IsLayerInLayerMask(int layer, LayerMask layerMask)
        {
            return (layerMask.value & (1 << layer)) != 0;
        }
        
        private void Update()
        {
            if (_renderers.Length == 0 || glowStates.Length < 2) return;

            // Handle waiting state
            if (_isWaiting)
            {
                _stateWaitTimer -= Time.deltaTime;
                if (_stateWaitTimer <= 0f)
                {
                    _isWaiting = false;
                    _lerpTime = 0f; // Reset lerp time for the next transition
                }
                return;
            }

            // Calculate indices for the current and next GlowLerp states
            int nextStateIndex = (_currentStateIndex + 1) % glowStates.Length;

            // Update the lerp time
            _lerpTime += Time.deltaTime / lerpDuration;

            // If lerp completes, switch to the next state and enter the waiting phase
            if (_lerpTime >= 1f)
            {
                _lerpTime = 1f;
                _currentStateIndex = nextStateIndex;

                GlowLerp nextGlow = glowStates[_currentStateIndex];
                _stateWaitTimer = nextGlow.Time;
                _isWaiting = true;
            }

            // Lerp between the current and next GlowLerp states
            GlowLerp currentGlow = glowStates[_currentStateIndex];
            GlowLerp nextGlowLerp = glowStates[nextStateIndex];

            Color currentColor = Color.Lerp(currentGlow.Color, nextGlowLerp.Color, _lerpTime);
            float currentIntensity = Mathf.Lerp(currentGlow.EmissionIntensity, nextGlowLerp.EmissionIntensity, _lerpTime);

            // Apply the lerped emission color and intensity to all renderers
            ApplyGlowToRenderers(currentColor, currentIntensity);
        }

        // Method to apply the glow effect to all MeshRenderers
        private void ApplyGlowToRenderers(Color color, float intensity)
        {
            foreach (var renderer in _renderers)
            {
                if (renderer != null && renderer.material != null)
                {
                    renderer.material.SetColor(EmissionColor, color * intensity);
                }
            }
        }

        // Set the material's emission to the default state (no glow)
        public void SetDefault()
        {
            foreach (var renderer in _renderers)
            {
                if (renderer != null && renderer.material != null)
                {
                    renderer.material.SetColor(EmissionColor, defaultEmissionColor * defaultEmissionIntensity);
                }
            }
        }
    }
}

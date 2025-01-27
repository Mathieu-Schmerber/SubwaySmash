using System;
using UnityEngine;

namespace Game
{
    public class Glow : MonoBehaviour
    {
        [Serializable]
        private struct GlowLerp
        {
            public Color Color;
            public float EmissionIntensity;
            public float Time; // Time to wait at this state before lerping to the next
        }

        [SerializeField] private GlowLerp[] glowStates; // Array of GlowLerp states
        [SerializeField] private float lerpDuration = 2f; // Duration of one lerp cycle

        private Material _material;
        private float _lerpTime;
        private float _stateWaitTimer;
        private int _currentStateIndex;
        private bool _isWaiting = false;

        private void Start()
        {
            // Get the material instance from the Renderer
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                _material = renderer.material;
            }

            if (_material != null)
            {
                // Enable the emission keyword to modify emission properties
                _material.EnableKeyword("_EMISSION");
            }

            if (glowStates.Length > 0)
            {
                // Initialize the material with the first state's emission color and intensity
                GlowLerp initialGlow = glowStates[_currentStateIndex];
                _material.SetColor("_EmissionColor", initialGlow.Color * initialGlow.EmissionIntensity);
                _stateWaitTimer = initialGlow.Time;
            }
        }

        private void Update()
        {
            if (_material == null || glowStates.Length < 2) return;

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

            // Set the emission color with intensity
            _material.SetColor("_EmissionColor", currentColor * currentIntensity);
        }
    }
}
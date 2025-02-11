using Game.Systems.Tutorial;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

namespace Game.Entities.GPE
{
    public class BoxedArea : TutorialCondition
    {
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
        
        [Header("Settings")]
        [SerializeField] private int _elementCountToValidate;
        [SerializeField] private LayerMask _include;
        
        [Header("Feedback")]
        [SerializeField] private Gradient _gradient;
        [SerializeField] private float _lerpDuration;
        [SerializeField] private float _slideSpeed;
        
        private TextMeshPro _text;
        private LineRenderer[] _lines;
        private readonly List<GameObject> _containedObjects = new();

        private void Awake()
        {
            _lines = GetComponentsInChildren<LineRenderer>();
            _text = GetComponentInChildren<TextMeshPro>();
        }

        private void Start()
        {
            if (_elementCountToValidate == 1)
                _text.text = $"";
            else
                _text.text = $"0/{_elementCountToValidate}";
            
            ApplyLineColor(_gradient.Evaluate(0));
        }

        private void OnValidate()
        {
            _text = GetComponentInChildren<TextMeshPro>();
            if (_elementCountToValidate == 1)
                _text.text = $"";
            else
                _text.text = $"0/{_elementCountToValidate}";
        }

        private void Update()
        {
            if (_lines is not { Length: > 0 }) 
                return;

            if (_elementCountToValidate > 1)
                _text.text = $"{_containedObjects.Count}/{_elementCountToValidate}";
            
            foreach (var line in _lines)
            {
                var offset = line.material.mainTextureOffset;
                offset.x += _slideSpeed * Time.deltaTime;
                line.material.mainTextureOffset = offset;
            }
        }

        private void SetValid()
        {
            ApplyLineColor(_gradient.Evaluate(1));
            VerifyIfReady();
        }

        private void SetInvalid()
        {
            StartCoroutine(LerpLineColors());
            UnVerify();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & _include) != 0)
            {
                if (!_containedObjects.Contains(other.gameObject))
                {
                    _containedObjects.Add(other.gameObject);
                    SetValid();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (((1 << other.gameObject.layer) & _include) != 0)
            {
                if (_containedObjects.Contains(other.gameObject))
                {
                    _containedObjects.Remove(other.gameObject);
                    SetValid();
                    if (_containedObjects.Count == 0)
                        SetInvalid();
                }
            }
        }

        private void ApplyLineColor(Color color)
        {
            foreach (var line in _lines)
                line.material.SetColor(BaseColor, color);
        }

        private IEnumerator<WaitForSeconds> LerpLineColors()
        {
            var time = 0f;

            while (time < _lerpDuration)
            {
                var t = time / _lerpDuration;
                var lerpColor = _gradient.Evaluate(t);
                ApplyLineColor(lerpColor);
                time += Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }

            ApplyLineColor(_gradient.colorKeys[_gradient.colorKeys.Length - 1].color);
        }

        private void VerifyIfReady()
        {
            var completionPercentage = Mathf.Clamp01((float)_containedObjects.Count / _elementCountToValidate);
            var lerpColor = _gradient.Evaluate(completionPercentage);
            ApplyLineColor(lerpColor);

            if (_containedObjects.Count == _elementCountToValidate)
                Verify();
        }
    }
}

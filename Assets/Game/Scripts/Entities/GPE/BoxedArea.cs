using Game.Systems.Tutorial;
using UnityEngine;
using System.Collections.Generic;
using LemonInc.Core.Utilities;
using MoreMountains.Feedbacks;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using TMPro;
using UnityEngine.UI;

namespace Game.Entities.GPE
{
    public class BoxedArea : TutorialCondition
    {
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
        
        [Header("Settings")]
        [SerializeField] private int _elementCountToValidate;
        [SerializeField] private LayerMask _include;
        [SerializeField] private float _timeToValidate;
        
        [Header("Feedback")]
        [SerializeField] private Gradient _gradient;
        [SerializeField] private float _lerpDuration;
        [SerializeField] private float _slideSpeed;
        [SerializeField] private MMF_Player _textChangedFeedback;
        [SerializeField] private MMF_Player _validateFeedback;
        [SerializeField] private MMF_Player _unvalidateFeedback;
        [SerializeField] private Image _loadingImage;
        
        private Timer _validateTimer;
        private TextMeshProUGUI _text;
        private LineRenderer[] _lines;
        private readonly List<GameObject> _containedObjects = new();
        private TweenBase _tween;

        private void Awake()
        {
            _validateTimer = new Timer();
            _lines = GetComponentsInChildren<LineRenderer>();
            _text = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Start()
        {
            UpdateText();
        }

        private void UpdateText()
        {
            _textChangedFeedback.PlayFeedbacks();
            if (_elementCountToValidate == 1)
                _text.text = $"";
            else
                _text.text = $"{_containedObjects.Count}/{_elementCountToValidate}";
        }
    
        private void OnValidate()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
            if (_elementCountToValidate == 1)
                _text.text = $"";
            else
                _text.text = $"{_containedObjects.Count}/{_elementCountToValidate}";
        }

        private void Update()
        {
            if (_lines is not { Length: > 0 }) 
                return;

            foreach (var line in _lines)
            {
                var offset = line.material.mainTextureOffset;
                offset.x += _slideSpeed * Time.deltaTime;
                line.material.mainTextureOffset = offset;
            }
        }

        private void SetValid()
        {
            if (_tween == null && _containedObjects.Count >= _elementCountToValidate)
            {
                _validateTimer.Start(_timeToValidate, false, OnValidateTick);
                _text.text = "";
                _tween = Tween.Value(0, 1f, value => _loadingImage.fillAmount = value, _timeToValidate, 0, Tween.EaseInOut);
            }
        }

        private void OnValidateTick()
        {
            if (_containedObjects.Count >= _elementCountToValidate)
            {
                Verify();
                _validateFeedback.PlayFeedbacks();
            }
        }

        private void SetInvalid()
        {
            if (_tween != null)
            {
                _tween.Cancel();
                _tween = null;
                _unvalidateFeedback.PlayFeedbacks();
            }

            ApplyColors();
            if (IsVerified)
                UnVerify();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & _include) != 0)
            {
                if (!_containedObjects.Contains(other.gameObject))
                {
                    _containedObjects.Add(other.gameObject);
                    if (_tween == null)
                        UpdateText();
                    SetValid();
                }
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (((1 << other.gameObject.layer) & _include) != 0)
            {
                if (_containedObjects.Contains(other.gameObject))
                {
                    _containedObjects.Remove(other.gameObject);
                    if (_containedObjects.Count < _elementCountToValidate)
                    {
                        SetInvalid();
                        UpdateText();
                    }
                }
            }
        }

        private void ApplyLineColor(Color color)
        {
            foreach (var line in _lines)
                line.sharedMaterial.SetColor(BaseColor, color);
        }

        private void ApplyColors()
        {
            var completionPercentage = Mathf.Clamp01(Mathf.Clamp((float)_containedObjects.Count, 0, _elementCountToValidate) / _elementCountToValidate);
            var lerpColor = _gradient.Evaluate(completionPercentage);
            ApplyLineColor(lerpColor);
        }
    }
}

using System;
using Game.Systems.Tutorial;
using UnityEngine;
using System.Collections.Generic;

namespace Game.Entities.GPE
{
    public class BoxedArea : TutorialCondition
    {
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
        [SerializeField] private LayerMask _include;
        [SerializeField] private Gradient _gradient;
        [SerializeField] private float _lerpDuration;
        [SerializeField] private float _slideSpeed;

        private LineRenderer[] _lines;
        private readonly List<GameObject> _containedObjects = new();

        private void Awake()
        {
            _lines = GetComponentsInChildren<LineRenderer>();
        }

        private void Start()
        {
            SetInvalid();
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
            ApplyLineColor(_gradient.colorKeys[0].color);
            Verify();
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
                var lerpedColor = _gradient.Evaluate(t);
                ApplyLineColor(lerpedColor);
                time += Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }

            ApplyLineColor(_gradient.colorKeys[_gradient.colorKeys.Length - 1].color);
        }
    }
}

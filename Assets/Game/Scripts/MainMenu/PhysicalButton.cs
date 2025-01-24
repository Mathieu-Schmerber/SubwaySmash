using System;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

namespace Game.MainMenu
{
    public class PhysicalButton : MonoBehaviour
    {
        [SerializeField] private string _text;
        [SerializeField] private MMF_Player _selectFeedback;
     
        private bool _isSelected = false;

        private void OnValidate()
        {
            var tmpText = GetComponentInChildren<TextMeshProUGUI>();
            if (tmpText)
                tmpText.text = _text;
        }

        private void OnTriggerStay(Collider other)
        {
            if (_isSelected)
                return;
            
            _isSelected = true;
            _selectFeedback.PlayFeedbacks();
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!_isSelected)
                return;
            
            _isSelected = false;
            _selectFeedback.PlayFeedbacks();
        }
    }
}
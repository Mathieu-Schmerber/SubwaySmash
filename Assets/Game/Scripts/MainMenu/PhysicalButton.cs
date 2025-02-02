using System;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Game.MainMenu
{
    public class PhysicalButton : MonoBehaviour
    {
        [SerializeField] private string _text;
        [SerializeField] private MMF_Player _selectFeedback;
        [SerializeField] private MMF_Player _deselectFeedback;
        [SerializeField] private MMF_Player _clickFeedback;
        [SerializeField] private UnityEvent _onClick;
        private TextMeshProUGUI _textMeshpro;
        
        public bool Interactable { get; set; }
        public bool IsSelected { get; set; }

        private void Awake()
        {
            _textMeshpro = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void SetText(string text)
        {
            _text = text;
            _textMeshpro.text = text;
        }

        public void AddListener(Action callback)
        {
            _onClick.AddListener(() =>
            {
                callback?.Invoke();
            });
        }
        
        private void OnValidate()
        {
            var tmpText = GetComponentInChildren<TextMeshProUGUI>();
            if (tmpText)
                tmpText.text = _text;
        }

        private void OnTriggerStay(Collider other)
        {
            if (IsSelected || !Interactable)
                return;
            
            IsSelected = true;
            _selectFeedback.PlayFeedbacks();
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!IsSelected || !Interactable)
                return;
            
            IsSelected = false;
            _deselectFeedback.PlayFeedbacks();
        }

        private void OnEnable()
        {
            Core.MenuInput.SelectMenu.OnPressed += Click;
        }
        
        private void OnDisable()
        {
            if (Core.Instance && Core.MenuInput)
                Core.MenuInput.SelectMenu.OnPressed -= Click;
        }

        private void Click()
        {
            if (!IsSelected || !Interactable) 
                return;
            
            _clickFeedback.PlayFeedbacks();
            _deselectFeedback.PlayFeedbacks();
            Interactable = false;
            _onClick?.Invoke();
        }
    }
}
using System;
using FMODUnity;
using Game.Systems;
using Game.Systems.Audio;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Ui.Main
{
    public class PhysicalButton : MonoBehaviour
    {
        [SerializeField] private string _text;
        [SerializeField] private bool _deactivateOnClick = true;
        [SerializeField] private MMF_Player _selectFeedback;
        [SerializeField] private MMF_Player _deselectFeedback;
        [SerializeField] private MMF_Player _clickFeedback;
        [SerializeField] private UnityEvent _onClick;
        [SerializeField] private EventReference _clickAudio;
        
        private TextMeshProUGUI _textMeshpro;

        [ShowInInspector, ReadOnly] public bool Interactable { get; set; }
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
            other.GetComponentInChildren<PlayerInputDialog>()?.ShowMenuNav(true);
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!IsSelected || !Interactable)
                return;
            
            IsSelected = false;
            _deselectFeedback.PlayFeedbacks();
            other.GetComponentInChildren<PlayerInputDialog>()?.ShowMenuNav(false);
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

            AudioManager.PlayOneShot(_clickAudio);
            _clickFeedback.PlayFeedbacks();
            _deselectFeedback.PlayFeedbacks();
            Interactable = !_deactivateOnClick;
            _onClick?.Invoke();
        }
    }
}
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
     
        public bool Interactable { get; set; }
        public bool IsSelected { get; set; }

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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && IsSelected && Interactable)
            {
                _clickFeedback.PlayFeedbacks();
                _deselectFeedback.PlayFeedbacks();
                Interactable = false;
                _onClick?.Invoke();
            }
        }
    }
}
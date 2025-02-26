using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Game.Ui.Pause
{
    public class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private MMF_Player _normalFeedback;
        [SerializeField] private MMF_Player _hoverFeedback;
        [SerializeField] private MMF_Player _clickFeedback;
        [SerializeField] private UnityEvent _onClick;
        
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponentInParent<CanvasGroup>();
        }

        private bool IsInteractable()
        {
            return _canvasGroup == null || _canvasGroup.interactable;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!IsInteractable()) return;

            _hoverFeedback?.PlayFeedbacks();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _normalFeedback?.PlayFeedbacks();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!IsInteractable()) return;

            _clickFeedback?.PlayFeedbacks();
            _onClick?.Invoke();
        }
    }
}
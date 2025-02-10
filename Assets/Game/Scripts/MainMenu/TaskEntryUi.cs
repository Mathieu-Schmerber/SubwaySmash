using Game.Systems.Tutorial;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

namespace Game.MainMenu
{
    public class TaskEntryUi : MonoBehaviour
    {
        [SerializeField] private MMF_Player _validateFeedback;
        [SerializeField] private MMF_Player _unvalidateFeedback;

        private TextMeshProUGUI _text;
        private TutorialCondition _condition;
        private bool _previousState;

        private void Awake()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void Bind(TutorialCondition condition)
        {
            _text.text = condition.Label;
            _condition = condition;
            if (_condition != null)
                _condition.OnConditionChanged -= OnConditionChanged;
            _condition.OnConditionChanged += OnConditionChanged;
            OnConditionChanged(_condition.IsVerified);
            _previousState = _condition.IsVerified;
        }

        private void OnConditionChanged(bool status)
        {
            if (status == _previousState)
                return;
            
            _previousState = status;
            if (_previousState)
                _validateFeedback.PlayFeedbacks();
            else
                _unvalidateFeedback.PlayFeedbacks();
        }

        private void OnDisable()
        {
            if (_condition != null)
                _condition.OnConditionChanged -= OnConditionChanged;
        }
    }
}
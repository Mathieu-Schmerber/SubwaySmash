using Game.Systems.Ui;
using MoreMountains.Feedbacks;
using Pixelplacement;
using TMPro;
using UnityEngine;

namespace Game.Systems.Score.Ui
{
    public class ScoreUi : MonoBehaviour
    {
        [SerializeField] private ProgressBar _comboBar;
        [SerializeField] private ProgressBar _cooldownBar;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _comboText;
        private MMF_Player _scoreFeedback;
        private MMF_Player _comboModifierFeedback;
        private MMF_Player _comboBarFeedback;

        private bool _hidden = true;
        private float _displayedScore;

        private void Awake()
        {
            _scoreFeedback = _scoreText.GetComponent<MMF_Player>();
            _comboModifierFeedback = _comboText.GetComponent<MMF_Player>();
            _comboBarFeedback = _comboBar.GetComponent<MMF_Player>();
            _comboBar.GetComponent<CanvasGroup>().alpha = 0;
        }

        private void OnEnable()
        {
            Core.ScoreSystem.OnComboLevelUpdated += OnComboLevelChange;
            Core.ScoreSystem.OnScoreUpdated += OnScoreChange;
            Core.ScoreSystem.OnProgressUpdated += OnProgressChange;
            Core.ScoreSystem.OnComboTimerStarted += Show;
            Core.ScoreSystem.OnComboTimerOver += Hide;
        }

        private void OnProgressChange(float value, float min, float max)
        {
            var progress = value / max;
            if (progress == 0)
                _comboBar.SetProgress(0);
            else
            {
                Tween.Value(_comboBar.Progress, progress, _comboBar.SetProgress, 0.1f, 0,
                    Tween.EaseLinear);
            }
        }

        private void OnScoreChange(float value)
        {
            var countTo = _scoreFeedback.GetFeedbackOfType<MMF_TMPCountTo>();
            countTo.CountFrom = _displayedScore;
            countTo.CountTo = value;
            _scoreFeedback.PlayFeedbacks();
            _displayedScore = value;
        }

        private void OnComboLevelChange(int value)
        {
            var display = _comboModifierFeedback.GetFeedbackOfType<MMF_TMPText>();
            display.NewText = $"x{value}";
            _comboModifierFeedback.PlayFeedbacks();
            Tween.Value(1f, 0f, _cooldownBar.SetProgress, Core.ScoreSystem.ScoreData.ComboCooldown, 0,
                Tween.EaseLinear);
        }
        
        private void Hide()
        {
            if (_hidden)
                return;
            
            _hidden = true;
            _comboBarFeedback.PlayFeedbacks();
        }

        private void Show()
        {
            if (!_hidden)
                return;
            
            _hidden = false;
            _comboBarFeedback.PlayFeedbacks();
            Tween.Value(1f, 0f, _cooldownBar.SetProgress, Core.ScoreSystem.ScoreData.ComboCooldown, 0,
                Tween.EaseLinear);
        }

        private void OnDisable()
        {
            Core.ScoreSystem.OnComboLevelUpdated -= OnComboLevelChange;
            Core.ScoreSystem.OnScoreUpdated -= OnScoreChange;
            Core.ScoreSystem.OnProgressUpdated -= OnProgressChange;
            Core.ScoreSystem.OnComboTimerStarted += Show;
            Core.ScoreSystem.OnComboTimerOver += Hide;        
        }
    }
}
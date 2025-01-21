using System.Globalization;
using Game.Systems.Ui;
using Pixelplacement;
using TMPro;
using UnityEngine;

namespace Game.Systems.Score.Ui
{
    public class ScoreUi : MonoBehaviour
    {
        [SerializeField] private ProgressBar _comboBar;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _comboText;

        [Header("Combo Level Animation")] [SerializeField]
        private Animation _comboLevelAnimation;

        private void OnEnable()
        {
            Core.ScoreSystem.OnComboFinish += OnComboFinish;
            Core.ScoreSystem.OnComboLevelUpdated += OnComboLevelChange;
            Core.ScoreSystem.OnScoreUpdated += OnScoreChange;
            Core.ScoreSystem.OnProgressUpdated += OnProgressChange;
        }

        private void OnProgressChange(float value, float min, float max)
        {
            var progress = value / max;
            _comboBar.SetProgress(progress);
            Debug.Log($"progress: {value}/{max}");
            Tween.Value(progress, 0f, _comboBar.SetProgress, Core.ScoreSystem.ScoreData.ComboCooldown, 0,
                Tween.EaseLinear);
        }

        private void OnScoreChange(float value)
        {
            _scoreText.text = value.ToString(CultureInfo.CurrentCulture);
        }

        private void OnComboLevelChange(int value)
        {
            _comboText.text = $"x{value}";
        }

        private void OnComboFinish()
        {
            // Additional feedback for combo finish can be implemented here
        }

        private void OnDisable()
        {
            Core.ScoreSystem.OnComboFinish -= OnComboFinish;
            Core.ScoreSystem.OnComboLevelUpdated -= OnComboLevelChange;
            Core.ScoreSystem.OnScoreUpdated -= OnScoreChange;
            Core.ScoreSystem.OnProgressUpdated -= OnProgressChange;
        }
    }
}
using UnityEngine;

namespace Game.Systems.Score
{
    public class ScoreSystem : MonoBehaviour
    {
        [SerializeField] private ScoreData _scoreData;
        public ScoreData ScoreData => _scoreData;

        private float _comboTimer;
        private float _comboCooldown;

        private float _currentComboProgress;
        private int _currentComboLevel;

        private float _currentScore;
        private float _cumulativeComboScore;

        public float CurrentScore
        {
            get => _currentScore;
            private set
            {
                if (_currentScore != value)
                {
                    _currentScore = value;
                    OnScoreUpdated?.Invoke(_currentScore);
                }
            }
        }

        public float CurrentComboProgress
        {
            get => _currentComboProgress;
            private set
            {
                if (_currentComboProgress != value)
                {
                    _currentComboProgress = value;
                    OnProgressUpdated?.Invoke(_currentComboProgress, 0f, GetComboTarget(_currentComboLevel + 1));
                }
            }
        }

        public int CurrentComboLevel
        {
            get => _currentComboLevel;
            private set
            {
                if (_currentComboLevel != value)
                {
                    _currentComboLevel = value;
                    OnComboLevelUpdated?.Invoke(_currentComboLevel);
                }
            }
        }

        public event System.Action OnComboFinish;
        public event System.Action<int> OnComboLevelUpdated;
        public event System.Action<float> OnScoreUpdated;
        public event OnProgress OnProgressUpdated;

        public delegate void OnProgress(float value, float min, float max);

        private void Start()
        {
            ResetCombo();
            _comboCooldown = _scoreData.ComboCooldown;
        }

        private void Update()
        {
            if (_comboTimer > 0)
            {
                _comboTimer -= Time.deltaTime;

                if (_comboTimer <= 0)
                {
                    HandleComboTimeout();
                }
            }
        }

        private void HandleComboTimeout()
        {
            if (CurrentComboProgress >= GetComboTarget(_currentComboLevel + 1))
            {
                // Combo successful, increase the combo level
                IncreaseCombo();
            }
            else
            {
                // Combo failed, finalize score and reset combo
                FinalizeComboScore();
                ResetCombo();
            }
        }

        private void IncreaseCombo()
        {
            while (CurrentComboProgress >= GetComboTarget(_currentComboLevel + 1))
            {
                _cumulativeComboScore += GetComboTarget(_currentComboLevel + 1);
                CurrentComboProgress -= GetComboTarget(_currentComboLevel + 1);
                CurrentComboLevel++;
            }

            _comboTimer = _comboCooldown;
        }

        private void FinalizeComboScore()
        {
            CurrentScore += _cumulativeComboScore * CurrentComboLevel;
            _cumulativeComboScore = 0f;
            OnComboFinish?.Invoke();
        }

        private void ResetCombo()
        {
            CurrentComboProgress = 0f;
            CurrentComboLevel = 1;
            _cumulativeComboScore = 0f;
            _comboTimer = _comboCooldown;
        }

        public void AddProgress(float points)
        {
            CurrentComboProgress += points;

            if (CurrentComboProgress >= GetComboTarget(_currentComboLevel + 1))
            {
                // Handle combo progression
                IncreaseCombo();
            }
        }

        public float GetComboTarget(int comboLevel)
        {
            // Geometric progression: BaseComboProgress * (ComboProgressModifier)^(comboLevel - 1)
            return _scoreData.BaseComboProgress * Mathf.Pow(_scoreData.ComboProgressModifier, comboLevel - 1);
        }

        public void OnKill(string identifier)
        {
            if (!ScoreData.TryGet(identifier, out var entry) || !entry.OnKill)
                return;

            AddProgress(entry.KillScore.Points);
        }

        public void OnTrigger(string identifier)
        {
            if (!ScoreData.TryGet(identifier, out var entry) || !entry.OnTrigger)
                return;

            AddProgress(entry.TriggerScore.Points);
        }

        public void OnPush(string identifier)
        {
            if (!ScoreData.TryGet(identifier, out var entry) || !entry.OnPush)
                return;

            AddProgress(entry.PushScore.Points);
        }
    }
}
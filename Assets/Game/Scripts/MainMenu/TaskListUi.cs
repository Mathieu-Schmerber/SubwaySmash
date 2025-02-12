using System.Linq;
using Game.Systems.Tutorial;
using UnityEngine;

namespace Game.MainMenu
{
    public class TaskListUi : MonoBehaviour
    {
        [SerializeField] private GameObject _entryPrefab;

        private TutorialCondition[] _conditions;
        
        private void Awake()
        {
            _conditions = FindObjectsByType<TutorialCondition>(FindObjectsSortMode.None);
        }

        private void OnEnable()
        {
            foreach (var condition in _conditions)
                condition.OnConditionChanged += OnConditionChanged;
        }

        private void OnConditionChanged(bool obj)
        {
            if (_conditions.Any(condition => !condition.IsVerified))
            {
                Core.LevelClearCondition.MarkLevelNotCleared();
                return;
            }

            Core.LevelClearCondition.MarkLevelCleared();
        }

        private void OnDisable()
        {
            foreach (var condition in _conditions)
                condition.OnConditionChanged -= OnConditionChanged;
        }

        private void Start()
        {
            foreach (var condition in _conditions.OrderBy(x => x.Order))
            {
                var entry = Instantiate(_entryPrefab, transform);
                var task = entry.GetComponent<TaskEntryUi>();
                task.Bind(condition);
            }
        }
    }
}
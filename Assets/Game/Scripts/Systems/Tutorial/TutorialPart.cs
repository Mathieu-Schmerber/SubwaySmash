using System;
using UnityEngine;

namespace Game.Systems.Tutorial
{
    [Serializable]
    public class TutorialPart
    {
        public TutorialCondition[] Conditions;
        public event Action OnCompleted;

        public void Initialize()
        {
            foreach (var condition in Conditions)
            {
                condition.OnConditionChanged += OnConditionChanged;
            }
        }
        
        public void Exit()
        {
            foreach (var condition in Conditions)
            {
                condition.OnConditionChanged -= OnConditionChanged;
            }
        }
        
        private void OnConditionChanged(bool status)
        {
            foreach (var condition in Conditions)
            {
                if (!condition.IsVerified)
                    return;
            }
            OnCompleted?.Invoke();
        }
    }
}
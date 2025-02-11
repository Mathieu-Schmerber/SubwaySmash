using System;
using UnityEngine;

namespace Game.Systems.Tutorial
{
    
    public abstract class TutorialCondition : MonoBehaviour
    {
        [field: SerializeField] public int Order { get; set; }
        [field: SerializeField] public string Label { get; set; }
        
        public event Action<bool> OnConditionChanged;  
        public bool IsVerified { get; private set; }

        protected void Verify()
        {
            IsVerified = true;
            OnConditionChanged?.Invoke(IsVerified);
        }

        protected void UnVerify()
        {
            IsVerified = false;
            OnConditionChanged?.Invoke(IsVerified);
        }
    }
}
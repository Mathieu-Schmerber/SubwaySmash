using System;
using UnityEngine;

namespace Game.Systems.Tutorial
{
    
    public abstract class TutorialCondition : MonoBehaviour
    {
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
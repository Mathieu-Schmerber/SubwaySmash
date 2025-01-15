using System;
using System.Collections;
using JetBrains.Annotations;
using LemonInc.Core.Utilities;
using UnityEngine;

namespace Game.Entities.Player.Abilities
{
    public abstract class AbilityBase : MonoBehaviour
    {
        private bool _locked = false;
        private readonly Timer _timer = new();

        public void SetCooldown(float cooldown)
        {
            _timer.Start(cooldown, false);
        }
        
        public void SetLocked(bool locked) => _locked = locked;

        public bool IsReady() => !_locked && _timer.IsOver();
        
        public void Perform(Action performed)
        {
            if (_locked)
            {
                performed?.Invoke();
                return;
            }

            _timer.Restart();
            StartCoroutine(nameof(OnPerform), performed);
        }
        
        protected abstract IEnumerator OnPerform(Action performed);
    }
}
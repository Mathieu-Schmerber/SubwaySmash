using System;
using System.Collections;
using JetBrains.Annotations;
using LemonInc.Core.Utilities;
using UnityEngine;

namespace Game.Entities.Player.Abilities
{
    public abstract class AbilityBase : MonoBehaviour
    {
        private readonly Timer _timer = new();

        public void SetCooldown(float cooldown)
        {
            _timer.Start(cooldown, false);
        }

        public bool IsReady() => _timer.IsOver();
        
        public void Perform(Action performed)
        {
            _timer.Restart();
            StartCoroutine(nameof(OnPerform), performed);
        }
        
        protected abstract IEnumerator OnPerform(Action performed);
    }
}
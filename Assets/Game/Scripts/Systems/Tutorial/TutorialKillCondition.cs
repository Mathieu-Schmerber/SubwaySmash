using System;
using Game.Entities;

namespace Game.Systems.Tutorial
{
    public class TutorialKillCondition : TutorialCondition
    {
        private IKillable _killable;

        private void Awake()
        {
            _killable = GetComponent<IKillable>();
        }

        private void OnEnable()
        {
            _killable.OnDeath += Verify;
        }
        
        private void OnDisable()
        {
            _killable.OnDeath -= Verify;
        }
    }
}
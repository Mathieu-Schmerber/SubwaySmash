using System;
using System.Collections;
using Databases;
using Game.Entities;
using Game.Systems.Push;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Systems.Score
{
    public class ScoreEntryIdentifier : MonoBehaviour
    {
        [ValueDropdown(nameof(GetScoreEntries))]
        [SerializeField] private string _identifier;

        private PushTriggerBase _triggerable;
        private Pushable _pushable;
        private IKillable _killable;

        private void Awake()
        {
            _pushable = GetComponent<Pushable>();
            _triggerable = GetComponent<PushTriggerBase>();
            _killable = GetComponent<IKillable>();
        }

        private void OnEnable()
        {
            if (_pushable)
                _pushable.OnPushed += OnPush;
            if (_triggerable)
                _triggerable.OnTrigger += OnTrigger;
            if (_killable != null)
                _killable.OnDeath += OnDeath;
        }

        private void OnDisable()
        {
            if (_pushable)
                _pushable.OnPushed -= OnPush;
            if (_triggerable)
                _triggerable.OnTrigger -= OnTrigger;
            if (_killable != null)
                _killable.OnDeath -= OnDeath;
        }
        
        private void OnDeath()
        {
            Core.ScoreSystem.OnDeath(_identifier, transform.position);
        }

        public void OnTrigger()
        {
            Core.ScoreSystem.OnTrigger(_identifier, transform.position);
        }

        private void OnPush()
        {
            Core.ScoreSystem.OnPush(_identifier, transform.position);
        }

#if UNITY_EDITOR
        private IEnumerable GetScoreEntries()
        {
            var data = RuntimeDatabase.Data.Score;
            return data.GetKeys();
        }
#endif
    }
}
using System;
using Game.Systems.Push;
using Unity.VisualScripting;

namespace Game.Systems.Tutorial
{
    public class TutorialTriggerCondition : TutorialCondition
    {
        private PushTriggerBase _triggerable;
        private void Awake()
        {
            _triggerable = GetComponent<PushTriggerBase>();
        }

        private void OnEnable()
        {
            _triggerable.OnTrigger += Verify;
        }

        private void OnDisable()
        {
            _triggerable.OnTrigger -= Verify;
        }
    }
}
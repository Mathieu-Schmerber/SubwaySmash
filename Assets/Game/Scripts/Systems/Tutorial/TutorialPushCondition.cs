using System;
using Game.Systems.Push;
using Unity.VisualScripting;

namespace Game.Systems.Tutorial
{
    public class TutorialPushCondition : TutorialCondition
    {
        private Pushable _pushable;
        private void Awake()
        {
            _pushable = GetComponent<Pushable>();
        }

        private void OnEnable()
        {
            _pushable.OnPushed += Verify;
        }

        private void OnDisable()
        {
            _pushable.OnPushed -= Verify;
        }
    }
}
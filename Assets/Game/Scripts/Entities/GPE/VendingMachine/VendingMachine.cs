using System.Collections.Generic;
using Game.Systems.Push;
using LemonInc.Core.Pooling;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Game.Entities.GPE.VendingMachine
{
    public class VendingMachine : PushTriggerBase
    {
        private MMF_Player _feedback;
        private bool _triggered;
        private HashSet<IKillable> _killed = new();
        
        private void Awake()
        {
            _feedback = GetComponent<MMF_Player>();
        }

        public override void Trigger(Pushable actor)
        {
            if (_triggered) return;
            _triggered = true;
            _feedback.PlayFeedbacks();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_triggered) return;
            var killable = other.GetComponent<IKillable>();
            if (killable != null && _killed.Add(killable))
            {
                Core.Pooling.From(Pool.FX_Electrified).Get(null, other.transform.position, Quaternion.identity);
                killable.Kill(Vector3.up, 0);
            }
        }
    }
}
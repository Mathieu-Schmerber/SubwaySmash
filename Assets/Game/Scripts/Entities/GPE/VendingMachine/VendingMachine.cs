using System.Collections.Generic;
using Game.Systems;
using Game.Systems.Audio;
using Game.Systems.Kill;
using Game.Systems.Push;
using Game.Utilities.Effects;
using LemonInc.Core.Pooling;
using MoreMountains.Feedbacks;
using UnityEngine;
using EventReference = FMODUnity.EventReference;

namespace Game.Entities.GPE.VendingMachine
{
    public class VendingMachine : PushTriggerBase
    {
        [SerializeField] private EventReference _killAudio;
        
        private MMF_Player _feedback;
        private bool _triggered;
        private HashSet<IKillable> _killed = new();
        private Glow _glow;

        private void Awake()
        {
            _feedback = GetComponent<MMF_Player>();
            _glow = GetComponent<Glow>();
        }

        public override void Trigger(Transform actor)
        {
            if (_triggered) return;
            _triggered = true;
            _feedback.PlayFeedbacks();
            _glow.SetDefault();
            Destroy(_glow);
        }

        private void OnTriggerEnter(Collider other)
        {
            Zaap(other);
        }

        private void Zaap(Collider other)
        {
            if (!_triggered) return;
            var killable = other.GetComponent<IKillable>();
            if (killable != null && _killed.Add(killable))
            {
                AudioManager.PlayOneShot(_killAudio);
                Core.Pooling.From(Pool.FX_Electrified).Get(null, other.transform.position, Quaternion.identity);
                killable.Kill(Vector3.up, 0);
            }
        }
        
        private void OnTriggerStay(Collider other)
        { 
            Zaap(other);
        }
    }
}
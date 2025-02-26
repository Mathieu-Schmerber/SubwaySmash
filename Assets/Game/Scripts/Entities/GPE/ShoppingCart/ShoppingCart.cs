using Game.Systems.Kill;
using Game.Systems.Push;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Game.Entities.GPE.ShoppingCart
{
    public class ShoppingCart : PushTriggerBase
    {
        private MMF_Player _feedback;
        [SerializeField] private float _killStrength;
        
        private void Awake()
        {
            _feedback = GetComponent<MMF_Player>();
        }
        
        public override void Trigger(Transform actor)
        {
            var killable = actor.GetComponent<IKillable>();
            if (killable == null) return;
            var dir = (actor.transform.position - transform.position).normalized;
            killable.Kill(Vector3.up,_killStrength);
            _feedback.PlayFeedbacks();
        }
    }
}

using Game.Systems.Push;
using UnityEngine;
using MoreMountains.Feedbacks;

namespace Game.Entities.GPE
{
    public class ShoppingCart : PushTriggerBase
    {
        private MMF_Player _feedback;
        private void Awake()
        {
            _feedback = GetComponent<MMF_Player>();
        }
        
        
        public override void Trigger(Pushable actor)
        {
            var killable = actor.GetComponent<IKillable>();
            if (killable == null) return;
            var dir = (actor.transform.position - transform.position).normalized;
            killable.Kill(Vector3.up,50);
            _feedback.PlayFeedbacks();
        }
    }
}

using Game.Systems.Push;
using UnityEngine;

namespace Game.Entities.GPE.Train
{
    public class Train : PushTriggerBase
    {
        public override void Trigger(Pushable actor)
        {
            Debug.LogWarning(actor);
            var killable = actor.GetComponent<IKillable>();
            if (killable == null) return;
            var dir = (actor.transform.position - transform.position).normalized;
            killable.Kill(Vector3.up,500);
        }
    }
}
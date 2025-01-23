using Game.Entities.Ai;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Systems.Waypoint
{
    public class Exit : MonoBehaviour
    {
        [SerializeField] private float _radius = .2f;
        [SerializeField] private bool _debug = true;
        [SerializeField, ShowIf(nameof(_debug))] private Color _debugColor = new(0.8f, 0.4f, 0f, 1);

        private void Update() => CheckForAiBrain();

        private void CheckForAiBrain()
        {
            var hits = Physics.OverlapSphere(transform.position, _radius);

            foreach (var hit in hits)
            {
                var aiBrain = hit.GetComponent<AiBrain>();
                if (aiBrain != null && TargetIsThis(aiBrain.Target))
                {
                    Destroy(aiBrain.gameObject);
                }
            }
        }

        private bool TargetIsThis(Transform aiBrainTarget)
            => Vector3.Distance(transform.position, aiBrainTarget.position) <= _radius;

        private void OnDrawGizmos()
        {
            Gizmos.color = _debugColor;
            Gizmos.DrawSphere(transform.position, _radius);
        }
    }
}
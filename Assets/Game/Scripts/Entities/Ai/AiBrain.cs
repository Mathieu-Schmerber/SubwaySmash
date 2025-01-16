using Game.Entities.Player;
using Game.Inputs;
using LemonInc.Core.Input;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Entities.Ai
{
    public class AiBrain : MonoBehaviour, IInputProvider
    {
        private NavMeshAgent _agent;
        private Transform _target;
        private NavMeshPath _path;

        public Vector3 MovementDirection
        {
            get
            {
                if (_target == null)
                    return Vector3.zero;

                _agent.CalculatePath(_target.position, _path);
                
                if (_path.corners.Length <= 1) return Vector3.zero;
                var direction = (_path.corners[1] - transform.position).normalized;
                return direction;
            }
        }

        public Vector3 AimDirection => MovementDirection.normalized;
        public InputState Dash { get; }
        public InputState Push { get; }

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _path = new NavMeshPath();
        }

        public void SetTarget(Transform target) => _target = target;
    }
}
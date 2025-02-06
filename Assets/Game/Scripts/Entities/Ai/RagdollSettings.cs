using UnityEngine;

namespace Game.Entities.Ai
{
    public class RagdollSettings : MonoBehaviour
    {
        [SerializeField] private bool _syncTransformRecursively;
        [SerializeField] private Rigidbody _mainRigidBody;

        public Rigidbody MainRigidbody => _mainRigidBody;
        public bool SyncTransformRecursively => _syncTransformRecursively;
    }
}
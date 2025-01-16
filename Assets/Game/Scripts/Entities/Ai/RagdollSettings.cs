using UnityEngine;

namespace Game.Entities.Ai
{
    public class RagdollSettings : MonoBehaviour
    {
        [SerializeField] private Rigidbody _mainRigidBody;

        public Rigidbody MainRigidbody => _mainRigidBody;
    }
}
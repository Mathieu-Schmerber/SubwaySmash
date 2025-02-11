using UnityEngine;

namespace Game.Entities.GPE
{
    public class OnTriggerEmitter : MonoBehaviour
    {
        private BoxedArea _area;

        private void Awake()
        {
            _area = GetComponentInParent<BoxedArea>();
        }

        private void OnTriggerEnter(Collider other) => _area.OnTriggerEnter(other);
        private void OnTriggerExit(Collider other) => _area.OnTriggerExit(other);
    }
}
using UnityEngine;

namespace Game.Utilities
{
    [RequireComponent(typeof(BoxCollider))]
    public class ContainedArea : MonoBehaviour
    {
        private BoxCollider _boxCollider;

        private void Start()
        {
            _boxCollider = GetComponent<BoxCollider>();
        }

        private void OnTriggerExit(Collider other)
        {
            Vector3 localPosition = transform.InverseTransformPoint(other.transform.position);
            Bounds localBounds = new Bounds(Vector3.zero, _boxCollider.size);

            if (localPosition.x > localBounds.max.x)
                localPosition.x = localBounds.min.x;
            else if (localPosition.x < localBounds.min.x)
                localPosition.x = localBounds.max.x;
            else if (localPosition.z > localBounds.max.z)
                localPosition.z = localBounds.min.z;
            else if (localPosition.z < localBounds.min.z)
                localPosition.z = localBounds.max.z;

            other.transform.position = transform.TransformPoint(localPosition);
        }
    }
}
using UnityEngine;

public class LockChildRotation : MonoBehaviour
{
    private Quaternion initialRotation;

    private void Start()
    {
        // Store the initial local rotation
        initialRotation = transform.localRotation;
    }

    private void LateUpdate()
    {
        // Lock X and Z rotation while allowing Y rotation to follow the parent
        Vector3 currentEulerAngles = transform.localRotation.eulerAngles;
        transform.localRotation = Quaternion.Euler(initialRotation.eulerAngles.x, currentEulerAngles.y, initialRotation.eulerAngles.z);
    }
}
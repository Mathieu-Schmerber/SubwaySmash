using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    enum Direction { North, East, South, West };
    public float TrainSpeed = 1.0f;
    [SerializeField]Direction MyDirection = Direction.North;
    public Rigidbody rb;
    Vector3 moveDir;

    // Update is called once per frame
    private void Start()
    {
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        rb.freezeRotation = true;

        switch (MyDirection)
        {
            case Direction.North:
                moveDir = new Vector3(1, 0, 0);
                rb.constraints |= RigidbodyConstraints.FreezePositionZ;
                break;
            case Direction.East:
                moveDir = new Vector3(0, 0, -1);
                rb.constraints |= RigidbodyConstraints.FreezePositionX;
                break;
            case Direction.South:
                moveDir = new Vector3(-1, 0, 0);
                rb.constraints |= RigidbodyConstraints.FreezePositionZ;
                break;
            case Direction.West:
                moveDir = new Vector3(0, 0, 1);
                rb.constraints |= RigidbodyConstraints.FreezePositionX;
                break;
            default:
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        MoveTrain(TrainSpeed);
    }

    void MoveTrain(float Speed)
    {
        
        rb.AddForce(moveDir * Speed, ForceMode.Impulse);
    }
}

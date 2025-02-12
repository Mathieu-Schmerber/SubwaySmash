using UnityEngine;

public class CartGravity : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] private float _threshold;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var magnitude = _rb.linearVelocity.magnitude;
        Debug.Log(magnitude);
        if (magnitude < _threshold)
        {
            _rb.useGravity = true;
        }
        else
        {
            _rb.useGravity = false;
        }
    }
}

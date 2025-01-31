using UnityEngine;
using LemonInc.Core.Utilities;

public class moveArrowUI : MonoBehaviour
{
    bool _isDown = true;
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _revSpeed = .5f;
    private readonly Timer _timer = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _timer.Start(_revSpeed, true, Flip);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDown)
            transform.position += Vector3.down * Time.deltaTime *_speed;
        else
        {
            transform.position += Vector3.up *Time.deltaTime *_speed;
        }
    }

    void Flip()
    {
        _isDown = !_isDown;
    }
}

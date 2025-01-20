using System;
using Game.Systems.Push;
using UnityEngine;

public class Pole : PushTriggerBase
{
    private bool _hasFallen = false;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void Trigger(Pushable actor)
    {
        if (_hasFallen) return;
        _hasFallen = true;
        
    }

    private void Fall(Transform actor)
    { 
        var dir = transform.position - actor.position;
        //rb.AddForce(dir*1000, ForceMode.Impulse);
        rb.AddForce(dir*10,ForceMode.Impulse);
        Debug.Log("pef");
    }
}

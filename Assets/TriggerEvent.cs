using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    public float TrainSpeed = 1.0f;
    public Rigidbody rb;
    bool isTriggered;

    // Update is called once per frame
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "NPC")
        {
            isTriggered = true;
        }
        
    }
    private void FixedUpdate()
    {
        if (isTriggered)
        {
            rb.linearVelocity = transform.forward * TrainSpeed * Time.fixedDeltaTime;
            Debug.Log("pourt");
            
        }
        
    }

   
}

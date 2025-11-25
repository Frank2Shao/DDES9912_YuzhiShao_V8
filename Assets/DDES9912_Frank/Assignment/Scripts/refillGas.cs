using System.Collections.Generic;
using UnityEngine;

public class refillGas:MonoBehaviour
{
    [Tooltip("The place where things go....")]
    public Transform destination;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;

        Vector3 savedVelocity = rb.linearVelocity;
        
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.position = destination.position;
        rb.linearVelocity = savedVelocity;
        rb.WakeUp();
    }
}

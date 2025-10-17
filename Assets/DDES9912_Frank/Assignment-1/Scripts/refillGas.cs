using System.Collections.Generic;
using UnityEngine;

public class refillGas:MonoBehaviour
{
    [Tooltip("The place where things go....")]
    public Transform destination;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        rb.useGravity = true;
        rb.transform.position = destination.position;
    }
}

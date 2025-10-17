using UnityEngine;
using System.Collections.Generic;

public class UseGas : MonoBehaviour
{

    [Tooltip("The cylinder where balls(gas) go....")]
    public Transform[] destinations;
    [Tooltip("time between teleports")]
    public float interval;
    private int nextDestination = 0;
    private bool teleportEnabled = false;
    private Rigidbody subject;

    
    public void StartEngine()
    {
        if (teleportEnabled) return;
        teleportEnabled = true;
        InvokeRepeating(nameof(Teleport), 0f, interval); // start loop
    }

    private void OnTriggerStay(Collider other)
    {
        if (!teleportEnabled) return;
        if (subject == null)
            subject = other.GetComponent<Rigidbody>();
    }

    private void Teleport()
    {
        if (!teleportEnabled) return;
        if (destinations.Length == 0 || subject == null) return;

        subject.useGravity = false;//this way small balls wou't drop down after teleport, it will hover in the cylinder,simulate the gasoline inside

        subject.transform.position = destinations[nextDestination].position;

        if (nextDestination != destinations.Length-1)
            nextDestination += 1;
        else nextDestination = 0 ; //ctrl the loop

        subject = null;
    }

    public void StopEngine()
    {
        teleportEnabled = false;
        CancelInvoke(nameof(Teleport)); // stop loop
        subject = null; // clear current target
    }


}

//EZPZ Interaction Toolkit
//by Matt Cabanag
//Created - don't remember when, some time in 2022-2923
//Modified by Frank on 27/11/2025

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class NMAWalkTowards : MonoBehaviour
{

    [Header("Navigation")]
    public Transform destination;
    public UnityEvent onArrive;
    public UnityEvent onFirstMove;
    public Transform[] altDestinations;

    
    public int currentAltIndex = 0;//added by Frank


    [Header("Graphics")]
    public Animator avatarAnimator;
    public string speedString = "speed";

    [Header("System Stuff (usually dont touch")]
    public NavMeshAgent myNma;
    public bool arrivalFlag = false;
    public bool prevArrivalFlag = false;
    public float dist2PrevPos;
    public Vector3 prevPos;
    public float fixedUpdateSpeed;
    public Vector3 fixedUpdateVelocity;
    public Vector3 prevPosFixedUpdate;

    // Start is called before the first frame update
    void Start()
    {
        if (myNma == null)
            myNma = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        dist2PrevPos = Vector3.Distance(transform.position, prevPos);

        if (dist2PrevPos > 0)
        {
            arrivalFlag = false;

            if (prevArrivalFlag != arrivalFlag)
            {
                onFirstMove.Invoke();
            }
        }
        else
        {
            arrivalFlag = true;

            if (prevArrivalFlag != arrivalFlag)
            {
                onArrive.Invoke();
            }
        }

        if (destination != null)
        {
            if(destination.gameObject.activeSelf)
                myNma.SetDestination(destination.position);
            else                
                myNma.SetDestination(transform.position);
        }

        HandleAvatar();

        prevArrivalFlag = arrivalFlag;
        prevPos = transform.position;
    }

    public void FixedUpdate()
    {
        fixedUpdateVelocity = myNma.velocity;
        fixedUpdateSpeed = myNma.velocity.magnitude;
        prevPosFixedUpdate = transform.position;
    }

    public void SetDestination(Transform newDest)
    {
        destination = newDest;
    }

    public void SetDestination(int i)
    {
        Debug.Log("Set Destination! " + i);
        destination = altDestinations[i];
    }

    [ContextMenu("Random Destination")]
    public void SetDestinationRandom()
    {
        int randomIndex = Random.Range(0, altDestinations.Length);

        while (altDestinations[randomIndex] == destination)
            randomIndex = Random.Range(0, altDestinations.Length);

        SetDestination(randomIndex);
    }

//added by Frank
    public void SwitchDestination()
{
    if (altDestinations == null || altDestinations.Length == 0) return;

    // switch to the next destination in the array
    currentAltIndex = (currentAltIndex + 1) % altDestinations.Length;

    destination = altDestinations[currentAltIndex];
    myNma.SetDestination(destination.position);
}

// use this to call SwitchDestination after a delay
public void SwitchDestinationAfterDelay(float delay)
{
    StartCoroutine(SwitchAfterDelay(delay));
}

private IEnumerator SwitchAfterDelay(float delay)
{
    // wait for the specified delay
    yield return new WaitForSeconds(delay);

    // then switch destination
    SwitchDestination();
}

//end of addition by Frank

    public void HandleAvatar()
    {
        if (myNma != null)
        {   
            if (avatarAnimator != null)
            {
                avatarAnimator.SetFloat(speedString, fixedUpdateSpeed);
            }
        }
    }
}

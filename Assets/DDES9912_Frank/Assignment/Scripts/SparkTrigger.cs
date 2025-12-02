using UnityEngine;

public class SparkTrigger : MonoBehaviour
{
    [Header("Engine parts")]
    public Rigidbody pistonRb;              // rigidbody of the piston to be pushed down
    public float downwardImpulse = 50f;     // force magnitude

    [Header("Effects")]
    public ParticleSystem explosionFx;      // explosion effect

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[SparkTrigger] {name} hit by {other.name}, fx assigned? {explosionFx != null}");
        //if (!other.CompareTag("Ball")) return;

        /* 1. provide downward impulse to the piston
        if (pistonRb != null)
        {
            //pistonRb.AddForce(Vector3.down * downwardImpulse, ForceMode.Impulse);
            pistonRb.useGravity = true;
            Debug.Log("Spark Trigger Piston Downward Impulse");
        }*/

        // 2. play explosion effect
        if (explosionFx != null)
        {
            explosionFx.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            explosionFx.Play();
            Debug.Log("Spark Triggered Explosion Effect");
        }
    }
}

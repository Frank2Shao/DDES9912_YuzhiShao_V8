using UnityEngine;

public class movePiston : MonoBehaviour
{
    private Rigidbody Piston;

    void Awake() { Piston = GetComponent<Rigidbody>(); }

    void FixedUpdate()
    {
        float vy = Piston.linearVelocity.y;
            if (vy < 0.01f){Piston.useGravity = true;}
            else if (vy >= 0.01f){Piston.useGravity = false;}
    }

    /*public Vector3 startPPosition;
    public Vector3 sin;
    public float alpha2, sinValue2, rangeFactor2, PistonSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        sinValue2 = Mathf.Sin(alpha2 * Mathf.Deg2Rad);

        sin.y = sinValue2 * rangeFactor2;

        transform.localPosition = startPPosition + sin;

        alpha2 += PistonSpeed * Time.deltaTime;
    }*/
}
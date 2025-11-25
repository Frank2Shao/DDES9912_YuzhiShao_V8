using UnityEngine;

public class ColourBlink : MonoBehaviour
{
    [Header("Animation time settings")]
    public float blinkClock, clockSpeed, maxClock;
    public AnimationCurve blinkCurve;

    [Header("Colour settings")]
    public Color currentColor, startColor, warningColor, endColor;

    [Header("System settings")]
    public float lerpValue;
    public Material myMaterial;

    [Header("Control settings")]
    public bool blinkEnabled = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myMaterial = GetComponent<Renderer>().material;
        currentColor = myMaterial.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (!blinkEnabled)
            return;

        if (blinkClock > maxClock)
            blinkClock = 0f;

        lerpValue = blinkCurve.Evaluate(blinkClock);
        currentColor = Color.Lerp(startColor, endColor, lerpValue);

        myMaterial.SetColor("EmissionColor", currentColor);
        myMaterial.color = currentColor;

        blinkClock +=Time.deltaTime * clockSpeed;
    }

    public void EnableBlink()
    {
        blinkEnabled = true;
    }

    public void SetReadyColor()
    {
        Debug.Log("[ColourBlink] SetReadyColor called on " + gameObject.name);

        blinkEnabled = false;

        currentColor = endColor;

        if (myMaterial == null)
            myMaterial = GetComponent<Renderer>().material;

        myMaterial.SetColor("_EmissionColor", currentColor);
        myMaterial.color = currentColor;

        Debug.Log("[ColourBlink] Color set to " + currentColor + " on " + gameObject.name);
    }

    public void SetWarningColor()
    {
        Debug.Log("[ColourBlink] SetWarningColor called on " + gameObject.name);

        blinkEnabled = false;

        currentColor = warningColor;

        if (myMaterial == null)
            myMaterial = GetComponent<Renderer>().material;

        myMaterial.SetColor("EmissionColor", currentColor);
        myMaterial.color = currentColor;

        Debug.Log("[ColourBlink] Warning color set to " + currentColor + " on " + gameObject.name);
    }
}

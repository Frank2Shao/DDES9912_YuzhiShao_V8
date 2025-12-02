using UnityEngine;

public class CombineNewParts : MonoBehaviour
{
    [Header("Indicators")]
    public ColourBlink indicator1,indicator2;
    [Header("Indicator Sounds")]
    public AnimationSound light1, light2;     

    [Header("Next Stage Animation")]
    public ConveyorController nextStage;   // next stage conveyor controller

    //check both indicators' colors and play next stage animation if both are ready
    public void CheckandPlay()
    {
        if (indicator1 == null || indicator2 == null || nextStage == null)
            return;

        if (IsReady(indicator1) && IsReady(indicator2))
        {
            nextStage.PlayAnimation();
            ResetIndicator(indicator1);
            ResetIndicator(indicator2);
        }
        else if (!IsReady(indicator1) && IsReady(indicator2))
        {
            indicator1.SetWarningColor();//if needed, uncomment this line: indicator3.SetReadyColor();
            light1.playSound(0);
        }
        else if (!IsReady(indicator2) && IsReady(indicator1))
        {
            indicator2.SetWarningColor();
            light2.playSound(0);
        }
        else
        {
            indicator1.SetWarningColor();
            indicator2.SetWarningColor();
            light1.playSound(0);
            light2.playSound(0);
        }
    }

    // check if indicator is at ready color
    private bool IsReady(ColourBlink indicator)
    {
        return indicator.currentColor == indicator.endColor;
    }

    // reset indicator to start color
    private void ResetIndicator(ColourBlink indicator)
    {
        if (indicator == null) return;

        // turn off blinking
        indicator.blinkEnabled = false;

        // reset color
        indicator.currentColor = indicator.startColor;

        // update material color
        if (indicator.myMaterial == null)
        {
            var rend = indicator.GetComponent<Renderer>();
            if (rend != null)
                indicator.myMaterial = rend.material;
        }

        if (indicator.myMaterial != null)
        {
            indicator.myMaterial.color = indicator.startColor;
            indicator.myMaterial.SetColor("_EmissionColor", indicator.startColor);
        }
    }
}

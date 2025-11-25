using UnityEngine;

public class CombineNewParts : MonoBehaviour
{
    [Header("Indicators")]
    public ColourBlink indicator1,indicator2,indicator3;     

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
        else if (!IsReady(indicator1))
        {
            indicator1.SetWarningColor();//if needed, uncomment this line: indicator3.SetReadyColor();
        }
        else if (!IsReady(indicator2))
        {
            indicator2.SetWarningColor();
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

        // 关闭闪烁（如果你还想让它闪可以改逻辑）
        indicator.blinkEnabled = false;

        // 还原到 startColor
        indicator.currentColor = indicator.startColor;

        // 用 ColourBlink 里缓存的材质，如果没有就自己取一次
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

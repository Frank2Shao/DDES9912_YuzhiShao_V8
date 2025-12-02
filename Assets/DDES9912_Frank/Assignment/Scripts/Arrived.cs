using UnityEngine;

public class Arrived : MonoBehaviour
{
    public ColourBlink indicator;   // 
    public int num;

    //this function is called when the part arrives at this stage
    public void OnArrived()
    {
        if (indicator != null)
        {
            indicator.SetReadyColor();   // set color to red
        }
    }

    public int counter()
    {
        num += 1;
        return num;
    }
}

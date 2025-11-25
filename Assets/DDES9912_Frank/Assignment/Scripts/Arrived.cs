using UnityEngine;

public class Arrived : MonoBehaviour
{
    public ColourBlink indicator;   // 指向“指示灯 1”的 ColourBlink

    // 这个函数会被动画事件调用
    public void OnArrived()
    {
        if (indicator != null)
        {
            indicator.SetReadyColor();   // 直接把灯设成“就绪”颜色
        }
    }
}

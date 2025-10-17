using UnityEngine;

public class GearPositionController : MonoBehaviour
{
    /*public Rigidbody forwardRoot, backwardRoot;
    public Transform commonParent;        
    
    float neutralZF, neutralZB;
    public float neutralZ, driveOffset = 0.05f, reverseOffset = -0.05f;

    void Start()
    {
        neutralZF = LocalZ(forwardRoot.transform);
        neutralZB = LocalZ(backwardRoot.transform);
    }

    public void Nutural()
    {
        SetLocalZ(forwardRoot, neutralZF);
        SetLocalZ(backwardRoot, neutralZB);
    }
    public void GoForward()
    {
        SetLocalZ(forwardRoot, neutralZF + driveOffset);
        SetLocalZ(backwardRoot, neutralZB);
    }

    public void GoBackward()
    {
        SetLocalZ(forwardRoot, neutralZF);
        SetLocalZ(backwardRoot, neutralZB + reverseOffset);
    }

    float LocalZ(Transform t)
    {
        Transform p = commonParent ? commonParent : t.parent;
        Vector3 lp = p ? p.InverseTransformPoint(t.position) : t.position;
        return lp.z;
    }

    void SetLocalZ(Rigidbody rb, float localZ)
    {
        if (!rb) return;

        Transform t = rb.transform;
        Transform p = commonParent ? commonParent : t.parent;

        Vector3 lp = p ? p.InverseTransformPoint(t.position) : t.position;
        lp.z = localZ;
        Vector3 wp = p ? p.TransformPoint(lp) : lp;

        rb.position = wp;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Physics.SyncTransforms();
    }*/

    public Transform forwardCogRoot;   // 从动1（有/无刚体都可）
    public Transform backwardCogRoot;  // 从动2
    public Transform commonParent;     // 参照父级：SP&CSP&p

    public float driveOffset   =  0.05f;  // D = neutral + 0.05
    public float reverseOffset = -0.05f;  // R = neutral - 0.05

    float neutralZF, neutralZB;

    void Start()
    {
        if (!commonParent) Debug.LogError("commonParent");
        if (!forwardCogRoot || !backwardCogRoot) Debug.LogError("请拖入两个 cogRoot");

        // 记录当前各自的“相对Z”作为空挡
        neutralZF = ToLocalZ(forwardCogRoot);
        neutralZB = ToLocalZ(backwardCogRoot);
    }

    // --- 三个按钮直接绑这三个 ---
    public void GoNeutral()
    {
        SetLocalZ(forwardCogRoot,  neutralZF);
        SetLocalZ(backwardCogRoot, neutralZB);
    }

    public void GoDrive()
    {
        SetLocalZ(forwardCogRoot,  neutralZF + driveOffset);
        SetLocalZ(backwardCogRoot, neutralZB);
    }

    public void GoReverse()
    {
        SetLocalZ(forwardCogRoot,  neutralZF);
        SetLocalZ(backwardCogRoot, neutralZB + reverseOffset);
    }

    // 读取/设置“相对父级”的本地Z
    float ToLocalZ(Transform t)
    {
        var lp = commonParent ? commonParent.InverseTransformPoint(t.position) : t.localPosition;
        return lp.z;
    }

    void SetLocalZ(Transform t, float targetLocalZ)
    {
        if (!t) return;

        // 1) 以父为参照改本地Z（只改Z，保留本地X/Y）
        Vector3 lp = commonParent ? commonParent.InverseTransformPoint(t.position) : t.localPosition;
        lp.z = targetLocalZ;
        Vector3 wp = commonParent ? commonParent.TransformPoint(lp) : lp;

        // 2) 如果有刚体：直接瞬移刚体到该“相对位”；否则改 transform
        var rb = t.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.position = wp;                 // 不用 MovePosition，纯瞬移
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.WakeUp();
        }
        else
        {
            t.position = wp;
        }

        Physics.SyncTransforms();
    }
}

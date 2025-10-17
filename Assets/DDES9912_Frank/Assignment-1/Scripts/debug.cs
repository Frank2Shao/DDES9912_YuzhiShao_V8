using UnityEngine;

public class DebugDualGearTeleporter : MonoBehaviour
{
    public enum TeleportMode { RigidbodyPosition, TransformPosition }

    [Header("Driven gear roots (Rigidbody)")]
    public Rigidbody forwardRoot;     // forwardCogRoot 的 Rigidbody
    public Rigidbody backwardRoot;    // backwardCogRoot 的 Rigidbody

    [Header("Optional common parent (leave empty to use each root's parent)")]
    public Transform commonParent;    // 比如 SP&CSP&p，或留空用各自 parent

    [Header("Offsets from each root's Neutral (meters)")]
    public float driveOffset   =  0.05f;    // forward 贴近主动轮
    public float reverseOffset = -0.05f;    // backward 贴近主动轮

    [Header("Teleport path")]
    public TeleportMode teleportMode = TeleportMode.RigidbodyPosition;

    [Header("Debug")]
    public bool verbose = true;

    float neutralZF, neutralZB;

    void Start()
    {
        if (!ValidateHierarchy()) { enabled = false; return; }

        neutralZF = ReadLocalZ(forwardRoot.transform);
        neutralZB = ReadLocalZ(backwardRoot.transform);

        if (verbose)
        {
            Debug.Log($"[Init] neutralZF={neutralZF:F4}, neutralZB={neutralZB:F4}, parent={(commonParent? commonParent.name : "<each-root-parent>")}");
            DumpRB("Fwd.Init", forwardRoot);
            DumpRB("Back.Init", backwardRoot);
        }
    }

    [ContextMenu("Go Neutral")]
    public void GoNeutral()
    {
        if (!ValidateHierarchy()) return;
        SetLocalZ(forwardRoot,  neutralZF,  "Neutral.Fwd");
        SetLocalZ(backwardRoot, neutralZB,  "Neutral.Back");
    }

    [ContextMenu("Go Drive")]
    public void GoDrive()
    {
        if (!ValidateHierarchy()) return;
        SetLocalZ(forwardRoot,  neutralZF + driveOffset,   "Drive.Fwd");
        SetLocalZ(backwardRoot, neutralZB,                 "Drive.Back->Neutral");
    }

    [ContextMenu("Go Reverse")]
    public void GoReverse()
    {
        if (!ValidateHierarchy()) return;
        SetLocalZ(forwardRoot,  neutralZF,                 "Reverse.Fwd->Neutral");
        SetLocalZ(backwardRoot, neutralZB + reverseOffset, "Reverse.Back");
    }

    [ContextMenu("Validate Hierarchy")]
    public bool ValidateHierarchy()
    {
        if (!forwardRoot)  { Debug.LogError("[Check] forwardRoot 未设置");  return false; }
        if (!backwardRoot) { Debug.LogError("[Check] backwardRoot 未设置"); return false; }

        if (forwardRoot.transform == backwardRoot.transform)
        {
            Debug.LogError("[Check] forwardRoot 与 backwardRoot 指向了同一个 Transform？");
            return false;
        }

        if (forwardRoot.isKinematic || backwardRoot.isKinematic)
            Debug.LogWarning($"[Check] 有 isKinematic=true 的刚体（Fwd:{forwardRoot.isKinematic}, Back:{backwardRoot.isKinematic}）。Teleport 依然能改 transform.position，但物理可能不跟。建议先都设为非 kinematic 试。");

        return true;
    }

    void SetLocalZ(Rigidbody rb, float targetLocalZ, string tag)
    {
        if (!rb) return;

        Transform t = rb.transform;
        Transform p = commonParent ? commonParent : t.parent;

        Vector3 lpNow  = p ? p.InverseTransformPoint(t.position) : t.position;
        Vector3 lpNext = lpNow; lpNext.z = targetLocalZ;
        Vector3 wpNext = p ? p.TransformPoint(lpNext) : lpNext;

        if (verbose)
        {
            Debug.Log(
                $"[{tag}] parent={(p? p.name : "<none>")}, mode={teleportMode}\n" +
                $"    lpNow.z={lpNow.z:F4} -> lpNext.z={lpNext.z:F4}\n" +
                $"    wpNow=({t.position.x:F4},{t.position.y:F4},{t.position.z:F4}) -> wpNext=({wpNext.x:F4},{wpNext.y:F4},{wpNext.z:F4})\n" +
                $"    RB isKin={rb.isKinematic}, constraints={rb.constraints}, asleep={rb.IsSleeping()}"
            );
        }

        switch (teleportMode)
        {
            case TeleportMode.RigidbodyPosition:
                rb.position = wpNext;                 // 纯瞬移刚体位置
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.WakeUp();
                break;

            case TeleportMode.TransformPosition:
                t.position = wpNext;                  // 强制改 Transform
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.WakeUp();
                break;
        }

        Physics.SyncTransforms();

        if (verbose)
        {
            Vector3 lpAfter = p ? p.InverseTransformPoint(t.position) : t.position;
            Debug.Log($"[{tag}.After] lpAfter.z={lpAfter.z:F4}, wpAfter=({t.position.x:F4},{t.position.y:F4},{t.position.z:F4})");
        }
    }

    float ReadLocalZ(Transform t)
    {
        Transform p = commonParent ? commonParent : t.parent;
        Vector3 lp = p ? p.InverseTransformPoint(t.position) : t.position;
        return lp.z;
    }

    void DumpRB(string tag, Rigidbody rb)
    {
        if (!rb) { Debug.LogWarning($"[{tag}] RB is null"); return; }
        var t = rb.transform;
        Debug.Log(
            $"[{tag}] name={t.name}, isKin={rb.isKinematic}, constraints={rb.constraints}, " +
            $"pos=({t.position.x:F3},{t.position.y:F3},{t.position.z:F3}), " +
            $"parent={(t.parent? t.parent.name : "<none>")}, commonParent={(commonParent? commonParent.name : "<none>")}"
        );
    }
}


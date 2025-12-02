using UnityEngine;
using UnityEngine.AI;

public class PauseNPCOnTrigger : MonoBehaviour
{
    [Header("References")]
    public Animator npcAnimator;
    public NavMeshAgent npcAgent;

    public NMAWalkTowards walker; 

    [Header("Settings")]
    public string playerTag = "Player";

    // Animator 里的打断参数名：记得和 Animator 参数保持一致
    public string interruptedBoolName = "IsInterrupted";

    private bool isPaused = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        isPaused = true;

        // 物理上停止移动
        if (npcAgent != null)
            npcAgent.isStopped = true;

        // 告诉 Animator：现在是被打断状态
        if (npcAnimator != null && !string.IsNullOrEmpty(interruptedBoolName))
            npcAnimator.SetBool(interruptedBoolName, true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        isPaused = false;

        // 恢复移动
        if (npcAgent != null)
            npcAgent.isStopped = false;

        // 告诉 Animator：解除打断，回到正常逻辑
        if (npcAnimator != null && !string.IsNullOrEmpty(interruptedBoolName))
            npcAnimator.SetBool(interruptedBoolName, false);
    }
}

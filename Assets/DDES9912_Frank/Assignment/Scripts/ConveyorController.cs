using UnityEngine;

public class ConveyorController : MonoBehaviour
{
    [Header("Animator Settings")]
    public Animator animator;
    public string TriggerName = "";

    private bool isPlaying = false;

    private void Awake()
    {
        // if animator not assigned, get it from the current GameObject
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    public void PlayAnimation()
    {
        if (animator == null) return;

        //if already playing, do nothing
        if (isPlaying) return;

        isPlaying = true;
        animator.ResetTrigger(TriggerName);
        animator.SetTrigger(TriggerName);
    }

    /*private void Start()
    {
        PlayAnimation();
    }*/

    /// reverse
    public void OnAnimationFinished()
    {
        isPlaying = false;
    }
}

using UnityEngine;
using System.Collections;

public class TestController : MonoBehaviour
{
    [Header("References")]
    public Animator animator;

    [Header("Animator Parameters")]
    public string Trigger = "PlayFirstPart";   // Trigger to the animation

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    // let the first part start after a delay
    public void StartWithDelay()
    {
        StartCoroutine(StartPartCoroutine());
    }

    private IEnumerator StartPartCoroutine()
    {
        // wait for 5 seconds before starting the first part
        yield return new WaitForSeconds(5f);

        // trigger the first part of the animation
        if (animator != null && !string.IsNullOrEmpty(Trigger))
        {
            animator.SetTrigger(Trigger);
        }
    }
}

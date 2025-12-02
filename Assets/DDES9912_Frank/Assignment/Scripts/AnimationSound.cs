using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AnimationSound : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip[] clips; // Array to hold multiple audio clips

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // method to be called by animation event
    public void playSound(int index)
    {
        if (audioSource == null || clips == null || clips.Length == 0)
            return;

        if (index < 0 || index >= clips.Length)
            return;

        audioSource.clip = clips[index];
        audioSource.Play();
    }
    public void stopSound()
{
    if (audioSource == null) return;
    audioSource.Stop();
}
}

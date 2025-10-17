using UnityEngine;
using System.Collections;

public class SoundOnSpin : MonoBehaviour
{
    public Rigidbody subject;
    private AudioSource musicSource;
    public float rotationThreshold = 0.3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public UseGas engine;
    public float fadeDuration = 3.0f;
    private bool wasEngineRunning = false;
    private Coroutine fadingCo = null;

    void Start()
    {
        subject = GetComponent<Rigidbody>();
        musicSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (engine != null)
        {
            if (!engine.IsEngineRunning)
            {
                if (wasEngineRunning)
                {
                    TryStartFadeOutAndStop();
                }
                wasEngineRunning = false;
                return; // only when engine is on
            }
            else
            {
                wasEngineRunning = true; 
            }
        }

        float angularSpeed = subject.angularVelocity.magnitude;

        if (angularSpeed > rotationThreshold)
        {
            if (fadingCo != null)
            {
                StopCoroutine(fadingCo);
                fadingCo = null;
            }
            if (!musicSource.isPlaying)
                musicSource.Play();
        }
        else
        {
            if (fadingCo == null && musicSource.isPlaying)
                musicSource.Pause();
        }
    }

    private void TryStartFadeOutAndStop()
    {
        if (musicSource == null) return;
        if (fadingCo != null) StopCoroutine(fadingCo);
        fadingCo = StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float start = musicSource.volume; 
        float mid = 0.5f;
        float half = fadeDuration * 0.5f;

        // 1 -> 0.5
        float t = 0f;
        while (t < half)
        {
            t += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(start, mid, t / half);
            yield return null;
        }

        // end
        t = 0f;
        while (t < half)
        {
            t += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(mid, 0f, t / half);
            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = 1f; 
        fadingCo = null;
    }
}

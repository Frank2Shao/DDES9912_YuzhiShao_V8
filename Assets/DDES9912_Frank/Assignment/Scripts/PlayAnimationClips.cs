using UnityEngine;

public class Animation : MonoBehaviour
{
    
    [Header("Target to control")]
    [SerializeField] private GameObject model;   // model

    public void Play()   // shows up in Button OnClick()， explode the engine
    {
        if (!model) return;

        // activate
        if (!model.activeSelf) model.SetActive(true);

        // use animation
        var anim = model.GetComponentInChildren<Animator>(); // Animator

        anim.speed = 1f;
        anim.Play("EngineExplode", 0, 0f);
    }

     public void PlayReverse()   // re assemble the engine
    {
        Debug.Log("PlayReverse clicked");
        if (!model) return;

        if (!model.activeSelf) model.SetActive(true);

        var anim = model.GetComponentInChildren<Animator>();
        if (anim == null) return;

        // reverse play
        anim.Play("EngineAssemble1", 0, 0f);
        
    }

    public void Stop()   // optional "stop" button
    {
        if (!model) return;

        var anim = model.GetComponent<Animator>();
    }
}


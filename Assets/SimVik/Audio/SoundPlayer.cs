using System.Collections;
using System.Collections.Generic;
using SimVik;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public List<AudioClip> clips;
    [Range(0f,1f)]public float volume = 1f;
    [Range(0f,0.5f)]public float pitchVariance = 0.1f;
    public bool playOnStart = true;

    void OnEnable()
    {
        if (playOnStart) Play();
    }

    public void Play()
    {
        if (clips.Count == 0) return;
        var clip = clips[Random.Range(0, clips.Count)];
        var pitch = Random.Range(1f - pitchVariance, 1f + pitchVariance);
        clip.Play( volume, pitch );
    }
}
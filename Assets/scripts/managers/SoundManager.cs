using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    AudioSource audioSource;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void PlaySound(AudioClip clip)
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(clip);
    }
    public void PlaySound_DiffPitching(AudioClip clip)
    {
        float pitchValue = Random.Range(0.9f, 1.2f);
        audioSource.pitch = pitchValue;
        audioSource.PlayOneShot(clip);
    }
    public void PlaySound_SpecPitching(AudioClip clip, float pitchValue)
    {
        audioSource.pitch = pitchValue;
        audioSource.PlayOneShot(clip);
    }
}

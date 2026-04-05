using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameSettings : MonoBehaviour
{
    float[] audioSourcesInitialVolumes;
    AudioSource[] AudioSusObjects;
    void Start()
    {
        AudioSusObjects = FindObjectsOfType<AudioSource>();
        audioSourcesInitialVolumes = new float[AudioSusObjects.Length];

        int i = 0;
        foreach (AudioSource component in AudioSusObjects)
        {
            GameObject obj = component.gameObject;
            audioSourcesInitialVolumes[i] = component.volume;
            component.volume = audioSourcesInitialVolumes[i++] * 0.5f;
            Debug.Log("sound object: " + obj.name + " volume: " + component.volume);
        }
    }
    void Update()
    {
        
    }
    public void VolumeUpdate(Slider slider)
    {
        int i = 0;
        foreach (AudioSource component in AudioSusObjects)
        {
            component.volume = audioSourcesInitialVolumes[i++] * slider.value;
        }
    }
}

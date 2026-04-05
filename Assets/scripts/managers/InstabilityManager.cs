using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstabilityManager : MonoBehaviour
{
    [SerializeField] private PlayerInstabilities player;

    [SerializeField] private List<string> goodEffects = new List<string>();
    [SerializeField] private List<string> badEffects = new List<string>();

    [SerializeField] private float timeBetweenEvents = 5.0f;

    private float timer;
    private void OnEnable()
    {
        // Subscribe to the event from the Player script.
        InteracableEffect.OnPlayerEnteredBossArea += BossFightStarted;
    }

    void Start()
    {
        timer = timeBetweenEvents;
    }


    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            TriggerInstabilityEvent();
            timer = timeBetweenEvents;
        }
    }
    public void BossFightStarted()
    {
        float currentInstability = player.Instability;
        if (currentInstability < 65)
        {
            badEffects.Add("rubber duck");
            goodEffects.Add("rubber duck");
        }
        else if (currentInstability >= 65)
        {
            goodEffects.Add("powerup shoot");
        }
    }
    private void TriggerInstabilityEvent()
    {
        float currentInstability = player.Instability;

        string chosenEffectName;
        string effectLevel;

        if (currentInstability < 50f)
        {
            effectLevel = "bad";
            if (badEffects.Count > 0)
            {
                int randomIndex = Random.Range(0, badEffects.Count);
                chosenEffectName = badEffects[randomIndex];
            }
            else
            {
                Debug.LogWarning("Bad effects list is empty!");
                return;
            }
        }
        else
        {
            effectLevel = "good";
            if (goodEffects.Count > 0)
            {
                int randomIndex = Random.Range(0, goodEffects.Count);
                chosenEffectName = goodEffects[randomIndex];
            }
            else
            {
                Debug.LogWarning("Good effects list is empty!");
                return;
            }
        }

        player.ApplyEffect(chosenEffectName, effectLevel);
        Debug.LogWarning($"Instability Event Triggered! Player Instability: {currentInstability}. Applying a '{effectLevel}' effect: '{chosenEffectName}'.");
    }
}

using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PlayerInstabilities : MonoBehaviour
{
    private float instabilityValue; // backing field

    public event System.Action<float> OnInstabilityChanged;

    public float Instability
    {
        get => instabilityValue;
        set
        {
            float clamped = Mathf.Clamp(value, 0, 100);
            if (Mathf.Approximately(instabilityValue, clamped)) return;

            instabilityValue = clamped;
            OnInstabilityChanged?.Invoke(instabilityValue);
        }
    }

    private float timer = 0f;
    public float interval = 2f; // changeable in Inspector

    public float defaultSpeedValue;
    public float defaultFireRateValue;
    public float defaultAttackStrrengthValue;
    public float defaultDamagingValue;

    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerAttack playerAttack;
    [SerializeField] PlayerProperties playerProperties;
    [SerializeField] CurseUI_Manager curseUI_Manager;

    [Header("Checkpoint System")]
    [SerializeField] private Transform defaultStartPoint, defaultBossPoint; // The starting point if no checkpoint exists
    [SerializeField] private int checkpointLives = 3; // The number of lives given at a checkpoint
    private int currentLives;

    private string currentEffect;
    bool boosFight;

    private void OnEnable()
    {
        // Subscribe to the event from the Player script.
        InteracableEffect.OnPlayerEnteredBossArea += SaveCheckpoint;
    }

    private void OnDisable()
    {
        // Unsubscribe from the event to prevent memory leaks.
        InteracableEffect.OnPlayerEnteredBossArea -= SaveCheckpoint;
    }

    void Start()
    {
        // Load the game state at the start.
        LoadCheckpoint();



        boosFight = false;

        defaultSpeedValue = playerMovement.moveSpeed;
        defaultFireRateValue = playerAttack.fireRate;
        defaultAttackStrrengthValue = playerAttack.damageValue;
        defaultDamagingValue = playerProperties.damageFactor;

        currentEffect = "";
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (!boosFight)
        {
            if (timer >= interval)
            {
                timer = 0f;
                Instability--;
            }
        }
    }

    public int GetCurrentLives()
    {
        return currentLives;
    }

    public void InstabilityChange(int value)
    {
        if (!boosFight)
        {
            Instability += value;
        }
    }

    public void ApplyEffect(string effect, string level)
    {
        switch (currentEffect)
        {
            case "Move speed":
                MoveSpeedEffect("normal");
                break;
            case "fire rate":
                FireRateEffect("normal");
                break;
            case "attack strength":
                AttackStrengthEffect("normal");
                break;
            case "damage value":
                DamageValueEffect("normal");
                break;
            case "rubber duck":
                RubberDuckEffect("normal");
                break;
            case "powerup shoot":
                PowerupShootEffect("normal");
                break;
            default:
                break;
        }
        currentEffect = effect;
        //==============================
        switch (currentEffect)
        {
            case "Move speed":
                MoveSpeedEffect(level);
                break;
            case "fire rate":
                FireRateEffect(level);
                break;
            case "attack strength":
                AttackStrengthEffect(level);
                break;
            case "damage value":
                DamageValueEffect(level);
                break;
            case "rubber duck":
                RubberDuckEffect(level);
                break;
            case "powerup shoot":
                PowerupShootEffect(level);
                break;
            default:
                Debug.LogWarning($"No effect called {effect} in the switch");
                break;
        }
    }

    // Methods for each effect
    private void MoveSpeedEffect(string type)
    {
        switch (type.ToLower())
        {
            case "good":
                Debug.Log("Move speed increased!");
                float increasedValue = (float)Math.Round(UnityEngine.Random.Range(1.3f, 1.8f), 1);
                playerMovement.moveSpeed *= increasedValue;

                // Add logic to boost move speed
                curseUI_Manager.Set_UI_Elements("Move Speed", "*" + increasedValue.ToString(), "good");
                break;
            case "bad":
                Debug.Log("Move speed decreased!");
                float decreasedValue = (float)Math.Round(UnityEngine.Random.Range(0.4f, 0.8f), 1);
                playerMovement.moveSpeed *= decreasedValue;

                // Add logic to reduce move speed
                curseUI_Manager.Set_UI_Elements("Move Speed", "*" + decreasedValue.ToString(), "bad");
                break;
            case "normal":
                Debug.Log("Move speed unchanged.");
                playerMovement.moveSpeed = defaultSpeedValue;
                // Optional: keep move speed the same
                break;
            default:
                Debug.LogWarning($"Unknown type '{type}' for MoveSpeedEffect.");
                break;
        }
    }

    private void FireRateEffect(string type)
    {
        switch (type.ToLower())
        {
            case "good":
                Debug.Log("Fire rate increased!");
                float decreasedValue = (float)Math.Round(UnityEngine.Random.Range(0.5f, 0.8f), 1);
                playerAttack.fireRate *= decreasedValue;

                curseUI_Manager.Set_UI_Elements("Fire Rate", "*" + decreasedValue.ToString(), "good");
                break;
            case "bad":
                Debug.Log("Fire rate decreased!");
                float increasedValue = (float)Math.Round(UnityEngine.Random.Range(1.2f, 2.4f), 1);
                playerAttack.fireRate *= increasedValue;

                curseUI_Manager.Set_UI_Elements("Fire Rate", "*" + increasedValue.ToString(), "bad");
                break;
            case "normal":
                Debug.Log("Fire rate unchanged.");
                playerAttack.fireRate = defaultFireRateValue;

                break;
            default:
                Debug.LogWarning($"Unknown type '{type}' for FireRateEffect.");
                break;
        }
    }

    private void AttackStrengthEffect(string type)
    {
        switch (type.ToLower())
        {
            case "good":
                Debug.Log("Attack strength increased!");
                float increasedValue = (float)Math.Round(UnityEngine.Random.Range(1.2f, 3f), 1);
                playerAttack.damageValue *= increasedValue;

                curseUI_Manager.Set_UI_Elements("Attack Strength", "*" + increasedValue.ToString(), "good");
                break;
            case "bad":
                Debug.Log("Attack strength decreased!");
                float decreasedValue = (float)Math.Round(UnityEngine.Random.Range(0.4f, 0.8f), 1);
                playerAttack.damageValue *= decreasedValue;

                curseUI_Manager.Set_UI_Elements("Attack Strength", decreasedValue.ToString(), "bad");
                break;
            case "normal":
                Debug.Log("Attack strength unchanged.");
                playerAttack.damageValue = defaultAttackStrrengthValue;

                break;
            default:
                Debug.LogWarning($"Unknown type '{type}' for AttackStrengthEffect.");
                break;
        }
    }

    private void DamageValueEffect(string type)
    {
        switch (type.ToLower())
        {
            case "good":
                Debug.Log("Damage value increased!");
                playerProperties.damageFactor *= 0.5f;

                curseUI_Manager.Set_UI_Elements("Damage Value", "You will get half damage", "good");

                break;
            case "bad":
                Debug.Log("Damage value decreased!");
                playerProperties.damageFactor *= 2f;

                curseUI_Manager.Set_UI_Elements("Damage Value", "You will get double damage", "bad");
                break;
            case "normal":
                Debug.Log("Damage value unchanged.");
                playerProperties.damageFactor = defaultDamagingValue;

                break;
            default:
                Debug.LogWarning($"Unknown type '{type}' for DamageValueEffect.");
                break;
        }
    }

    private void RubberDuckEffect(string type)
    {
        switch (type.ToLower())
        {
            case "good":
            case "bad":
                Debug.Log("Rubber duck is sad!");
                playerAttack.duckShoot = true;
                curseUI_Manager.Set_UI_Elements("Rubber Duck curse", "On", "bad");
                break;
            case "normal":
                playerAttack.duckShoot = false;
                Debug.Log("Rubber duck is calm.");
                break;
            default:
                Debug.LogWarning($"Unknown type '{type}' for RubberDuckEffect.");
                break;
        }
    }

    private void PowerupShootEffect(string type)
    {
        switch (type.ToLower())
        {
            case "good":
                Debug.Log("Powerup shoot is strong!");
                curseUI_Manager.Set_UI_Elements("Powerup Shoot blessing", "On -not ready yet-", "good");
                break;
            case "normal":
                Debug.Log("Powerup shoot is normal.");
                break;
            default:
                Debug.LogWarning($"Unknown type '{type}' for PowerupShootEffect.");
                break;
        }
    }

    /// <summary>
    /// Loads the player's state from a saved checkpoint or sets default values.
    /// </summary>
    private void LoadCheckpoint()
    {
        // Check if a checkpoint exists by looking for a saved key.
        if (PlayerPrefs.HasKey("CheckpointSaved"))
        {
            Debug.Log("Checkpoint found! Loading saved data.");

            // Load position
            float x = PlayerPrefs.GetFloat("CheckpointX");
            float y = PlayerPrefs.GetFloat("CheckpointY");
            float z = PlayerPrefs.GetFloat("CheckpointZ");
            transform.position = new Vector3(x, y, z);

            // Load instability and lives
            Instability = PlayerPrefs.GetFloat("CheckpointInstability");
            currentLives = PlayerPrefs.GetInt("CheckpointLives");

        }
        else
        {
            Debug.Log("No checkpoint found. Starting at default position.");
            // Set the player's position to the default start point.
            if (defaultStartPoint != null)
            {
                transform.position = defaultStartPoint.position;
            }

            // Initialize with default values.
            Instability = 20; // Or whatever your initial value is
            currentLives = checkpointLives;
        }
    }

    /// <summary>
    /// Saves the player's current state as a checkpoint.
    /// This is called when the player enters the boss area.
    /// </summary>
    private void SaveCheckpoint()
    {
        Debug.Log("Saving checkpoint...");

        // Save the player's current position.
        PlayerPrefs.SetFloat("CheckpointX", defaultBossPoint.position.x);
        PlayerPrefs.SetFloat("CheckpointY", defaultBossPoint.position.y);
        PlayerPrefs.SetFloat("CheckpointZ", defaultBossPoint.position.z);

        // Save the current instability and lives.
        PlayerPrefs.SetFloat("CheckpointInstability", Instability);
        PlayerPrefs.SetInt("CheckpointLives", currentLives);

        // Set a flag to indicate that a checkpoint exists.
        PlayerPrefs.SetInt("CheckpointSaved", 1);

        boosFight = true;

        Debug.Log("Checkpoint saved successfully!");
    }

    /// <summary>
    /// Handles player respawn after death.
    /// </summary>
    public bool Respawn()
    {
        if (PlayerPrefs.HasKey("CheckpointSaved"))
        {
            currentLives--;
            Debug.Log($"Player died. Lives remaining: {currentLives}");

            // Check if there are remaining checkpoint lives.
            if (currentLives <= 0)
            {
                Debug.Log("Out of checkpoint lives. Deleting checkpoint and restarting from default.");
                PlayerPrefs.DeleteKey("CheckpointSaved"); // Delete the flag
                PlayerPrefs.DeleteKey("CheckpointX");
                PlayerPrefs.DeleteKey("CheckpointY");
                PlayerPrefs.DeleteKey("CheckpointZ");
                PlayerPrefs.DeleteKey("CheckpointInstability");
                PlayerPrefs.DeleteKey("CheckpointLives");

                return false;
            }
            else
            {
                Debug.Log("Respawning at last checkpoint.");
                // Respawn at the last saved checkpoint position.
                float x = PlayerPrefs.GetFloat("CheckpointX");
                float y = PlayerPrefs.GetFloat("CheckpointY");
                float z = PlayerPrefs.GetFloat("CheckpointZ");
                transform.position = new Vector3(x, y, z);

                // Restore instability
                Instability = PlayerPrefs.GetFloat("CheckpointInstability");
                return true;
            }
        }
        return false;
    }
}
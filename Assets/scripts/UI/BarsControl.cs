using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarsControl : MonoBehaviour
{
    [SerializeField] PlayerProperties playerProperties;
    [Header("health bar")]
    [SerializeField] Image healthBar;
    [SerializeField] TextMeshProUGUI healthText;

    [Header("magic stability bar")]
    [SerializeField] Image magicStabilityBar;
    [SerializeField] TextMeshProUGUI magicStabilityText;

    [SerializeField] PlayerInstabilities PlayerInstabilities;
    private void Awake()
    {
        PlayerInstabilities.OnInstabilityChanged += magicStabilityUpdate;
    }
    void Start()
    {
        HealthUpdate(playerProperties.maxHealth);
    }

    void Update()
    {
        
    }
    public void HealthUpdate(float newHealth)
    {
        healthBar.fillAmount = newHealth/ playerProperties.maxHealth;
        healthText.text = string.Format("{0:00}/{1:00}", newHealth, playerProperties.maxHealth);
    }
    public void magicStabilityUpdate(float value)
    {
        magicStabilityBar.fillAmount = value / 100;
        magicStabilityText.text = string.Format("{0:00}/100", value);
    }
}

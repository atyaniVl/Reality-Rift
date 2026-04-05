using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossHealth : MonoBehaviour
{
    [SerializeField] public float maxHealth { get; private set; }
    public float currHealth { get; private set; }
    public bool canGetDamaged { get; private set; }
    [SerializeField] float iFrameDuration;
    [SerializeField] int flashesCount;
    SpriteRenderer spriteRenderer;

    [SerializeField] Boss boss;

    [Header("health bar")]
    [SerializeField] Image healthBar;



    void Start()
    {
        maxHealth = 1000;
        canGetDamaged = true;
        currHealth = maxHealth;
         spriteRenderer = GetComponent< SpriteRenderer>();

        HealthUpdate(maxHealth);
    }
    public void HealthUpdate(float newHealth)
    {
        healthBar.fillAmount = newHealth / maxHealth;
    }
    public void TakeDamage(float _damage = 10)
    {
        if (canGetDamaged)
        {
            canGetDamaged = false;
            currHealth = Mathf.Clamp(currHealth - _damage , 0, maxHealth);
            HealthUpdate(currHealth);
            if (currHealth > 0)
            {
                StartCoroutine(iFrame());
                if (currHealth < 40)
                {
                    boss.phase = 2;
                }
            }
            else
            {
                gameObject.SetActive(false);
                boss.phase = 3;
            }
        }
    }
    IEnumerator iFrame()
    {
        Physics2D.IgnoreLayerCollision(3, 9, true);
        for (int i = 0; i < flashesCount; i++)
        {
            float timeToWait = iFrameDuration / (flashesCount * 2);
            spriteRenderer.color = new Color(0.4f, 0.8f, 1, 0.5f);
            yield return new WaitForSeconds(timeToWait);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(timeToWait);
        }
        Physics2D.IgnoreLayerCollision(3, 9, false);
        canGetDamaged = true;
        yield return null;
    }


}

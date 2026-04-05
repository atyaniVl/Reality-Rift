using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerProperties : MonoBehaviour
{
    [SerializeField] BarsControl barsControl;
    [SerializeField] PlayerInstabilities playerInstabilities;
    [SerializeField] public float maxHealth { get; private set; }
    [SerializeField] LayerMask enemy;
    public float currHealth { get; private set; }
    public float damageFactor { get; set; } //instability can change this value
    public bool canGetDamaged { get; private set; }
    [SerializeField] float iFrameDuration;
    [SerializeField] int flashesCount;
    [SerializeField] AudioClip damage, healSFX, die, win;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        damageFactor = 1;
        maxHealth = 100;
        canGetDamaged = true;
        currHealth = maxHealth;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    [ContextMenu("damage player")]
    public void TakeDamage(float _damage = 10)
    {
        if (canGetDamaged)
        {
            if (damage != null) SoundManager.instance.PlaySound_DiffPitching(damage);
            canGetDamaged = false;
            currHealth = Mathf.Clamp(currHealth - _damage * damageFactor, 0, maxHealth);
            barsControl.HealthUpdate(currHealth);
            playerInstabilities.InstabilityChange(-2);
            if (currHealth > 0)
            {
                gameObject.GetComponent<Animator>().SetTrigger("takeDamage");
                StartCoroutine(iFrame());
            }
            else
            {
                DeathMethod();
            }
        }
    }

    public void DeathMethod()
    {
        if (die != null) SoundManager.instance.PlaySound_SpecPitching(die, 0.1f);
        StartCoroutine(Death());
    }

    public void TakeHeal(float heal)
    {
        if (healSFX != null) SoundManager.instance.PlaySound(healSFX);
        currHealth = Mathf.Clamp(currHealth + heal, 0, maxHealth);
        barsControl.HealthUpdate(currHealth);
        StartCoroutine(HealingAnimation());
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

    IEnumerator HealingAnimation()
    {
        yield return null;
        for (int i = 0; i < flashesCount; i++)
        {
            float timeToWait = iFrameDuration / (flashesCount * 2);
            spriteRenderer.color = new Color(1, 0.8f, 0.2f, 0.5f);
            yield return new WaitForSeconds(timeToWait);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(timeToWait);
        }
    }

    // Modified Death coroutine
    IEnumerator Death()
    {
        // Disable player controls and visuals for the death animation
        GetComponent<PlayerAttack>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponentInChildren<Animator>().SetTrigger("die");

        yield return new WaitForSeconds(2f);

        // This is the core change: call the Respawn logic
        if (playerInstabilities.Respawn())
        {
            StartCoroutine(iFrame());
            currHealth = maxHealth; // Reset health
            barsControl.HealthUpdate(currHealth); // Update health UI
                                                  // Re-enable player components
            GetComponent<PlayerAttack>().enabled = true;
            GetComponent<PlayerMovement>().enabled = true;
            GetComponent<Collider2D>().enabled = true;
            GetComponent<Rigidbody2D>().gravityScale = GetComponent<PlayerMovement>().startGravityScale;
            GetComponentInChildren<Animator>().SetTrigger("respawn");

        }
        else
        {
            canGetDamaged = false;
            UI_Manager.instance.GameOverMenu(true);
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(9) && !collision.CompareTag("bat enemy"))
        {
            print("trigger an enemy");
            TakeDamage(5);
        }
        else if (collision.CompareTag("Win"))
        {
            print("trigger win");
            GameObject.FindWithTag("MainCamera").GetComponent<AudioSource>().Pause();
            GetComponentInChildren<Animator>().SetTrigger("isWin");
            SoundManager.instance.PlaySound(win);
            GetComponent<PlayerAttack>().enabled = false;
            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            UI_Manager.instance.WinMenu(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Bouneries"))
        {
            print("hereeeeeeeeeeeee");
            TakeDamage(100);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    protected Animator animator;
    public AudioClip damageSound, die;
    public float maxHealth { protected set; get; }
    public float health { protected set; get; }
    public float selfDamageFactor { protected set; get; }

    // Start is called before the first frame update
    private void Awake()
    {
        animator = GetComponent<Animator>();
        maxHealth = 100;
        selfDamageFactor = 3;
        health = maxHealth;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(float damage)
    {
        if(damageSound != null) SoundManager.instance.PlaySound_DiffPitching(damageSound);
        health = Mathf.Clamp(health - damage, 0, maxHealth);
        Debug.Log("witch health"+ health);
        if (health > 0)
        {
            animator.SetTrigger("takeDamage");
        }
        else
        {
            Debug.Log("witch died");
            SoundManager.instance.PlaySound_SpecPitching(die, 1.05f);
            GetComponent<Collider2D>().enabled = false;

            if (GetComponent<MeleeMovement>() is not null)
                GetComponent<MeleeMovement>().speed = 0;
            else if(GetComponent<WitchMovement>() is not null)
                GetComponent<WitchMovement>().enabled = false;
            else if (GetComponent<WitchHealth>() is not null)
                GetComponent<WitchHealth>().enabled = false;
            else if (GetComponent<WitchAttack>() is not null)
                GetComponent<WitchAttack>().enabled = false;

            GameObject.Find("player 1").GetComponent<PlayerInstabilities>().Instability += 20;

            animator.SetTrigger("die");
            Destroy(gameObject, 2);
        }
    }
    public void Die()
    {

    }

}

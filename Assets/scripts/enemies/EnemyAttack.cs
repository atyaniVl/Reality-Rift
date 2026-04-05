using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("enemy attack")]
    public Animator animator;
    public float attackCooldown;
    public Transform attackRange;
    public float rangeRadius;
    public  LayerMask playerLayer;
    public Collider2D PlayerInRange;
    public AudioClip attack;
    public float damage { set; get; }
    public bool colliderDetect { set; get; }

    private void Awake()
    {
        damage = 10;
    }
    void Start()
    {
        
    }

    void Update()
    {
        if (colliderDetect)
        {
            animator.SetBool("playerDetected", true);
            if (attackCooldown > 1.25f)
            {
                Attack();
            }
            else
                attackCooldown += Time.deltaTime;

        }
        else
        {
            animator.SetBool("playerDetected", false);
        }

    }
    public virtual void Attack()
    {
        PlayerInRange = null;
        animator.SetTrigger("attack");
        attackCooldown = 0;
        PlayerInRange = Physics2D.OverlapCircle(
            attackRange.position, rangeRadius, playerLayer);
    }
    public void PlayerDamaging()
    {
        if (colliderDetect && PlayerInRange is not null && PlayerInRange.CompareTag("Player"))
        {   
            PlayerInRange.gameObject.GetComponent<PlayerProperties>().TakeDamage(damage);
        }
        PlayerInRange = null;
    }
    void OnDrawGizmosSelected()
    {
        if (attackRange is null)
            return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackRange.position, rangeRadius);
    }
    public void AttackSound()
    {
        SoundManager.instance.PlaySound_DiffPitching(attack);
    }

}

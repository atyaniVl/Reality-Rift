using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    Animator animator;
    PlayerMovement playerMovement;
    public float fireRate { get; set; }
    float attackCooldown = Mathf.Infinity;
    [SerializeField] AudioClip attack, duckAttack;
    [SerializeField] Transform attackPoint;
    [SerializeField] ProjectilePool projectilePool;
    [SerializeField] private int projectilesPerBurst = 1; // The number of projectiles to fire in a single attack
    [SerializeField] private float timeBetweenProjectiles = 0.2f;
    public float damageValue { get; set; }
    public bool duckShoot;


    void Start()
    {
        duckShoot = false;
        fireRate = 0.5f;
        damageValue = 1.0f;
        animator = GetComponentInChildren<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (attackCooldown > fireRate)
        {
            if (Input.GetKeyDown(KeyCode.Space) && playerMovement.CanAttack())
                if (!duckShoot)
                    Attack();
                else
                    DuckAttack();
        }
        else
        {
            attackCooldown += Time.deltaTime;
        }
    }

    void Attack()
    {
        attackCooldown = 0;
        animator.SetTrigger("attack");
        Debug.Log("player shooted");


        
    }
    void DuckAttack()
    {
        attackCooldown = 0;
        animator.SetTrigger("attack");
        Debug.Log("player shooted duck");
    }


    private IEnumerator ShootBurstCoroutine()
    {
        if (!duckShoot)
        {
            /*for (int i = 0; i < projectilesPerBurst; i++)
            {*/
            GameObject projectileGO = projectilePool.GetProjectile();

            if (projectileGO != null)
            {
                projectileGO.transform.position = attackPoint.position;
                projectileGO.transform.rotation = attackPoint.rotation;

                PlayerProjectile projectileScript = projectileGO.GetComponent<PlayerProjectile>();

                if (projectileScript != null)
                {
                    projectileScript.SetDamage(damageValue);
                    float spreadDirection = Mathf.Sign(transform.localScale.x);
                    if (attack != null) SoundManager.instance.PlaySound_DiffPitching(attack);
                    projectileScript.Shoot(spreadDirection);
                }
            }

            yield return new WaitForSeconds(timeBetweenProjectiles);
            //}
        }
        else
        {
            /*for (int i = 0; i < projectilesPerBurst; i++)
{*/
            GameObject projectileGO = projectilePool.GetDuckProjectile();

            if (projectileGO != null)
            {
                projectileGO.transform.position = attackPoint.position;
                projectileGO.transform.rotation = attackPoint.rotation;

                DuckProjectile projectileScript = projectileGO.GetComponent<DuckProjectile>();

                if (projectileScript != null)
                {
                    projectileScript.SetDamage(10);
                    float spreadDirection = Mathf.Sign(transform.localScale.x);
                    if (attack != null) SoundManager.instance.PlaySound_DiffPitching(duckAttack);
                    projectileScript.Shoot(spreadDirection);
                }
            }

            yield return new WaitForSeconds(timeBetweenProjectiles);

        }
    }
}

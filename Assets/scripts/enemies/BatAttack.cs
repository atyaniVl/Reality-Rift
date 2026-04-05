using UnityEngine;
public class BatAttack : EnemyAttack
{
    [Header("BatAttack")]
    EnemyMovement enemyMovement;
    Vector3 playerPos;
    bool isAttacking;

    private void Awake()
    {
        attackCooldown = Mathf.Infinity;
        damage = 5;
        enemyMovement = GetComponent<EnemyMovement>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (PlayerDetection())
        {
            enemyMovement.moving = false;
            if (attackCooldown > 6)
            {
                playerPos = PlayerInRange.transform.position;
                Vector3 distance = (playerPos - transform.position).normalized;
                transform.Translate(distance*Time.deltaTime* enemyMovement.speed*1.3f);
                if(distance.x > 0)
                {
                    enemyMovement.SetScale(1);
                }
                else
                {
                    enemyMovement.SetScale(-1);
                }
            }
            else
            {
                attackCooldown += Time.deltaTime;
            }
        }
        else
        {
            if (!enemyMovement.backFromAttacking)
            {
                attackCooldown = Mathf.Infinity;
                enemyMovement.ResetSpeed();
                enemyMovement.backFromAttacking = true;
                enemyMovement.NextPositionIndex();
            }
            enemyMovement.moving = true;
        }
    }

    bool PlayerDetection()
    {
        PlayerInRange = Physics2D.OverlapCircle(
            attackRange.position, rangeRadius, playerLayer);

        return PlayerInRange != null && PlayerInRange.CompareTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerProperties>().TakeDamage(damage);
        }
        attackCooldown = Mathf.Infinity;
        enemyMovement.moving =true;
        enemyMovement.ResetSpeed();
        enemyMovement.NextPositionIndex();
    }
}

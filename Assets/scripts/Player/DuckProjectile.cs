using UnityEngine;

public class DuckProjectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f;

    private float currentSpeed = 5f;

    [SerializeField] private float initialVerticalSpeed = 5f;
    [SerializeField] private float gravity = 10f;

    private float timer;
    private float direction;
    private float damage;
    private float currentVerticalSpeed;

    private void OnEnable()
    {
        currentVerticalSpeed = initialVerticalSpeed;
    }

    public void SetDamage(float _damage)
    {
        damage = _damage;
    }

    public void Shoot(float _direction)
    {
        direction = _direction;
        transform.localScale = new Vector3(transform.localScale.x * _direction, transform.localScale.y, transform.localScale.z);
        timer = lifetime;
    }

    private void Update()
    {
        transform.position += new Vector3(direction * currentSpeed * Time.deltaTime,0, 0);

        currentSpeed -= currentSpeed * Time.deltaTime;

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.LogWarning("Projectile triggered with" + collision.gameObject.name);
        if ("enemy" == collision.gameObject.tag)
        {
            if (collision.gameObject.GetComponent<WitchHealth>() != null)
            {
                collision.gameObject.GetComponent<WitchHealth>().TakeDamage(10);
                ReturnToPool();
            }
        }
        else if ("melee enemy" == collision.gameObject.tag)
        {
            if (collision.gameObject.gameObject.GetComponent<EnemyHealth>() != null)
            {
                collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(10);
                ReturnToPool();
            }
        }
        else if ("boss" == collision.gameObject.tag)
        {
            collision.gameObject.GetComponent<BossHealth>().TakeDamage(damage * 5);
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        ParticleManager.Instance.PlayParticleEffect("duck projectile ps", transform.position, 0.5f);
        ProjectilePool.Instance.ReturnDuckProjectile(gameObject);
    }
}

using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{

    [SerializeField] private float lifetime = 0.5f;

    private float timer;
    private float direction;
    private float currentSpeed = 10;

    private float damage;

    private void OnEnable()
    {
    }
    public void SetDamage(float _damage)
    {
        damage = _damage;
    }

    public void Shoot(float _direction)
    {
        direction = _direction;
        transform.localScale = new Vector3(transform.localScale.x * _direction, transform.localScale.y, transform.localScale.z);
        GetComponent<Collider2D>().enabled = true;

        timer = lifetime;
    }

    private void Update()
    {
        transform.position += new Vector3(direction, 0, 0)* currentSpeed * Time.deltaTime;

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.LogWarning("Projectile triggered with" + other.name);
        if ("enemy" == other.gameObject.tag)
        {
            if(other.GetComponent<WitchHealth>() != null)
            {
                other.GetComponent<WitchHealth>().TakeDamage(damage*40);
                ReturnToPool();

            }
        }
        else if ("melee enemy" == other.gameObject.tag)
        {
            if (other.GetComponent<EnemyHealth>() != null)
            {
                other.GetComponent<EnemyHealth>().TakeDamage(damage);
                ReturnToPool();
            }
        }
        else if("ground" == other.gameObject.tag)
        {
            ReturnToPool();
        }
        else if ("boss" == other.gameObject.tag)
        {
            other.GetComponent<BossHealth>().TakeDamage(damage * 30);
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        ParticleManager.Instance.PlayParticleEffect("player Projectile ps", transform.position, 0.5f);
        ProjectilePool.Instance.ReturnProjectile(gameObject);
    }
}

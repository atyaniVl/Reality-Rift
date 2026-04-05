using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    // The speed at which the projectile will move.
    [SerializeField]
    private float moveSpeed = 5f;

    // The tags that will cause the projectile to be destroyed on collision.
    private const string PLAYER_TAG = "Player";
    private void Start()
    {
        Destroy(gameObject, 10f);
    }

    void Update()
    {
        // Move the projectile upwards along the Y-axis.
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object has the "Ground" or "Player" tag.

        if (other.CompareTag(PLAYER_TAG))
        {
            Debug.Log("playe should damage 25");
            other.GetComponent<PlayerProperties>().TakeDamage(25);
            ParticleManager.Instance.PlayParticleEffect("boss projectile ps", transform.position, 0.5f);

            Destroy(gameObject);
        }
    }
}

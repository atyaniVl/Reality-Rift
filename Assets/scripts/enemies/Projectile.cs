using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed {private set; get; }
    [SerializeField] AudioClip fireBallStart, fireBallExp;
    float lifeTime;
    float initialScale;
    Vector3 direction;
    bool hit;
    Animator animator;
    BoxCollider2D boxCollider;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null )
            Debug.Log("the -anmator- is null in -projectile-");
        boxCollider = GetComponent<BoxCollider2D>();
        if ( boxCollider == null )
            Debug.Log("the -collider- is null in -projectile-");
        speed = 5;
        initialScale = transform.localScale.x;
    }
    void Start()
    {
        SoundManager.instance.PlaySound_DiffPitching(fireBallStart);
    }

    void Update()
    {
        if (hit) return;
        transform.Translate(direction *speed* Time.deltaTime);
        if (gameObject.activeSelf && lifeTime > 5)
        {
            Explode();  
            lifeTime = 0;
        }
        lifeTime += Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Explode();
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerProperties>().TakeDamage(15);
        }
    }
    public void Explode()
    {
        hit = true;
        boxCollider.enabled = false;
        animator.SetTrigger("explode");
        SoundManager.instance.PlaySound_DiffPitching(fireBallExp);
    }
    public void DeActivate()
    {
        ParticleManager.Instance.PlayParticleEffect("witch projectile ps", transform.position, 0.5f);
        Destroy(gameObject);
    }
    public void SetDirection(Vector3 _dir)
    {
        hit = false;
        gameObject.SetActive(true);
        boxCollider.enabled = true;
        direction = _dir.normalized * speed;
        transform.localScale = new Vector3(initialScale * Mathf.Sign(_dir.x), initialScale, initialScale);
    }
}

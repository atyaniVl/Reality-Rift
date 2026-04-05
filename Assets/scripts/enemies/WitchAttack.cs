using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] wizerdPositions;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform attackRange;
    [SerializeField] private Transform projectilePos;

    [Header("Settings")]
    [SerializeField] private float rangeRadius = 3f;
    [SerializeField] private LayerMask playerLayer;   // ✅ Must contain ONLY the Player layer
    [SerializeField] private AudioClip hide;

    private Collider2D playerInRange;
    private Animator animator;
    private float initialScale;
    private float attackCooldown;
    private int shootIndex;

    // ✅ Hide from inspector so Unity doesn't override it
    [HideInInspector] public bool isAttacking = false;

    // ✅ Store the position of the player when the attack starts
    private Vector3 lockedTargetPos;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogWarning("Animator component is missing on WitchAttack object.");
    }

    private void Start()
    {
        initialScale = transform.localScale.x;
        attackCooldown = 0f;
        shootIndex = 0;
        isAttacking = false;
        lockedTargetPos = Vector3.zero;
    }

    private void Update()
    {
        bool playerDetected = PlayerDetection();

        // Start attacking only when the player is detected
        if (playerDetected && !isAttacking)
            isAttacking = true;

        // Trigger attack if cooldown finished and player is in range
        if (attackCooldown > 1.4f && playerDetected && isAttacking)
        {
            shootIndex++;
            lockedTargetPos = playerInRange.transform.position; // ✅ capture position now
            animator.SetTrigger("attack");
            attackCooldown = 0f;
        }

        // After 4 shots, hide and disable collider
        if (shootIndex >= 4)
        {
            shootIndex = 0;
            isAttacking = false;
            animator.SetTrigger("hide");
            GetComponent<Collider2D>().enabled = false;
        }

        attackCooldown += Time.deltaTime;
    }

    // Called by animation event
    public void Attack()
    {
        // ✅ Use the locked target position captured at attack start
        if (lockedTargetPos != Vector3.zero)
        {
            GameObject _projectile = Instantiate(projectile, projectilePos.position, Quaternion.identity);
            Vector3 dir = (lockedTargetPos - projectilePos.position).normalized;
            _projectile.GetComponent<Projectile>().SetDirection(dir);
        }
    }

    private bool PlayerDetection()
    {
        playerInRange = Physics2D.OverlapCircle(
            attackRange.position, rangeRadius, playerLayer);

        // ✅ Double check the detected object is tagged "Player"
        if (playerInRange != null && playerInRange.CompareTag("Player"))
        {
            // Face the player
            if (playerInRange.transform.position.x > transform.position.x)
                transform.localScale = new Vector3(initialScale, initialScale, initialScale);
            else
                transform.localScale = new Vector3(-initialScale, initialScale, initialScale);

            return true;
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackRange == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackRange.position, rangeRadius);
    }

    public void PlayHideSound()
    {
        SoundManager.instance.PlaySound_SpecPitching(hide, 1.2f);
    }
}

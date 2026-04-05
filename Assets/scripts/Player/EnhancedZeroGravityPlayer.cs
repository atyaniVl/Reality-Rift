using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class EnhancedZeroGravityPlayer : MonoBehaviour
{
    [Header("Gravity Settings")]
    public float normalGravity = 1f;       // Default gravity scale
    public float zeroGravity = 0f;         // Gravity scale in zero-G
    public float upwardImpulse = 5f;       // Initial upward push

    [Header("Floating Controls")]
    public float driftForce = 0.1f;        // Automatic small drift in zero-G
    public float floatNudgeForce = 0.2f;   // Left/right nudge force in zero-G
    public float downwardForce = 10f;      // Speed when pressing Down Arrow
    public float floatRestoreForce = 5f;   // Upward nudge when releasing Down Arrow

    [Header("Floating Limits")]
    public float floatHorizontalMultiplier = 0.2f; // Horizontal speed multiplier while floating

    [Header("Movement Settings")]
    public float speed = 5f;               // Walking speed

    [Header("Ground Check")]
    public Transform groundCheck;          // Empty GameObject at player's feet
    public float checkRadius = 0.1f;
    public LayerMask groundLayer;          // Layer assigned to ground objects

    private Rigidbody2D rb;
    private Collider2D col;
    private bool insideZeroG = false;
    private float originalFriction = 0.4f;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        if (col.sharedMaterial != null)
            originalFriction = col.sharedMaterial.friction;

        rb.gravityScale = normalGravity;
        Debug.Log("Player initialized with normal gravity.");
    }

    void Update()
    {
        // Check if player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        // Walking on the ground
        if (isGrounded && !insideZeroG)
        {
            HandleMovement();
        }

        // Floating in zero-G
        if (insideZeroG)
        {
            // Gentle automatic random drift
            if (rb.linearVelocity.magnitude < 0.1f)
            {
                Vector2 randomDir = Random.insideUnitCircle.normalized * driftForce;
                rb.AddForce(randomDir, ForceMode2D.Force);
            }

            // Player-controlled horizontal nudge (slower while floating)
            float horizontalInput = Input.GetAxis("Horizontal");
            rb.AddForce(new Vector2(horizontalInput * floatNudgeForce * floatHorizontalMultiplier, 0), ForceMode2D.Force);

            // Downward movement (no cooldown)
            if (Input.GetKey(KeyCode.DownArrow))
            {
                rb.gravityScale = normalGravity; // enable downward gravity
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -downwardForce); // move down instantly
            }
            else
            {
                // Return to zero-G and float back up smoothly
                rb.gravityScale = zeroGravity;
                rb.AddForce(Vector2.up * floatRestoreForce, ForceMode2D.Force);
            }
        }
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ZeroGravityRoom"))
        {
            insideZeroG = true;

            // Apply zero-G instantly
            rb.gravityScale = zeroGravity;

            // Lift player off the floor
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, upwardImpulse);

            // Remove friction for smooth floating
            if (col.sharedMaterial != null)
                col.sharedMaterial.friction = 0f;

            Debug.Log("Entered Zero-Gravity Room. Gravity = 0");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ZeroGravityRoom"))
        {
            insideZeroG = false;

            // Restore normal gravity instantly
            rb.gravityScale = normalGravity;

            // Restore friction
            if (col.sharedMaterial != null)
                col.sharedMaterial.friction = originalFriction;

            Debug.Log("Exited Zero-Gravity Room. Gravity restored to normal.");
        }
    }

    // Optional: visualize ground check in editor
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}
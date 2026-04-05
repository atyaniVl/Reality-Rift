using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    CapsuleCollider2D capsulCollider;

    [Header("Player Movement")]
    public float moveSpeed { get; set; }
    [SerializeField] float jumpForce;
    [SerializeField] float initialScale;

    bool isWalking;
    int wallJumpCounter;

    [Header("Jumping")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] Transform groundCheckRange;
    [SerializeField] float rangeRadius;
    public float wallJumpCooldown, startGravityScale, Horiz_axis;

    [Header("Coyote Time")]
    [SerializeField] float coyoteTime = 0.2f;
    float coyoteTimeCounter;

    /*[Header("dashing")]
    bool isDashing;
    public bool canDash;
    [SerializeField] float dashVelocity;
    [SerializeField] float dashTiming;
    Vector2 dashingDir;*/

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        capsulCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponentInChildren<Animator>();
        startGravityScale = rb.gravityScale;
        moveSpeed = 8.0f;
    }

    void Start()
    {

        //canDash = true;
    }

    void Update()
    {
        /*if (wallJumpCooldown > 0.2f)
        {
            Movement();
            //Dashing();
            if (!IsGrounded() && IsWalled())
            {
                rb.gravityScale = 0;
                rb.velocity = Vector2.zero;
            }
            else
            {
                rb.gravityScale = startGravityScale;
            }*/

        Movement();

        // Coyote Time Logic
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("start jump");
            Jump();
        }

        //}
        else
        {
            wallJumpCooldown += Time.deltaTime;
        }

        RaycastHit2D rayCastHit = Physics2D.CapsuleCast(capsulCollider.bounds.center,
            capsulCollider.bounds.size, capsulCollider.direction,
            0, new Vector2(transform.localScale.x, 0), 0.01f, groundLayer);
        if (animator != null)
        {
            animator.SetBool("isGrounded", IsGrounded());
        }
    }

    void Dashing()
    {
        /*if (canDash && Input.GetKeyDown(KeyCode.LeftShift))
        {
            isDashing = true;
            canDash = false;
            dashingDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")/100).normalized;
            if (dashingDir == Vector2.zero)
            {
                dashingDir = new Vector2(Mathf.Sign(transform.localScale.x), 0);
            }
            StartCoroutine(DashingCoroutine());
            if (isDashing)
            {
                Debug.Log("dashing");
                rb.linearVelocity = dashingDir * dashVelocity; 
                return;
            }
        }*/
    }

    void Movement()
    {
        Horiz_axis = Input.GetAxis("Horizontal");
        if (Horiz_axis > 0.1)
        {
            transform.localScale = Vector3.one * initialScale;
        }
        else if (Horiz_axis < -0.1)
        {
            transform.localScale = new Vector3(-1, 1, 1) * initialScale;
        }
        rb.linearVelocity = new Vector2(Horiz_axis * moveSpeed,
            rb.linearVelocity.y);
        if (animator != null)
        {
            animator.SetBool("isWalking", Horiz_axis != 0);

        }
        //MovingBackGround.Instance.IsWalkingTrigger(IsWalking(), IsBlocked(),
        // Mathf.Sign(transform.localScale.x));
        if (gameObject.transform.parent != null)
        {
            //MovingBackGround.Instance.IsWalkingTrigger(true, IsBlocked(),
            // Mathf.Sign(transform.localScale.x));
        }
        else
        {
            // MovingBackGround.Instance.IsWalkingTrigger(IsWalking(), IsBlocked(),
            // Mathf.Sign(transform.localScale.x));
        }
    }

    void Jump()
    {
        // Coyote jump logic is applied here. The player can jump if they are grounded
        // OR if the coyote time counter is greater than zero.
        if (IsGrounded() || coyoteTimeCounter > 0)
        {
            Debug.Log("player jumped");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            // Immediately set the counter to 0 after jumping to prevent double jumps.
            coyoteTimeCounter = 0;

            if (animator != null)
            {
                animator.SetTrigger("jump");
            }
        }
        /*else if(!IsGrounded() && IsWalled())
        {
            if (Horiz_axis == 0)
            {
                rb.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 15, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x) * initialScale,
                    transform.localScale.y, transform.localScale.z);
            }
            else
            {
                rb.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 2, 6);
            }
                wallJumpCooldown = 0;

        }  */
    }

    bool IsGrounded()
    {
        /*RaycastHit2D GroundedRayCastHit = Physics2D.CapsuleCast(capsulCollider.bounds.center,
            capsulCollider.bounds.size, capsulCollider.direction
                , 0, Vector2.down, 0.5f, groundLayer);*/
        Collider2D[] ground = Physics2D.OverlapCircleAll(
            groundCheckRange.position, rangeRadius, groundLayer);
        Debug.Log("ground colliders :" + ground.Length);
        return ground.Length > 0;
    }

    bool IsWalled()
    {
        return IsBlocked();
    }

    public bool CanAttack()
    {
        if (IsGrounded())
            return true;
        else
            return false;
    }

    bool IsWalking()
    {
        if (Horiz_axis == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    bool IsBlocked()
    {
        RaycastHit2D rayCastHit = Physics2D.CapsuleCast(capsulCollider.bounds.center,
            capsulCollider.bounds.size, capsulCollider.direction,
            0, new Vector2(transform.localScale.x, 0), 0.01f, groundLayer);
        //animator.SetBool("isWalled", rayCastHit.collider != null);
        return rayCastHit.collider != null;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheckRange is null)
            return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheckRange.position, rangeRadius);
    }

    IEnumerator DashingCoroutine()
    {
        /*yield return new WaitForSeconds(dashTiming);
        isDashing = false;*/
        yield return 0;
    }
}
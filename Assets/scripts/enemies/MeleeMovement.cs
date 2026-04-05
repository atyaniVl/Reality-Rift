using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeMovement : MonoBehaviour
{
    [SerializeField] float movementDistance;
    public float speed { set; get; }
    [SerializeField] Animator animator;
    [SerializeField] Transform attackArea;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] Vector2 attackingArea;
    bool movingLeft, isMoving;
    float leftEdge, rightEdge, startScale;
    private void Awake()
    {
        leftEdge = transform.position.x - movementDistance;
        rightEdge = transform.position.x + movementDistance;
        startScale = Mathf.Abs(transform.localScale.x);
        movingLeft = Mathf.Sign(transform.localScale.x) != 1 ? true : false;
        speed = 4;
        isMoving = true;
    }
    void Start()
    {
        
    }
    void Update()
    {
        if (isMoving)
        {
            if (movingLeft)
            {
                transform.localScale = new Vector3(-startScale, startScale, startScale);
                if (transform.position.x > leftEdge)
                {
                    transform.Translate(Vector3.left * speed * Time.deltaTime);
                }
                else
                {
                    movingLeft = !movingLeft;
                }
            }
            else
            {
                transform.localScale = new Vector3(startScale, startScale, startScale);
                if (transform.position.x < rightEdge)
                {
                    transform.Translate(Vector3.right * speed * Time.deltaTime);
                }
                else
                {
                    movingLeft = !movingLeft;
                }
            }
        }
        Collider2D PlayerInRange = Physics2D.OverlapBox(
            attackArea.position, attackingArea, playerLayer);
        if(PlayerInRange is not null)
        {
            if (PlayerInRange.CompareTag("Player")) 
            {
                isMoving = false;
                GetComponent<EnemyAttack>().colliderDetect = true;            
            }
            else
            {
                isMoving = true;
                GetComponent<EnemyAttack>().colliderDetect = false;

            }
        }
    }
    void OnDrawGizmosSelected()
    {
        if (attackArea is null)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackArea.position, attackingArea);
    }
}

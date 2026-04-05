using System.Collections.Generic;
using UnityEngine;
public class EnemyMovement : MonoBehaviour
{
    public Vector3 currPos, nextPos, distance;
    [Header("movement parameters")]
    public List<GameObject> movementPositions;
    public float idleTime;
    [SerializeField] protected float arrivalThreshold = 0.1f;
    public float speed;
    protected float idleCooldown;
    public bool moving;
    public bool attacking;
    public bool backFromAttacking;
    public int posIndex;
    public float initScale;
    protected private float defaultSpeed;
    public AudioSource AudioSource;

    private void Awake()
    {
        initScale = transform.localScale.x;
        idleCooldown = Mathf.Infinity;
        defaultSpeed = speed;
        moving = true;
        currPos = movementPositions[0].transform.position;
    }
    void Start()
    {
        NextPositionIndex();
        AudioSource.pitch = Random.Range(0.95f, 1.05f);
    }

    protected virtual void FixedUpdate()
    {
        Debug.Log("in base FixedUpdate");
        Movement();
    }
    public void SetScale(int dir)
    {
        transform.localScale = new Vector3 (initScale*dir, initScale, initScale);
    }
    public virtual void Movement()
    {
        Debug.Log("in base movement");
        if (moving)
        {
            ResetSpeed();
            if (Vector3.Distance(transform.position, nextPos) <= arrivalThreshold)
            {
                if (idleCooldown > 1)
                {
                    idleCooldown = 0;
                    NextPositionIndex();
                }
                else
                {
                    idleCooldown += Time.deltaTime;
                }
            }
            else
            {
                transform.Translate(distance*Time.deltaTime*speed);
                if (distance.x > 0)
                {
                    SetScale(1);
                }
                else
                {
                    SetScale(-1);
                }

            }
        }
    }

    public virtual void NextPositionIndex()
    {
        currPos = transform.position;
        if (backFromAttacking)
        {
            backFromAttacking = false;
        }
        else
        {
            posIndex = (posIndex + 1) % movementPositions.Count;
        }

        nextPos = movementPositions[posIndex].transform.position;
        distanceCalculate(currPos, nextPos);
    }

    public void distanceCalculate(Vector3 _curr, Vector3 _next)
    {
        distance = (_next - _curr).normalized;
        moving = true;
    }

    public void ResetSpeed()
    {
        speed = defaultSpeed;
    }

}

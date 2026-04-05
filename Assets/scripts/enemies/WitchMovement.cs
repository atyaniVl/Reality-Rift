using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchMovement : MonoBehaviour
{
    Vector3 currPos, nextPos, distance;
    [Header("movement parameters")]
    public List<GameObject> movementPositions;
    //public float idleTime;
    //[SerializeField] protected float arrivalThreshold = 0.1f;
    //public float speed;
    //protected float idleCooldown;
    //public bool moving;
    //public bool attacking;
    //public bool backFromAttacking;
    int posIndex;
    float initScale;
    //protected private float defaultSpeed;

    //[SerializeField] AudioClip unHide;
    WitchAttack wizzardAttack;
    private void Awake()
    {
        initScale = transform.localScale.x;
        //moving = true;
        currPos = movementPositions[0].transform.position;
    }
    void Start()
    {
        wizzardAttack = GetComponent<WitchAttack>();
        transform.position = movementPositions[0].transform.position;
        WizzNextPositionIndex();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
    public void WizzNextPositionIndex()
    {
        posIndex = Random.Range(0, movementPositions.Count );
        nextPos = movementPositions[posIndex].transform.position;
        WizzMovement();
    }
    public void WizzMovement()
    {
        transform.position = nextPos;
        GetComponent<Animator>().SetTrigger("Unhide");
        GetComponent<Collider2D>().enabled = true;
        wizzardAttack.isAttacking = true;
    }
    /*public void PlayUnhideSound()
    {
        SoundManager.instance.PlaySound(unHide);
    }*/

}

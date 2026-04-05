using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizzardMovement : EnemyMovement
{
    // Start is called before the first frame update
   [SerializeField] AudioClip unHide;
    //WizzardAttack wizzardAttack;

    void Start()
    {
        //wizzardAttack = GetComponent<WizzardAttack>();
        moving = true;
        transform.position = movementPositions[0].transform.position;
        WizzNextPositionIndex();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void WizzNextPositionIndex()
    {
        posIndex = Random.Range(0,movementPositions.Count-1);
        nextPos = movementPositions[posIndex].transform.position;
        WizzMovement();
    }
    public  void WizzMovement()
    {
        transform.position = nextPos;
        GetComponent<Animator>().SetTrigger("Unhide");
        GetComponent<Collider2D>().enabled = true;
        //wizzardAttack.isAttacking = true;
    }
    public void PlayUnhideSound()
    {
        SoundManager.instance.PlaySound(unHide);
    }
}

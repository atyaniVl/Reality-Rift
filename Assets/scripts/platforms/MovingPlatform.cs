using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MovingPlatform : EnemyMovement
{
    public bool canMove { set; get; }
    //[SerializeField] AudioClip mechanicMoving;
    AudioSource audSus;
    [SerializeField] Lever lever;
    private void Awake()
    {
        initScale = transform.localScale.x;
        idleCooldown = Mathf.Infinity;
        defaultSpeed = speed;
        moving = true;
        canMove = true;
        currPos = movementPositions[0].transform.position;
        audSus = GetComponent<AudioSource>();
        if (audSus == null)
        {
            Debug.LogError("platforms audio source is null");
        }
    }

    void Start()
    {
        NextPositionIndex();
        if(lever is not null)
            AudioState(false);
        else
            AudioState(true);
    }

    protected override void FixedUpdate()
    {
        if (lever != null)
        {
            Debug.Log("lever != null");
                canMove = lever._switch;
        }
        else
            canMove = true;
        PlateformMovement();


    }
    private void PlateformMovement()
    {
        if (moving && canMove)
        {
            ResetSpeed();
            if (Vector3.Distance(transform.position, nextPos) <= arrivalThreshold)
            {
                if (idleCooldown > 1.5)
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

                transform.Translate(distance * Time.deltaTime * speed);

            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {   
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(this.transform);
        }
    }    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(null);
        }
    }
    public void AudioState(bool state)
    {
        if (state)
        {
            float pitchValue = Random.Range(0.9f, 1.2f);
            audSus.pitch = pitchValue;
            audSus.Play();
        }
        else
        {
            audSus.Stop();
        }
    }
}

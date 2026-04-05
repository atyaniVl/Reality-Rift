using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CircleCollider2D))]

public class Bush : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] AudioClip bush;
    private void Awake()
    {
        }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(bush!= null) SoundManager.instance.PlaySound_DiffPitching(bush);
        if (collision.CompareTag("Player"))
        {
            PlayerProperties playerProperties = collision.GetComponent<PlayerProperties>();
            if(playerProperties != null)
            {
                playerProperties.TakeDamage(damage);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerProperties playerProperties = collision.GetComponent<PlayerProperties>();
            if (playerProperties != null)
            {
                playerProperties.TakeDamage(damage);
            }
        }
    }
}

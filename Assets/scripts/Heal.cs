using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CircleCollider2D))]
public class Heal : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }
    [SerializeField] float HealAmount;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerProperties playerProperties = collision.gameObject.GetComponent<PlayerProperties>();
            if (playerProperties is not null)
            {
                playerProperties.TakeHeal(HealAmount);
                Destroy(this.gameObject);
            }
        }
    }
}

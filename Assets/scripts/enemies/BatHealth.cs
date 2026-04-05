using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatHealth : EnemyHealth
{
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        maxHealth = 100;
        health = maxHealth;
        selfDamageFactor = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Died()
    {
        Destroy(gameObject);
    }
}

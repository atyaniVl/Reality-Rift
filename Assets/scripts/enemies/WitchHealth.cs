using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchHealth : EnemyHealth
{
    void Start() 
    {
        animator = GetComponent<Animator>();
        maxHealth = 150;
        health = maxHealth;
    }
}

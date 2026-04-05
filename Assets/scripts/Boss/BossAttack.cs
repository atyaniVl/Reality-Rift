using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab; // The prefab of the projectile to spawn.

    [SerializeField]
    private GameObject specialObjectPrefab; // The prefab of the special object to spawn.

    [SerializeField]
    private List<Transform> projectileSpawnPositions; // A list of positions where projectiles will be spawned.

    [SerializeField]
    private Transform specialObjectSpawnPosition;

    [SerializeField]
    private float timeBetweenProjectiles = 0.5f;

    [SerializeField]
    private float delayBeforeSecondAttack = 1.0f;

    [SerializeField] AudioClip fireBallStart;

    public void SpawnProjectiles()
    {
        if (fireBallStart != null) SoundManager.instance.PlaySound(fireBallStart);
        int index = Random.Range(0, projectileSpawnPositions.Count-2);
        for (int i = 0; i < projectileSpawnPositions.Count; i++)
        {
            if (index == i) continue;
            if (projectileSpawnPositions[i] != null)
            {
                Instantiate(projectilePrefab, projectileSpawnPositions[i].position, Quaternion.identity);
            }
        }
    }


    public void ChainAttack()
    {
        SpawnProjectiles();
        if (specialObjectSpawnPosition != null)
        {
            //Instantiate(specialObjectPrefab, specialObjectSpawnPosition.position, Quaternion.identity);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script manages the pool of projectiles for better performance
public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance { get; private set; }

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int poolSize = 10;

    // New prefab and size for the duck projectiles
    [SerializeField] private GameObject duckProjectilePrefab;
    [SerializeField] private int duckPoolSize = 10;

    private Queue<GameObject> pooledProjectiles = new Queue<GameObject>();
    private Queue<GameObject> pooledDuckProjectiles = new Queue<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialize the standard projectile pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.transform.parent = transform;
            projectile.SetActive(false);
            pooledProjectiles.Enqueue(projectile);
        }

        // Initialize the duck projectile pool
        for (int i = 0; i < duckPoolSize; i++)
        {
            GameObject duckProjectile = Instantiate(duckProjectilePrefab);
            duckProjectile.transform.parent = transform;
            duckProjectile.SetActive(false);
            pooledDuckProjectiles.Enqueue(duckProjectile);
        }
    }

    public GameObject GetProjectile()
    {
        if (pooledProjectiles.Count > 0)
        {
            GameObject projectile = pooledProjectiles.Dequeue();
            projectile.SetActive(true);
            return projectile;
        }
        else
        {
            Debug.LogWarning("Projectile pool is empty! Consider increasing the pool size.");
            return null;
        }
    }

    public void ReturnProjectile(GameObject projectile)
    {
        projectile.SetActive(false);
        pooledProjectiles.Enqueue(projectile);
    }

    // New method to get a duck projectile from its dedicated pool
    public GameObject GetDuckProjectile()
    {
        if (pooledDuckProjectiles.Count > 0)
        {
            GameObject duckProjectile = pooledDuckProjectiles.Dequeue();
            duckProjectile.SetActive(true);
            return duckProjectile;
        }
        else
        {
            Debug.LogWarning("Duck projectile pool is empty! Consider increasing the duck pool size.");
            return null;
        }
    }

    // New method to return a duck projectile to its dedicated pool
    public void ReturnDuckProjectile(GameObject projectile)
    {
        projectile.SetActive(false);
        pooledDuckProjectiles.Enqueue(projectile);
    }
}

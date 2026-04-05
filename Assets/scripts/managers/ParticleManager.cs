using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    //ParticleManager.Instance.PlayParticleEffect("Particle System", transform.position, 1);
    public static ParticleManager Instance;

    [SerializeField]
    private List<GameObject> particlePrefabs;

    private Dictionary<string, GameObject> particleEffects = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep the manager alive between scenes.
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        foreach (GameObject prefab in particlePrefabs)
        {
            if (prefab != null)
            {
                particleEffects[prefab.name] = prefab;
            }
        }
    }

    public void PlayParticleEffect(string particleName, Vector3 position, float duration = 1f)
    {
        if (particleEffects.ContainsKey(particleName))
        {
            Debug.Log("particle system will play");
            GameObject effectPrefab = particleEffects[particleName];
            GameObject effectInstance = Instantiate(effectPrefab, position, Quaternion.identity);

            ParticleSystem ps = effectInstance.GetComponent<ParticleSystem>();

            if (ps != null)
            {
                ps.Play();

                float destroyTime = duration > 0 ? duration : ps.main.duration;
                StartCoroutine(DestroyAfterTime(effectInstance, destroyTime));
            }
            else
            {
                Debug.LogWarning("ParticleManager: The prefab '" + particleName + "' does not have a ParticleSystem component.");
                Destroy(effectInstance);
            }
        }
        else
        {
            Debug.LogError("ParticleManager: Particle effect '" + particleName + "' not found in the list.");
        }
    }

    private IEnumerator DestroyAfterTime(GameObject objToDestroy, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(objToDestroy);
    }
}
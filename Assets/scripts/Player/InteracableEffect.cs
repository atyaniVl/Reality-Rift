using UnityEngine;
using Unity.Cinemachine;

public class InteracableEffect : MonoBehaviour
{
    [SerializeField] CinemachineCamera cinemachineCamera;

    public static event System.Action OnPlayerEnteredBossArea;

    [SerializeField] AudioClip heal, death, stability, fight;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("heal"))
        {
            GetComponent<PlayerProperties>().TakeHeal(30);
            ParticleManager.Instance.PlayParticleEffect("heal ps", transform.position, 0.5f);
            if (heal != null) SoundManager.instance.PlaySound_DiffPitching(heal);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("death"))
        {
            GetComponent<PlayerProperties>().TakeDamage(50);
            ParticleManager.Instance.PlayParticleEffect("death ps", transform.position, 0.5f);
            if (death != null) SoundManager.instance.PlaySound_DiffPitching(death);

            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("stability"))
        {
            GetComponent<PlayerInstabilities>().Instability += 30;
            ParticleManager.Instance.PlayParticleEffect("magic ps", transform.position, 0.5f);
            if (stability != null) SoundManager.instance.PlaySound_DiffPitching(stability);

            Destroy(collision.gameObject);
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("boss door"))
        {
            if (fight != null) SoundManager.instance.PlaySound_DiffPitching(fight);
            collision.isTrigger = false;
            Debug.Log("Player has entered the boss area!");
            cinemachineCamera.Lens.OrthographicSize = 20;
            OnPlayerEnteredBossArea?.Invoke();
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Boss : MonoBehaviour
{
    [SerializeField] Animator bossAnimator;
    [SerializeField] AudioClip fightMusic, normalMusic;
    [SerializeField] AudioSource musicSource;
    [SerializeField] private float transitionDuration = 1.0f;
    [SerializeField] GameObject bossHealthBar;
    [SerializeField] PlayerInstabilities playerInstabilities;
    [SerializeField] BossAttack bossAttack;



    public int phase = 0;

     bool alive = true;
    private void OnEnable()
    {
        // Subscribe to the event from the Player script.
        InteracableEffect.OnPlayerEnteredBossArea += OnRoomEnter;
        Debug.Log("Boss is listening for the player to enter the boss area.");
    }

    public void OnRoomEnter()
    {


        if (bossHealthBar == null)
        {
            Debug.LogError("Boss.OnRoomEnter → BossHealthBar reference is missing!");
            return;
        }

        bossHealthBar.SetActive(true);
        StartFight();
        phase = 1;
    }

    public void StartFight()
    {
        StartMusicTransition();

    }
    private IEnumerator FightCoroutine()
    {
        
        while (alive)
        {
            if (1 == phase)
            {
                yield return new WaitForSeconds(4);
                Debug.Log("BossAttackOne.");

                bossAttack.SpawnProjectiles();
            }
            else if (2 == phase)
            {
                yield return new WaitForSeconds(2.5f);
                Debug.Log("BossAttackTwo.");

                bossAttack.ChainAttack();
            }
            else if (3 == phase)
            {
                alive = false;
                Debug.Log("fight ends.");
                if (bossAnimator != null) bossAnimator.SetTrigger("die");

                playerInstabilities.InstabilityChange(+100);
                //playerWinScene
                yield return new WaitForSeconds(5);
                gameObject.SetActive(false);
                int index = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1;
                if (index < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
                    UI_Manager.instance.StartCoroutine(UI_Manager.instance.Transition_in(index));


            }
            if (bossAnimator != null) bossAnimator.SetTrigger("attack");
        }
        yield return null;
    }
    public void StartMusicTransition()
    {
        if (musicSource == null)
        {
            Debug.LogError("MusicSource is not assigned!");
            return;
        }

        if (fightMusic == null)
        {
            Debug.LogError("FightMusic clip is not assigned!");
            return;
        }


        StartCoroutine(TransitionMusic());
    }

    private IEnumerator TransitionMusic()
    {
        float startVolume = musicSource.volume;

        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        musicSource.volume = 0f;

        musicSource.clip = fightMusic;

        musicSource.Play();

        elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            musicSource.volume = Mathf.Lerp(0f, startVolume, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        musicSource.volume = startVolume;
        StartCoroutine(FightCoroutine());
    }
}

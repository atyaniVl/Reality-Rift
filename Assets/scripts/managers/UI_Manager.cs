using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;
using Unity.VisualScripting;


public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;
    [Header("Button transition")]

    [SerializeField] Color normal, hover, click;
    [SerializeField] AudioClip hoverSFX, clickSFX;
    [Header("menu")]
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject player;
    [SerializeField] GameObject gameOver;
    [SerializeField] GameObject winMenu;
    [Header("transition")]
    [SerializeField] float transitionSpeed;
    [SerializeField] float offset;
    [SerializeField] Image transitionPanel;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }   
        else
        {
            Destroy(instance);
        }
    }
    void Start()
    {
        transitionPanel.transform.localPosition = Vector3.zero;
        StartCoroutine(Transition_out());
        GameOverMenu(false);
        PauseMenu(false);
        WinMenu(false);
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape) && pauseMenu != null)
        {
            PauseMenu(!pauseMenu.activeSelf);
            float pitch = pauseMenu.activeSelf ? 0.8f : 1.2f;
            SoundManager.instance.PlaySound_SpecPitching(clickSFX, pitch);
        }
    }
    public void PauseMenu(bool state)
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(state);
            player.GetComponent<PlayerAttack>().enabled = !state;
            player.GetComponent<PlayerMovement>().enabled = !state;
            player.GetComponent<PlayerProperties>().enabled = !state;
            Time.timeScale = state ? 0f : 1f;
        }
    }
    public void GameOverMenu(bool state)
    {
        if(gameOver != null)
        {
            gameOver.SetActive(state);
        }
    }
    public void WinMenu(bool state)
    {
        if(winMenu != null)
        {
            winMenu.SetActive(state);
        }
    }
    public void HoverEnter(TextMeshProUGUI btnTMPro)
    {
        btnTMPro.color = hover;
        SoundManager.instance.PlaySound_SpecPitching(hoverSFX, 1.2f);
    }
    public void HoverExit(TextMeshProUGUI btnTMPro)
    {
        btnTMPro.color = normal;
        SoundManager.instance.PlaySound_SpecPitching(hoverSFX, 0.8f);
    }
    public void HoldClick(TextMeshProUGUI btnTMPro)
    {
        btnTMPro.color = click;
        SoundManager.instance.PlaySound(clickSFX);
    }
    public void Quit()
    {
        PlayerPrefs.DeleteKey("CheckpointSaved"); // Delete the flag
        PlayerPrefs.DeleteKey("CheckpointX");
        PlayerPrefs.DeleteKey("CheckpointY");
        PlayerPrefs.DeleteKey("CheckpointZ");
        PlayerPrefs.DeleteKey("CheckpointInstability");
        PlayerPrefs.DeleteKey("CheckpointLives");
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Debug.LogWarning("StopGame called, but this only works in the Unity Editor.");
#endif
        Application.Quit();
    }
    public void Play()
    {
        StartCoroutine(Transition_in(1));
    }
    public void Options()
    {
        /*
         * show options panel
         * hide main menu or pause menu
         */
    }
    public void Restart()
    {
        PlayerPrefs.DeleteKey("CheckpointSaved"); // Delete the flag
        PlayerPrefs.DeleteKey("CheckpointX");
        PlayerPrefs.DeleteKey("CheckpointY");
        PlayerPrefs.DeleteKey("CheckpointZ");
        PlayerPrefs.DeleteKey("CheckpointInstability");
        PlayerPrefs.DeleteKey("CheckpointLives");
        Time.timeScale = 1f;
        StartCoroutine(Transition_in(2));
    }    public void MainMenu()
    {
        PlayerPrefs.DeleteKey("CheckpointSaved"); // Delete the flag
        PlayerPrefs.DeleteKey("CheckpointX");
        PlayerPrefs.DeleteKey("CheckpointY");
        PlayerPrefs.DeleteKey("CheckpointZ");
        PlayerPrefs.DeleteKey("CheckpointInstability");
        PlayerPrefs.DeleteKey("CheckpointLives");
        Debug.Log("main menu");
        Time.timeScale = 1f;
        StartCoroutine(Transition_in(0));
    }
    public IEnumerator Transition_in(int sceneIndex)
    {
        transitionPanel.transform.localPosition = new Vector3(-offset, 0, 0);
        while (transitionPanel.transform.localPosition.x < 0)
        {
            transitionPanel.transform.localPosition += new Vector3(Time.deltaTime * transitionSpeed, 0, 0);
            yield return null;
        }
        transitionPanel.transform.localPosition = Vector3.zero;
        /*
         * main menu -> game
         * game -> main menu
         * restart
         */
        SceneManager.LoadSceneAsync(sceneIndex);
    }
    public IEnumerator Transition_out()
    {
        transitionPanel.transform.localPosition = Vector3.zero;
        while (transitionPanel.transform.localPosition.x < offset)
        {
            transitionPanel.transform.localPosition += new Vector3(Time.deltaTime * transitionSpeed, 0, 0);
            yield return null;
        }
        transitionPanel.transform.localPosition = new Vector3(offset, 0, 0);
    }
}

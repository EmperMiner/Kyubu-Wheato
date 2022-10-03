using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private int MainMenu = 0;
    private int LoadingGameToMenu = 2;

    public static bool GameIsPaused = false;

    private AudioManager AudioPlayer;
    private PlayerController player;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject ingameSettingsUI;
    private SettingsMenu SettingsScript;
    [SerializeField] private GameObject HUD;

    private static Action onIngameLoaderCallback;

    private void Start() 
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        SettingsScript = ingameSettingsUI.GetComponent<SettingsMenu>();
        SettingsScript.StartingSettings();

    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape) && player.playerAlive == true)
        {
            if (GameIsPaused)
            {
                ResumeGame();
            } 
            else
            {
                PauseGame();
            }
        }
    }


    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        HUD.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        ingameSettingsUI.SetActive(false);
        HUD.SetActive(true);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void ButtonSelect()
    {
        FindObjectOfType<AudioManager>().PlaySound("UIButtonPress");
    }

    public void BackToMainMenu()
    {
        onIngameLoaderCallback = () => { SceneManager.LoadScene(MainMenu); };

        SceneManager.LoadScene(LoadingGameToMenu);
    }

    public static void IngameLoaderCallback()
    {
        if (onIngameLoaderCallback!=null)
        {
            onIngameLoaderCallback();
            onIngameLoaderCallback = null;
        }
    }   
}


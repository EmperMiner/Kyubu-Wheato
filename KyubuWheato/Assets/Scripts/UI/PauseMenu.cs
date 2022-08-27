using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private int MainMenu = 0;
    private int LoadingGameToMenu = 2;
    private int Level_2 = 3;

    public static bool GameIsPaused = false;

    private PlayerController player;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject HUD;

    private static Action onIngameLoaderCallback;

    private void Start() 
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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
        HUD.SetActive(true);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void SettingsInGameMenu()
    {
        
    }

    public void Restart()
    {
        onIngameLoaderCallback = () => { SceneManager.LoadScene(Level_2); };

        SceneManager.LoadScene(LoadingGameToMenu);
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


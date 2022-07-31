using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private int MainMenu = 0;

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject HUD;

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(MainMenu);
    }

    

    
}


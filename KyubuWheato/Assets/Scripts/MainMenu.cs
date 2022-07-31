using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    private int Level_2 = 1;

    public void StartGame()
    {
        SceneManager.LoadScene(Level_2);
    }

    public void SettingsMenu()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

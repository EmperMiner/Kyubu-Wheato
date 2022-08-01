using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    private int LoadingMenuToGame = 1;
    private int Level_2 = 3;

    private static Action onLoaderCallback;

    public void StartGame()
    {
        onLoaderCallback = () => { SceneManager.LoadScene(Level_2); };

        SceneManager.LoadScene(LoadingMenuToGame);
    }

    public void SettingsMenu()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    public static void LoaderCallback()
    {
        if (onLoaderCallback!=null)
        {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }
}

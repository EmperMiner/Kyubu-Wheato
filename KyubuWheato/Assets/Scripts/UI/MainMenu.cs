using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioManager AudioPlayer;
    private int LoadingMenuToGame = 1;
    private int Level_1 = 4;

    private static Action onLoaderCallback;
    
    public void StartGame()
    {
        StartCoroutine(StartGameDelay());
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

    public void ButtonSelect()
    {
        AudioPlayer.PlaySound("UIButtonPress");
    }

    public void ButtonStart()
    {
        AudioPlayer.PlaySound("UIStart");
    }

    IEnumerator StartGameDelay()
    {
        yield return new WaitForSeconds(1f);

        onLoaderCallback = () => { SceneManager.LoadScene(Level_1); };

        SceneManager.LoadScene(LoadingMenuToGame);

        yield return null;
    }
}

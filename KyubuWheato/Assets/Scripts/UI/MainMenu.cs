using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioManager AudioPlayer;
    [SerializeField] private GameObject ConfirmationPanel;
    [SerializeField] private TextMeshProUGUI LevelNumber; 
    private int LoadingMenuToGame = 1;
    private int Level_1 = 4;
    private int Tutorial_Scene = 16;

    private static Action onLoaderCallback;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("SavedLevel")) { PlayerPrefs.SetInt("SavedLevel", 1); }
        int check = PlayerPrefs.GetInt("SavedLevel");
        if (check == 0) { PlayerPrefs.SetInt("SavedLevel", 1); }

        if (!PlayerPrefs.HasKey("DisableTutorial")) { PlayerPrefs.SetInt("DisableTutorial", 0); }
    }

    private void Start()
    {
        ConfirmationPanel.SetActive(false);
        if (!PlayerPrefs.HasKey("SavedLevel")) { PlayerPrefs.SetInt("SavedLevel", 1); }
        int check = PlayerPrefs.GetInt("SavedLevel");
        if (check == 0) { PlayerPrefs.SetInt("SavedLevel", 1); }
    }

    public void LoadGame()
    {
        int load = PlayerPrefs.GetInt("SavedLevel");
        if (load == 1) { StartGame(); }
        else { StartCoroutine(LoadGameDelay(load)); }
    }
    
    public void StartGame() { StartCoroutine(StartGameDelay()); }

    public void QuitGame() { Application.Quit(); }

    public void CheckStartGame()
    {
        int check = PlayerPrefs.GetInt("SavedLevel");
        if (check == 1) { StartGame(); }
        else { OpenConfirmation(); }
    }

    private void OpenConfirmation()
    {
        ConfirmationPanel.SetActive(true);
        int levelsaved = PlayerPrefs.GetInt("SavedLevel");
        LevelNumber.text = "Level " + levelsaved;
        if (levelsaved == 12) { LevelNumber.text = "Level bW[f8e04_M-#LR>tF"; }
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
        if (PlayerPrefs.GetInt("DisableTutorial") == 1) { Tutorial_Scene = 4; }
        yield return new WaitForSeconds(1f);

        onLoaderCallback = () => { SceneManager.LoadScene(Tutorial_Scene); PlayerPrefs.SetInt("SavedLevel", 1); };

        SceneManager.LoadScene(LoadingMenuToGame);

        yield return null;
    }

    public void StartGame(bool ShowTutorial)
    {
        if (ShowTutorial == false) { PlayerPrefs.SetInt("DisableTutorial", 1); }
        StartCoroutine(TutorialToGameDelay());
    }

    IEnumerator TutorialToGameDelay()
    {
        yield return new WaitForSeconds(1f);

        onLoaderCallback = () => { SceneManager.LoadScene(Level_1); PlayerPrefs.SetInt("SavedLevel", 1); };

        SceneManager.LoadScene(LoadingMenuToGame);

        yield return null;
    }

    IEnumerator LoadGameDelay(int levelWanted)
    {
        yield return new WaitForSeconds(1f);

        onLoaderCallback = () => { SceneManager.LoadScene(levelWanted + 3); };

        SceneManager.LoadScene(LoadingMenuToGame);

        yield return null;
    }
}

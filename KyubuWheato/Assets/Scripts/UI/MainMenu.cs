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

    private static Action onLoaderCallback;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("SavedLevel")) { PlayerPrefs.SetInt("SavedLevel", 1); }
        int check = PlayerPrefs.GetInt("SavedLevel");
        if (check == 0) { PlayerPrefs.SetInt("SavedLevel", 1); }
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

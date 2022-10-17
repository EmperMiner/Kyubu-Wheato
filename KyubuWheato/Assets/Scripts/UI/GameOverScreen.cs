using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TextMeshProUGUI WheatGainedText;
    [SerializeField] private GameObject HUD;

    private int MainMenu = 0;
    private int GameOverGameToMenu = 3;
    private int Level_1 = 4;

    private static Action onIngameLoaderCallback;

    private void Start()
    {
        gameOverScreen.SetActive(false);
        HUD.SetActive(true);
    }

    public void GameOverTrigger(int WheatGained, bool victory)
    {
        HUD.SetActive(false);
        if (victory) { WheatGainedText.text = "Wheats Harvested:" + WheatGained.ToString() + " + 1000 Bonus"; }
        else { WheatGainedText.text = "Wheats Harvested:" + WheatGained.ToString(); }
        gameOverScreen.SetActive(true);
    }

    public void ButtonSelect()
    {
        FindObjectOfType<AudioManager>().PlaySound("UIButtonPress");
    }

    public void Restart()
    {
        SceneManager.LoadScene(Level_1);
    }

    public void BackToMainMenu()
    {
        onIngameLoaderCallback = () => { SceneManager.LoadScene(MainMenu); };

        SceneManager.LoadScene(GameOverGameToMenu);
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

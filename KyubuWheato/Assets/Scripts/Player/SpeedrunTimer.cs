using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class SpeedrunTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float time;

    private void Start()
    {
        timerText = GameObject.FindGameObjectWithTag("Timer").GetComponent<TextMeshProUGUI>();
        if (SceneManager.GetActiveScene().buildIndex == 14) {  }

        UpdateTimerVisibility();
    
        int level = SceneManager.GetActiveScene().buildIndex - 4;
        if (level == 0) 
        { 
            PlayerPrefs.SetFloat("Timer", 0);
            time = 0;
        }
        else { time = PlayerPrefs.GetFloat("Timer"); }
    }

    void Update() 
    {    
        time += Time.deltaTime;
        UpdateTimer(0); 
    }
    
    public void UpdateTimerVisibility()
    {
        // Sets transparency of timer based on time settings
        timerText.color = new Color32(255, 255, 255, Convert.ToByte(255*PlayerPrefs.GetInt("ShowTimerToggle")));
    }

    public void UpdateTimer(int timerIndex)
    {
        int hours = ((int) time/3600);
        int minutes = ((int) time/60)%60;
        float seconds = (time % 60);

        string extraHourZero;
        if (hours < 10) { extraHourZero = "0"; }
        else { extraHourZero = ""; }

        string extraMinuteZero;
        if (minutes < 10) { extraMinuteZero = "0"; }
        else { extraMinuteZero = ""; }

        string extraSecondZero;
        if (seconds < 10f) { extraSecondZero = "0"; }
        else { extraSecondZero = ""; }

        if (timerIndex == 0) { timerText.text = extraHourZero + hours.ToString() + ":" + extraMinuteZero + minutes.ToString() + ":" + extraSecondZero + seconds.ToString("f3"); }
        else 
        { 
            TextMeshProUGUI WinningTimer = GameObject.FindGameObjectWithTag("EndingTimer").GetComponent<TextMeshProUGUI>();
            WinningTimer.text = extraHourZero + hours.ToString() + ":" + extraMinuteZero + minutes.ToString() + ":" + extraSecondZero + seconds.ToString("f3"); 
        }
        
    }

    public void SetEndingTime() { PlayerPrefs.SetFloat("Timer", time); }
}

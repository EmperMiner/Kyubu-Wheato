using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SpeedrunTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    public float time;
    void Start()
    {
        timerText.color = new Color32(255, 255, 255, 0);
        if (PlayerPrefs.GetInt("ShowTimerToggle") == 1) { timerText.color = new Color32(255, 255, 255, 255); }

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

        int hours = ((int) time/3600);
        int minutes = ((int) time/60)%60;
        float seconds = (time % 60);

        string extraHourZero;
        if (hours < 10) { extraHourZero = ""; }
        else { extraHourZero = "0"; }

        string extraMinuteZero;
        if (minutes < 10) { extraMinuteZero = ""; }
        else { extraMinuteZero = "0"; }

        string extraSecondZero;
        if (hours < 10) { extraSecondZero = ""; }
        else { extraSecondZero = "0"; }

        timerText.text = extraHourZero + hours.ToString() + ":" + extraMinuteZero + minutes.ToString() + ":" + extraSecondZero + seconds.ToString("f3");
    }
}

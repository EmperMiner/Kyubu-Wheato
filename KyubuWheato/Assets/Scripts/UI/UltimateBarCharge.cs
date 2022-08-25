using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class UltimateBarCharge : MonoBehaviour
{
    [SerializeField] private Slider ultimateChargeSlider;
    [SerializeField] private GameObject ultimateCharge;
    [SerializeField] private GameObject ultimateBorder;

    public int maxUltimateCharge = 200;
    public int currentUltimateCharge = 0;
    public bool ultimateInProgress = false;

    public bool havePizza;
    private bool started = false;

    private void Start()
    {
        LoadData();
        ultimateCharge.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        ultimateBorder.GetComponent<Image>().color = new Color32(224, 83, 83, 0);
    }

    private void Update() 
    {
        if (havePizza == true && started == false) { StartUltimateBar(); }
    }

    private void StartUltimateBar()
    {
        ultimateChargeSlider.maxValue = maxUltimateCharge;
        ultimateChargeSlider.value = 0;
        UltimateBarFlash(false);
        ultimateCharge.GetComponent<Image>().color = new Color32(255, 255, 255, 200);
        ultimateBorder.GetComponent<Image>().color = new Color32(224, 83, 83, 200);
        started = true;
    }

    public void IncreaseUltimateCharge(int UltimateChargeAmount)
    {
        if (ultimateInProgress == false)
        {
            currentUltimateCharge += UltimateChargeAmount;
            if (currentUltimateCharge > maxUltimateCharge) { currentUltimateCharge = maxUltimateCharge;}
            ultimateChargeSlider.value = currentUltimateCharge;
            StartCoroutine(FlashingUltimateBar());
        }
    } 

    IEnumerator FlashingUltimateBar()
    {
        UltimateBarFlash(true);
        yield return new WaitForSeconds(0.25f);
        UltimateBarFlash(false);
        yield return null;
    }

    public void UltimateBarFlash(bool flashIndex)
    {
        if (flashIndex) { ultimateCharge.GetComponent<Image>().color = new Color32(200, 200, 200, 255); }
        else { ultimateCharge.GetComponent<Image>().color = new Color32(255, 255, 255, 200); }
    }

    private void LoadData()
    {
        string json = File.ReadAllText(Application.dataPath + "/gameSaveData.json");
        PlayerData loadedPlayerData = JsonUtility.FromJson<PlayerData>(json);

        havePizza = loadedPlayerData.havePizza;
    }   

    private class PlayerData
    {
        public bool havePizza;
    }
}

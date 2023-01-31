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
    private bool rainbowStarted;
    private PlayerController player;

    public int maxUltimateCharge;
    public int currentUltimateCharge;
    public bool ultimateInProgress = false;

    public bool havePizza;
    private bool started = false;
    public Color32[] colors;

    private void Awake()
    {
        maxUltimateCharge = 800;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        UltimateLoadData();
        ultimateCharge.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        ultimateBorder.GetComponent<Image>().color = new Color32(224, 83, 83, 0);
        rainbowStarted = false;

        colors = new Color32[7]
        {
            new Color32(255, 0, 0, 255), //red
            new Color32(255, 165, 0, 255), //orange
            new Color32(255, 255, 0, 255), //yellow
            new Color32(0, 255, 0, 255), //green
            new Color32(0, 0, 255, 255), //blue
            new Color32(75, 0, 130, 255), //indigo
            new Color32(238, 130, 238, 255), //violet
        };

        ultimateChargeSlider.value = currentUltimateCharge;
    }

    private void Update() 
    {
        if (havePizza == true && started == false) { StartUltimateBar(); }
        if (currentUltimateCharge == maxUltimateCharge && rainbowStarted == false) { StartCoroutine(ColorCycle()); FindObjectOfType<AudioManager>().PlaySound("UltimateCharged"); player.StartCoroutine(player.UltNotifText()); }
        if (currentUltimateCharge < maxUltimateCharge) { rainbowStarted = false; }
    }

    private IEnumerator ColorCycle()
    {
        rainbowStarted = true;
        int i = 0;
        while(currentUltimateCharge == maxUltimateCharge)
        {
            for(float interpolant = 0f; interpolant < 1f; interpolant+= 0.01f)
            {
                ultimateCharge.GetComponent<Image>().color = Color.Lerp(colors[i%7], colors[(i+1)%7], interpolant);
                yield return null;
            }
            i++;
        }
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

    public void UltimateLoadData()
    {
        string json = File.ReadAllText(Application.dataPath + "/ingameSaveData.json");
        PlayerData loadedPlayerData = JsonUtility.FromJson<PlayerData>(json);

        havePizza = loadedPlayerData.havePizza;
    }   

    private class PlayerData
    {
        public bool havePizza;
    }
}

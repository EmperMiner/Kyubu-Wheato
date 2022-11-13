using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class ShopManager : MonoBehaviour
{
    private float shopMoveSpeed;
    private int shopMaxHealth;
    private int shopPlayerHealth;
    private float shopStrength;
    private int shopWheat;
    private int shopDiceNumber;
    private float shopPlayerCooldownTime;
    private float shopDefense;
    private float shopWheatDroprate;
    private int shopDicePreviewerLevel;
    private int shopDiceDroprate;
    private bool shopHavePizza;
    private bool shopHaveCarrotCake;
    private bool shopHaveFlan;
    private bool shopHaveCremeBrulee;
    private bool shopHaveBanhmi;
    private bool shopHaveCupcake;
    private bool shopHaveChickenNuggets;
    private bool shopHavePastelDeChoclo;
    private bool shopHaveGarlicBread;
    private bool shopHaveHornScallop;

    private AudioManager AudioPlayer;
    [SerializeField] private Text ShopWheatCounterNumber;
    [SerializeField] private int[] upgradePrices;
    [SerializeField] private Text[] upgradePriceText;
    [SerializeField] private Image[] UpgradesImage;
    [SerializeField] private Sprite[] UpgradesSpritesVariants;
    [SerializeField] private Text NotifText;
    [SerializeField] private SettingsMenu SettingsScript;

    private bool NotifTextBuySuccessDisplayed = false;
    private bool NotifTextNotEnoughMoneyDisplayed = false;
    private bool NotifTextAlreadyBoughtDisplayed = false;
    private bool NotifTextHaveNotBoughtDisplayed = false;

    [SerializeField] private GameObject trophy;
    [SerializeField] private TextMeshProUGUI winsText;
    [SerializeField] private GameObject NewEntreeScreen;

    private string MaxBought = "Maxed";

    private int refundLoops;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("WinCounter")) { PlayerPrefs.SetInt("WinCounter", 0); }
        if (!PlayerPrefs.HasKey("NewEntreeScreen")) { PlayerPrefs.SetInt("NewEntreeScreen", 0); }
        if (!PlayerPrefs.HasKey("Ramen")) { PlayerPrefs.SetInt("Ramen", 0); }
        if (!PlayerPrefs.HasKey("Salmon")) { PlayerPrefs.SetInt("Salmon", 0); }
        if (!PlayerPrefs.HasKey("Steak")) { PlayerPrefs.SetInt("Steak", 0); }
    }

    private void Start()
    {
        trophy.SetActive(false);
        int i = PlayerPrefs.GetInt("WinCounter");
        if (i > 0) { DisplayTrophy(i); }

        

        Time.timeScale = 1f;
        LoadData();
        AudioPlayer = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        SettingsScript.StartingSettings();
        UpdateUpgradeUI(420);
        NotifText.text = "";

        if (PlayerPrefs.GetInt("NewEntreeScreen") == 1)
        {
            PlayerPrefs.SetInt("NewEntreeScreen", 0);
            NewEntreeScreen.SetActive(true);
            AudioPlayer.PlaySound("UnlockEntree");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            shopWheat += 10000;
            ShopWheatCounterNumber.text = shopWheat.ToString();
            SaveData();
        } else if (Input.GetKeyDown(KeyCode.R))
        {
            shopWheat -= 946;
            ShopWheatCounterNumber.text = shopWheat.ToString();
            SaveData();
        }

        if (Input.GetKeyDown(KeyCode.T)) { PlayerPrefs.SetInt("Ramen", 0); }
        if (Input.GetKeyDown(KeyCode.Y)) { PlayerPrefs.SetInt("Salmon", 0); }
        if (Input.GetKeyDown(KeyCode.U)) { PlayerPrefs.SetInt("Steak", 0); }
        if (Input.GetKeyDown(KeyCode.I)) { PlayerPrefs.SetInt("WinCounter", 0); }
        if (Input.GetKeyDown(KeyCode.O)) { PlayerPrefs.SetInt("Steak", 1); }
    } 
 
    public void Buy(int UpgradeValue)
    {
        if (UpgradeValue == 0)
        {
            if (shopStrength == 1.0 && shopWheat >= upgradePrices[0]) { shopStrength = 1.5f; shopWheat -= upgradePrices[0]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopStrength == 1.5 && shopWheat >= upgradePrices[1]) { shopStrength = 2f; shopWheat -= upgradePrices[1]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopStrength == 2 && shopWheat >= upgradePrices[2]) { shopStrength = 2.5f; shopWheat -= upgradePrices[2]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopStrength == 2.5 && shopWheat >= upgradePrices[3]) { shopStrength = 3f; shopWheat -= upgradePrices[3]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopStrength == 3 && shopWheat >= upgradePrices[4]) { shopStrength = 5f; shopWheat -= upgradePrices[4]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopStrength == 5) { StartCoroutine(NotifTextAlreadyBought()); }
            else { StartCoroutine(NotifTextNotEnoughMoney()); } 
        }
        if (UpgradeValue == 1)
        {
            if (shopDefense == 1.0 && shopWheat >= upgradePrices[0]) { shopDefense = 1.5f; shopWheat -= upgradePrices[0]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopDefense == 1.5 && shopWheat >= upgradePrices[1]) { shopDefense = 2f; shopWheat -= upgradePrices[1]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopDefense == 2 && shopWheat >= upgradePrices[2]) { shopDefense = 3f; shopWheat -= upgradePrices[2]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopDefense == 3 && shopWheat >= upgradePrices[3]) { shopDefense = 4f; shopWheat -= upgradePrices[3]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopDefense == 4 && shopWheat >= upgradePrices[4]) { shopDefense = 5f; shopWheat -= upgradePrices[4]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopDefense == 5) { StartCoroutine(NotifTextAlreadyBought()); }
            else { StartCoroutine(NotifTextNotEnoughMoney()); }  
        }
        if (UpgradeValue == 2)
        {
            if (shopMaxHealth <= 30 && shopWheat >= upgradePrices[0]) { shopMaxHealth = 50; shopWheat -= upgradePrices[0]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopMaxHealth == 50 && shopWheat >= upgradePrices[1]) { shopMaxHealth = 90; shopWheat -= upgradePrices[1]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopMaxHealth == 90 && shopWheat >= upgradePrices[2]) { shopMaxHealth = 140; shopWheat -= upgradePrices[2]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopMaxHealth == 140 && shopWheat >= upgradePrices[3]) { shopMaxHealth = 200; shopWheat -= upgradePrices[3]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopMaxHealth == 200 && shopWheat >= upgradePrices[4]) { shopMaxHealth = 300; shopWheat -= upgradePrices[4]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopMaxHealth >= 300) { StartCoroutine(NotifTextAlreadyBought()); }
            else { StartCoroutine(NotifTextNotEnoughMoney()); }
        }
        if (UpgradeValue == 3)
        {
            if (shopMoveSpeed < 2.51 && shopWheat >= upgradePrices[0]) { shopMoveSpeed = 2.75f; shopWheat -= upgradePrices[0]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopMoveSpeed >= 2.74 && shopMoveSpeed < 2.76 && shopWheat >= upgradePrices[1]) { shopMoveSpeed = 3f; shopWheat -= upgradePrices[1]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopMoveSpeed >= 2.99 && shopMoveSpeed < 3.01 && shopWheat >= upgradePrices[2]) { shopMoveSpeed = 3.25f; shopWheat -= upgradePrices[2]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopMoveSpeed >= 3.24 && shopMoveSpeed < 3.26 && shopWheat >= upgradePrices[3]) { shopMoveSpeed = 3.5f; shopWheat -= upgradePrices[3]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopMoveSpeed >= 3.49 && shopMoveSpeed < 3.51 && shopWheat >= upgradePrices[4]) { shopMoveSpeed = 4f; shopWheat -= upgradePrices[4]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopMoveSpeed >= 3.99) { StartCoroutine(NotifTextAlreadyBought()); }
            else { StartCoroutine(NotifTextNotEnoughMoney()); } 
        }
        if (UpgradeValue == 4)
        {
            if (shopPlayerCooldownTime >= 10.0 && shopWheat >= upgradePrices[0]) { shopPlayerCooldownTime = 9.0f; shopWheat -= upgradePrices[0]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopPlayerCooldownTime == 9.0 && shopWheat >= upgradePrices[1]) { shopPlayerCooldownTime = 8.0f; shopWheat -= upgradePrices[1]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopPlayerCooldownTime == 8.0 && shopWheat >= upgradePrices[2]) { shopPlayerCooldownTime = 7.0f; shopWheat -= upgradePrices[2]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopPlayerCooldownTime == 7.0 && shopWheat >= upgradePrices[3]) { shopPlayerCooldownTime = 6.0f; shopWheat -= upgradePrices[3]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopPlayerCooldownTime == 6.0 && shopWheat >= upgradePrices[4]) { shopPlayerCooldownTime = 5.0f; shopWheat -= upgradePrices[4]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopPlayerCooldownTime <= 5.0) { StartCoroutine(NotifTextAlreadyBought()); }
            else { StartCoroutine(NotifTextNotEnoughMoney()); } 
        }
        if (UpgradeValue == 5)
        {
            if (shopDiceNumber == 6 && shopWheat >= upgradePrices[0]) { shopDiceNumber = 9; shopWheat -= upgradePrices[0]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopDiceNumber == 9 && shopWheat >= upgradePrices[1]) { shopDiceNumber = 12; shopWheat -= upgradePrices[1]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopDiceNumber == 12 && shopWheat >= upgradePrices[2]) { shopDiceNumber = 18; shopWheat -= upgradePrices[2]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopDiceNumber == 18 && shopWheat >= upgradePrices[3]) { shopDiceNumber = 24; shopWheat -= upgradePrices[3]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopDiceNumber == 24 && shopWheat >= upgradePrices[4]) { shopDiceNumber = 30; shopWheat -= upgradePrices[4]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopDiceNumber == 30) { StartCoroutine(NotifTextAlreadyBought()); }
            else { StartCoroutine(NotifTextNotEnoughMoney()); } 
        }
        if (UpgradeValue == 6)
        {
            if (shopWheatDroprate == 30.0 && shopWheat >= upgradePrices[0]) { shopWheatDroprate = 40.0f; shopWheat -= upgradePrices[0]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopWheatDroprate == 40.0 && shopWheat >= upgradePrices[1]) { shopWheatDroprate = 50.0f; shopWheat -= upgradePrices[1]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopWheatDroprate == 50.0 && shopWheat >= upgradePrices[2]) { shopWheatDroprate = 60.0f; shopWheat -= upgradePrices[2]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopWheatDroprate == 60.0 && shopWheat >= upgradePrices[3]) { shopWheatDroprate = 70.0f; shopWheat -= upgradePrices[3]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopWheatDroprate == 70.0 && shopWheat >= upgradePrices[4]) { shopWheatDroprate = 80.0f; shopWheat -= upgradePrices[4]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopWheatDroprate == 80.0) { StartCoroutine(NotifTextAlreadyBought()); }              
            else { StartCoroutine(NotifTextNotEnoughMoney()); } 
        }
        if (UpgradeValue == 7)
        {
            if (shopDicePreviewerLevel == 0 && shopWheat >= upgradePrices[0]) { shopDicePreviewerLevel = 1; shopWheat -= upgradePrices[0]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopDicePreviewerLevel == 1 && shopWheat >= upgradePrices[1]) { shopDicePreviewerLevel = 2; shopWheat -= upgradePrices[1]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopDicePreviewerLevel == 2 && shopWheat >= upgradePrices[2]) { shopDicePreviewerLevel = 3; shopWheat -= upgradePrices[2]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopDicePreviewerLevel == 3 && shopWheat >= upgradePrices[3]) { shopDicePreviewerLevel = 4; shopWheat -= upgradePrices[3]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopDicePreviewerLevel == 4 && shopWheat >= upgradePrices[4]) { shopDicePreviewerLevel = 5; shopWheat -= upgradePrices[4]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopDicePreviewerLevel == 5) { StartCoroutine(NotifTextAlreadyBought()); }
            else { StartCoroutine(NotifTextNotEnoughMoney()); }
        }
        if (UpgradeValue == 8)
        {
            if (shopDiceDroprate == 1000 && shopWheat >= upgradePrices[0]) { shopDiceDroprate = 500; shopWheat -= upgradePrices[0]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopDiceDroprate == 500 && shopWheat >= upgradePrices[1]) { shopDiceDroprate = 400; shopWheat -= upgradePrices[1]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopDiceDroprate == 400 && shopWheat >= upgradePrices[2]) { shopDiceDroprate = 300; shopWheat -= upgradePrices[2]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopDiceDroprate == 300 && shopWheat >= upgradePrices[3]) { shopDiceDroprate = 200; shopWheat -= upgradePrices[3]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopDiceDroprate == 200 && shopWheat >= upgradePrices[4]) { shopDiceDroprate = 100; shopWheat -= upgradePrices[4]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopDiceDroprate == 100) { StartCoroutine(NotifTextAlreadyBought()); }
            else { StartCoroutine(NotifTextNotEnoughMoney()); }
        }
        if (UpgradeValue == 101)
        {
            if (shopHavePizza == false && shopWheat >= upgradePrices[5]) { shopHavePizza = true; shopWheat -= upgradePrices[5]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopHavePizza == true) { StartCoroutine(NotifTextAlreadyBought()); }
            else { StartCoroutine(NotifTextNotEnoughMoney()); }  
        }
        if (UpgradeValue == 102)
        {
            if (shopHaveCarrotCake == false && shopWheat >= upgradePrices[5]) { shopHaveCarrotCake = true; shopWheat -= upgradePrices[5]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopHaveCarrotCake == true) { StartCoroutine(NotifTextAlreadyBought()); }
            else { StartCoroutine(NotifTextNotEnoughMoney()); }
        }
        if (UpgradeValue == 103)
        {
            if (shopHaveFlan == false && shopWheat >= upgradePrices[6]) { shopHaveFlan = true; shopWheat -= upgradePrices[6]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopHaveFlan == true) { StartCoroutine(NotifTextAlreadyBought()); }
            else { StartCoroutine(NotifTextNotEnoughMoney()); }
        }
        if (UpgradeValue == 104)
        {
            if (shopHaveFlan == false) { StartCoroutine(NotifTextNeedFlan()); }
            else if (shopHaveCremeBrulee == false && shopWheat >= upgradePrices[6]) { shopHaveCremeBrulee = true; shopWheat -= upgradePrices[6]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopHaveCremeBrulee == true) { StartCoroutine(NotifTextAlreadyBought()); }
            else { StartCoroutine(NotifTextNotEnoughMoney()); }
        }
        if (UpgradeValue == 105)
        {
            if (shopDicePreviewerLevel == 0) { StartCoroutine(NotifTextNeedDicePreviewer()); }
            else if (shopHaveBanhmi == false && shopWheat >= upgradePrices[5]) { shopHaveBanhmi = true; shopWheat -= upgradePrices[5]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopHaveBanhmi == true) { StartCoroutine(NotifTextAlreadyBought()); }
            else { StartCoroutine(NotifTextNotEnoughMoney()); }
        }
        if (UpgradeValue == 106)
        {
            if (shopHaveCupcake == false && shopWheat >= upgradePrices[6]) { shopHaveCupcake = true; shopWheat -= upgradePrices[6]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopHaveCupcake == true) { StartCoroutine(NotifTextAlreadyBought()); }
            else { StartCoroutine(NotifTextNotEnoughMoney()); }  
        }
        if (UpgradeValue == 107)
        {
            if (shopHaveChickenNuggets == false && shopWheat >= upgradePrices[7]) { shopHaveChickenNuggets = true; shopWheat -= upgradePrices[7]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopHaveChickenNuggets == true) { StartCoroutine(NotifTextAlreadyBought()); }
            else { StartCoroutine(NotifTextNotEnoughMoney()); }  
        }
        if (UpgradeValue == 108)
        {
            if (shopHavePastelDeChoclo == false && shopWheat >= upgradePrices[7]) { shopHavePastelDeChoclo = true; shopWheat -= upgradePrices[7]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopHavePastelDeChoclo == true) { StartCoroutine(NotifTextAlreadyBought()); }
            else { StartCoroutine(NotifTextNotEnoughMoney()); }  
        }
        if (UpgradeValue == 109)
        {
            if (shopHaveGarlicBread == false && shopWheat >= upgradePrices[6]) { shopHaveGarlicBread = true; shopWheat -= upgradePrices[6]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopHaveGarlicBread == true) { StartCoroutine(NotifTextAlreadyBought()); }
            else { StartCoroutine(NotifTextNotEnoughMoney()); }  
        }
        if (UpgradeValue == 110)
        {
            if (shopHaveHornScallop == false && shopWheat >= upgradePrices[5]) { shopHaveHornScallop = true; shopWheat -= upgradePrices[5]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopHaveHornScallop == true) { StartCoroutine(NotifTextAlreadyBought()); }
            else { StartCoroutine(NotifTextNotEnoughMoney()); }  
        }
        if (UpgradeValue == 111)
        {
            int EntreeValue = PlayerPrefs.GetInt("Ramen");
            if (EntreeValue == 0) { StartCoroutine(NotifTextHaveNotUnlocked()); }
            else { PlayerPrefs.SetInt("Ramen", 1); AudioPlayer.PlaySound("PurchaseShop"); }
        }
        if (UpgradeValue == 112)
        {
            int EntreeValue = PlayerPrefs.GetInt("Salmon");
            if (EntreeValue == 0) { StartCoroutine(NotifTextHaveNotUnlocked()); }
            else { PlayerPrefs.SetInt("Salmon", 1); AudioPlayer.PlaySound("PurchaseShop"); }
        }
        if (UpgradeValue == 113)
        {
            int EntreeValue = PlayerPrefs.GetInt("Steak");
            if (EntreeValue == 0) { StartCoroutine(NotifTextHaveNotUnlocked()); }
            else { PlayerPrefs.SetInt("Steak", 1); AudioPlayer.PlaySound("PurchaseShop"); }
        }
        UpdateUpgradeUI(UpgradeValue);   
        ShopWheatCounterNumber.text = shopWheat.ToString();
        SaveData();
    }

    public void Refund(int RefundValue)
    {
        if (RefundValue == 0 || RefundValue == 420)
        {
            if (shopStrength == 1.0) { refundLoops = 0; if (RefundValue != 420) {  StartCoroutine(NotifTextHaveNotBought()); } }
            else if (shopStrength == 1.5) { refundLoops = 1; }
            else if (shopStrength == 2) { refundLoops = 2; }
            else if (shopStrength == 2.5) { refundLoops = 3; }
            else if (shopStrength == 3) { refundLoops = 4; }
            else if (shopStrength == 5) { refundLoops = 5; }
            else { Debug.Log("Error In Refunding"); }   
            for (int i = 0; i < refundLoops; i++) { shopWheat += upgradePrices[i]; }        
            shopStrength = 1f;
        }
        if (RefundValue == 1 || RefundValue == 420)
        {
            if (shopDefense == 1.0) { refundLoops = 0; if (RefundValue != 420) { StartCoroutine(NotifTextHaveNotBought()); } }
            else if (shopDefense == 1.5) { refundLoops = 1; }
            else if (shopDefense == 2) { refundLoops = 2; }
            else if (shopDefense == 3) { refundLoops = 3; }
            else if (shopDefense == 4) { refundLoops = 4; }
            else if (shopDefense == 5) { refundLoops = 5; }
            else { Debug.Log("Error In Refunding"); }   
            for (int i = 0; i < refundLoops; i++) { shopWheat += upgradePrices[i]; }       
            shopDefense = 1f;
        }
        if (RefundValue == 2 || RefundValue == 420)
        {
            if (shopMaxHealth <= 30) { refundLoops = 0; if (RefundValue != 420) { StartCoroutine(NotifTextHaveNotBought()); } }
            else if (shopMaxHealth == 50) { refundLoops = 1; }
            else if (shopMaxHealth == 90) { refundLoops = 2; }
            else if (shopMaxHealth == 140) { refundLoops = 3; }
            else if (shopMaxHealth == 200) { refundLoops = 4; }
            else if (shopMaxHealth == 300) { refundLoops = 5; }
            else { Debug.Log("Error In Refunding"); }  
            for (int i = 0; i < refundLoops; i++) { shopWheat += upgradePrices[i]; }         
            shopMaxHealth = 30;
        }
        if (RefundValue == 3 || RefundValue == 420)
        {
            if (shopMoveSpeed < 2.51) { refundLoops = 0; if (RefundValue != 420) { StartCoroutine(NotifTextHaveNotBought()); } }
            else if (shopMoveSpeed >= 2.74 && shopMoveSpeed < 2.76 ) { refundLoops = 1; }
            else if (shopMoveSpeed >= 2.99 && shopMoveSpeed < 3.01 ) { refundLoops = 2; }
            else if (shopMoveSpeed >= 3.24 && shopMoveSpeed < 3.26 ) { refundLoops = 3; }
            else if (shopMoveSpeed >= 3.49 && shopMoveSpeed < 3.51 ) { refundLoops = 4; }
            else if (shopMoveSpeed >= 3.99) { refundLoops = 5; }
            else { Debug.Log("Error In Refunding"); }    
            for (int i = 0; i < refundLoops; i++) { shopWheat += upgradePrices[i]; }    
            shopMoveSpeed = 2.5f;
        }
        if (RefundValue == 4 || RefundValue == 420)
        {
            if (shopPlayerCooldownTime >= 10.0) { refundLoops = 0; if (RefundValue != 420) { StartCoroutine(NotifTextHaveNotBought()); } }
            else if (shopPlayerCooldownTime == 9.0) { refundLoops = 1; }
            else if (shopPlayerCooldownTime == 8.0) { refundLoops = 2; }
            else if (shopPlayerCooldownTime == 7.0) { refundLoops = 3; }
            else if (shopPlayerCooldownTime == 6.0) { refundLoops = 4; }
            else if (shopPlayerCooldownTime <= 5.0) { refundLoops = 5; }
            else { Debug.Log("Error In Refunding"); }   
            for (int i = 0; i < refundLoops; i++) { shopWheat += upgradePrices[i]; }    
            shopPlayerCooldownTime = 10.0f;
        }
        if (RefundValue == 5 || RefundValue == 420)
        {
            if (shopDiceNumber == 6) { refundLoops = 0; if (RefundValue != 420) { StartCoroutine(NotifTextHaveNotBought()); } }
            else if (shopDiceNumber == 9) { refundLoops = 1; }
            else if (shopDiceNumber == 12) { refundLoops = 2; }
            else if (shopDiceNumber == 18) { refundLoops = 3; }
            else if (shopDiceNumber == 24) { refundLoops = 4; }
            else if (shopDiceNumber == 30) { refundLoops = 5; }
            else { Debug.Log("Error In Refunding"); }         
            for (int i = 0; i < refundLoops; i++) { shopWheat += upgradePrices[i]; }  
            shopDiceNumber = 6;
        }
        if (RefundValue == 6 || RefundValue == 420)
        {
            if (shopWheatDroprate == 30.0) { refundLoops = 0; if (RefundValue != 420) { StartCoroutine(NotifTextHaveNotBought()); } }
            else if (shopWheatDroprate == 40.0) { refundLoops = 1; }
            else if (shopWheatDroprate == 50.0) { refundLoops = 2; }
            else if (shopWheatDroprate == 60.0) { refundLoops = 3; }
            else if (shopWheatDroprate == 70.0) { refundLoops = 4; }
            else if (shopWheatDroprate == 80.0) { refundLoops = 5; }
            else { Debug.Log("Error In Refunding"); }    
            for (int i = 0; i < refundLoops; i++) { shopWheat += upgradePrices[i]; }      
            shopWheatDroprate = 30.0f;
        }
        if (RefundValue == 7 || RefundValue == 420)
        {
            if (shopDicePreviewerLevel == 0) { refundLoops = 0; if (RefundValue != 420) { StartCoroutine(NotifTextHaveNotBought()); } }
            else if (shopDicePreviewerLevel == 1) { refundLoops = 1; }
            else if (shopDicePreviewerLevel == 2) { refundLoops = 2; }
            else if (shopDicePreviewerLevel == 3) { refundLoops = 3; }
            else if (shopDicePreviewerLevel == 4) { refundLoops = 4; }
            else if (shopDicePreviewerLevel == 5) { refundLoops = 5; }
            else { Debug.Log("Error In Refunding"); }   
            for (int i = 0; i < refundLoops; i++) { shopWheat += upgradePrices[i]; }    
            if(shopHaveBanhmi) 
                { 
                    shopWheat += upgradePrices[5]; 
                    shopHaveBanhmi = false; 
                    UpdateUpgradeUI(RefundValue + 98); 
                } 
            shopDicePreviewerLevel = 0;
        }
        if (RefundValue == 8 || RefundValue == 420)
        {
            if (shopDiceDroprate == 1000) { refundLoops = 0; if (RefundValue != 420) { StartCoroutine(NotifTextHaveNotBought()); } }
            else if (shopDiceDroprate == 500) { refundLoops = 1; }
            else if (shopDiceDroprate == 400) { refundLoops = 2; }
            else if (shopDiceDroprate == 300) { refundLoops = 3; }
            else if (shopDiceDroprate == 200) { refundLoops = 4; }
            else if (shopDiceDroprate == 100) { refundLoops = 5; }
            else { Debug.Log("Error In Refunding"); }   
            for (int i = 0; i < refundLoops; i++) { shopWheat += upgradePrices[i]; }    
            shopDiceDroprate = 1000;
        }
        if (RefundValue == 101 || RefundValue == 420)
        {
            if (shopHavePizza == false) { if (RefundValue != 420) { StartCoroutine(NotifTextHaveNotBought()); } }
            else { shopWheat += upgradePrices[5]; AudioPlayer.PlaySound("RefundShop"); }
            shopHavePizza = false;
        }
        if (RefundValue == 102 || RefundValue == 420)
        {
            if (shopHaveCarrotCake == false) { if (RefundValue != 420) { StartCoroutine(NotifTextHaveNotBought()); } }
            else { shopWheat += upgradePrices[5]; AudioPlayer.PlaySound("RefundShop"); }
            shopHaveCarrotCake = false;
        }
        if (RefundValue == 103 || RefundValue == 420)
        {
            if (shopHaveFlan == false) { if (RefundValue != 420) { StartCoroutine(NotifTextHaveNotBought()); } }
            else 
            { 
                AudioPlayer.PlaySound("RefundShop");
                shopWheat += upgradePrices[6]; 
                if(shopHaveCremeBrulee) 
                { 
                    shopWheat += upgradePrices[6]; 
                    shopHaveCremeBrulee = false; 
                    UpdateUpgradeUI(RefundValue + 1); 
                } 
            }
            shopHaveFlan = false;
        }
        if (RefundValue == 104 || RefundValue == 420)
        {
            if (shopHaveCremeBrulee == false) { if (RefundValue != 420) { StartCoroutine(NotifTextHaveNotBought()); } }
            else { shopWheat += upgradePrices[6]; AudioPlayer.PlaySound("RefundShop"); }
            shopHaveCremeBrulee = false;
        }
        if (RefundValue == 105 || RefundValue == 420)
        {
            if (shopHaveBanhmi == false) { if (RefundValue != 420) { StartCoroutine(NotifTextHaveNotBought()); } }
            else { shopWheat += upgradePrices[5]; AudioPlayer.PlaySound("RefundShop"); }
            shopHaveBanhmi = false;
        }
        if (RefundValue == 106 || RefundValue == 420)
        {
            if (shopHaveCupcake == false) { if (RefundValue != 420) { StartCoroutine(NotifTextHaveNotBought()); } }
            else { shopWheat += upgradePrices[6]; AudioPlayer.PlaySound("RefundShop"); }
            shopHaveCupcake = false;
        }
        if (RefundValue == 107 || RefundValue == 420)
        {
            if (shopHaveChickenNuggets == false) { if (RefundValue != 420) { StartCoroutine(NotifTextHaveNotBought()); } }
            else { shopWheat += upgradePrices[7]; AudioPlayer.PlaySound("RefundShop"); }
            shopHaveChickenNuggets = false;
        }
        if (RefundValue == 108 || RefundValue == 420)
        {
            if (shopHavePastelDeChoclo == false) { if (RefundValue != 420) { StartCoroutine(NotifTextHaveNotBought()); } }
            else { shopWheat += upgradePrices[7]; AudioPlayer.PlaySound("RefundShop"); }
            shopHavePastelDeChoclo = false;
        }
        if (RefundValue == 109 || RefundValue == 420)
        {
            if (shopHaveGarlicBread == false) { if (RefundValue != 420) { StartCoroutine(NotifTextHaveNotBought()); } }
            else { shopWheat += upgradePrices[6]; AudioPlayer.PlaySound("RefundShop"); }
            shopHaveGarlicBread = false;
        }
        if (RefundValue == 110 || RefundValue == 420)
        {
            if (shopHaveHornScallop == false) { if (RefundValue != 420) { StartCoroutine(NotifTextHaveNotBought()); } }
            else { shopWheat += upgradePrices[5]; AudioPlayer.PlaySound("RefundShop"); }
            shopHaveHornScallop = false;
        }
        if (RefundValue == 111)
        {
            int EntreeValue = PlayerPrefs.GetInt("Ramen");
            if (EntreeValue == 0) { StartCoroutine(NotifTextHaveNotUnlocked()); }
            else { PlayerPrefs.SetInt("Ramen", 2); AudioPlayer.PlaySound("RefundShop"); }
        }
        if (RefundValue == 112)
        {
            int EntreeValue = PlayerPrefs.GetInt("Salmon");
            if (EntreeValue == 0) { StartCoroutine(NotifTextHaveNotUnlocked()); }
            else { PlayerPrefs.SetInt("Salmon", 2); AudioPlayer.PlaySound("RefundShop"); }
        }
        if (RefundValue == 113)
        {
            int EntreeValue = PlayerPrefs.GetInt("Steak");
            if (EntreeValue == 0) { StartCoroutine(NotifTextHaveNotUnlocked()); }
            else { PlayerPrefs.SetInt("Steak", 2); AudioPlayer.PlaySound("RefundShop"); }
        }
        if (refundLoops > 0 || RefundValue == 420) { AudioPlayer.PlaySound("RefundShop"); }
        UpdateUpgradeUI(RefundValue);
        ShopWheatCounterNumber.text = shopWheat.ToString();
        SaveData();
    }

    private void UpdateUpgradeUI(int UpdateUpgradeUI)
    {
        if (UpdateUpgradeUI == 0 || UpdateUpgradeUI == 420)
        {
            if (shopStrength == 1.0) { UpgradesImage[0].sprite = UpgradesSpritesVariants[0]; upgradePriceText[0].text = upgradePrices[0].ToString(); }
            else if (shopStrength == 1.5) { UpgradesImage[0].sprite = UpgradesSpritesVariants[1]; upgradePriceText[0].text = upgradePrices[1].ToString();}
            else if (shopStrength == 2) { UpgradesImage[0].sprite = UpgradesSpritesVariants[2]; upgradePriceText[0].text = upgradePrices[2].ToString();}
            else if (shopStrength == 2.5) { UpgradesImage[0].sprite = UpgradesSpritesVariants[3]; upgradePriceText[0].text = upgradePrices[3].ToString();}
            else if (shopStrength == 3) { UpgradesImage[0].sprite = UpgradesSpritesVariants[4]; upgradePriceText[0].text = upgradePrices[4].ToString();}
            else if (shopStrength == 5) { UpgradesImage[0].sprite = UpgradesSpritesVariants[5]; upgradePriceText[0].text = MaxBought;}
            else { Debug.Log("Error in Updating UI");}
        }
        if (UpdateUpgradeUI == 1 || UpdateUpgradeUI == 420)
        {
            if (shopDefense == 1.0) { UpgradesImage[1].sprite = UpgradesSpritesVariants[6]; upgradePriceText[1].text = upgradePrices[0].ToString(); }
            else if (shopDefense == 1.5) { UpgradesImage[1].sprite = UpgradesSpritesVariants[7]; upgradePriceText[1].text = upgradePrices[1].ToString();}
            else if (shopDefense == 2) { UpgradesImage[1].sprite = UpgradesSpritesVariants[8]; upgradePriceText[1].text = upgradePrices[2].ToString();}
            else if (shopDefense == 3) { UpgradesImage[1].sprite = UpgradesSpritesVariants[9]; upgradePriceText[1].text = upgradePrices[3].ToString();}
            else if (shopDefense == 4) { UpgradesImage[1].sprite = UpgradesSpritesVariants[10]; upgradePriceText[1].text = upgradePrices[4].ToString();}
            else if (shopDefense == 5) { UpgradesImage[1].sprite = UpgradesSpritesVariants[11]; upgradePriceText[1].text = MaxBought;}
            else { Debug.Log("Error in Updating UI");}
        }
        if (UpdateUpgradeUI == 2 || UpdateUpgradeUI == 420)
        {
            if (shopMaxHealth == 30) { UpgradesImage[2].sprite = UpgradesSpritesVariants[12]; upgradePriceText[2].text = upgradePrices[0].ToString(); }
            else if (shopMaxHealth == 50) { UpgradesImage[2].sprite = UpgradesSpritesVariants[13]; upgradePriceText[2].text = upgradePrices[1].ToString();}
            else if (shopMaxHealth == 90) { UpgradesImage[2].sprite = UpgradesSpritesVariants[14]; upgradePriceText[2].text = upgradePrices[2].ToString();}
            else if (shopMaxHealth == 140) { UpgradesImage[2].sprite = UpgradesSpritesVariants[15]; upgradePriceText[2].text = upgradePrices[3].ToString();}
            else if (shopMaxHealth == 200) { UpgradesImage[2].sprite = UpgradesSpritesVariants[16]; upgradePriceText[2].text = upgradePrices[4].ToString();}
            else if (shopMaxHealth == 300) { UpgradesImage[2].sprite = UpgradesSpritesVariants[17]; upgradePriceText[2].text = MaxBought;}
            else { Debug.Log("Error in Updating UI");}
        }
        if (UpdateUpgradeUI == 3 || UpdateUpgradeUI == 420)
        {
            if (shopMoveSpeed < 2.51) { UpgradesImage[3].sprite = UpgradesSpritesVariants[18]; upgradePriceText[3].text = upgradePrices[0].ToString(); }
            else if (shopMoveSpeed >= 2.74 && shopMoveSpeed < 2.76 ) { UpgradesImage[3].sprite = UpgradesSpritesVariants[19]; upgradePriceText[3].text = upgradePrices[1].ToString();}
            else if (shopMoveSpeed >= 2.99 && shopMoveSpeed < 3.01 ) { UpgradesImage[3].sprite = UpgradesSpritesVariants[20]; upgradePriceText[3].text = upgradePrices[2].ToString();}
            else if (shopMoveSpeed >= 3.24 && shopMoveSpeed < 3.26 ) { UpgradesImage[3].sprite = UpgradesSpritesVariants[21]; upgradePriceText[3].text = upgradePrices[3].ToString();}
            else if (shopMoveSpeed >= 3.49 && shopMoveSpeed < 3.51 ) { UpgradesImage[3].sprite = UpgradesSpritesVariants[22]; upgradePriceText[3].text = upgradePrices[4].ToString();}
            else if (shopMoveSpeed >= 3.99) { UpgradesImage[3].sprite = UpgradesSpritesVariants[23]; upgradePriceText[3].text = MaxBought;}
            else { Debug.Log("Error in Updating UI");}
        }
        if (UpdateUpgradeUI == 4 || UpdateUpgradeUI == 420)
        {
            if (shopPlayerCooldownTime >= 10.0) { UpgradesImage[4].sprite = UpgradesSpritesVariants[24]; upgradePriceText[4].text = upgradePrices[0].ToString(); }
            else if (shopPlayerCooldownTime == 9.0) { UpgradesImage[4].sprite = UpgradesSpritesVariants[25]; upgradePriceText[4].text = upgradePrices[1].ToString();}
            else if (shopPlayerCooldownTime == 8.0) { UpgradesImage[4].sprite = UpgradesSpritesVariants[26]; upgradePriceText[4].text = upgradePrices[2].ToString();}
            else if (shopPlayerCooldownTime == 7.0) { UpgradesImage[4].sprite = UpgradesSpritesVariants[27]; upgradePriceText[4].text = upgradePrices[3].ToString();}
            else if (shopPlayerCooldownTime == 6.0) { UpgradesImage[4].sprite = UpgradesSpritesVariants[28]; upgradePriceText[4].text = upgradePrices[4].ToString();}
            else if (shopPlayerCooldownTime <= 5.0) { UpgradesImage[4].sprite = UpgradesSpritesVariants[29]; upgradePriceText[4].text = MaxBought;}
            else { Debug.Log("Error in Updating UI");}
        }
        if (UpdateUpgradeUI == 5 || UpdateUpgradeUI == 420)
        {
            if (shopDiceNumber == 6) { UpgradesImage[5].sprite = UpgradesSpritesVariants[30]; upgradePriceText[5].text = upgradePrices[0].ToString(); }
            else if (shopDiceNumber == 9) { UpgradesImage[5].sprite = UpgradesSpritesVariants[31]; upgradePriceText[5].text = upgradePrices[1].ToString();}
            else if (shopDiceNumber == 12) { UpgradesImage[5].sprite = UpgradesSpritesVariants[32]; upgradePriceText[5].text = upgradePrices[2].ToString();}
            else if (shopDiceNumber == 18) { UpgradesImage[5].sprite = UpgradesSpritesVariants[33]; upgradePriceText[5].text = upgradePrices[3].ToString();}
            else if (shopDiceNumber == 24) { UpgradesImage[5].sprite = UpgradesSpritesVariants[34]; upgradePriceText[5].text = upgradePrices[4].ToString();}
            else if (shopDiceNumber == 30) { UpgradesImage[5].sprite = UpgradesSpritesVariants[35]; upgradePriceText[5].text = MaxBought;}
            else { Debug.Log("Error in Updating UI");}
        }
        if (UpdateUpgradeUI == 6 || UpdateUpgradeUI == 420)
        {
            if (shopWheatDroprate == 30.0) { UpgradesImage[6].sprite = UpgradesSpritesVariants[36]; upgradePriceText[6].text = upgradePrices[0].ToString(); }
            else if (shopWheatDroprate == 40.0) { UpgradesImage[6].sprite = UpgradesSpritesVariants[37]; upgradePriceText[6].text = upgradePrices[1].ToString();}
            else if (shopWheatDroprate == 50.0) { UpgradesImage[6].sprite = UpgradesSpritesVariants[38]; upgradePriceText[6].text = upgradePrices[2].ToString();}
            else if (shopWheatDroprate == 60.0) { UpgradesImage[6].sprite = UpgradesSpritesVariants[39]; upgradePriceText[6].text = upgradePrices[3].ToString();}
            else if (shopWheatDroprate == 70.0) { UpgradesImage[6].sprite = UpgradesSpritesVariants[40]; upgradePriceText[6].text = upgradePrices[4].ToString();}
            else if (shopWheatDroprate == 80.0) { UpgradesImage[6].sprite = UpgradesSpritesVariants[41]; upgradePriceText[6].text = MaxBought;}
            else { Debug.Log("Error in Updating UI");}
        }
        if (UpdateUpgradeUI == 7 || UpdateUpgradeUI == 420)
        {
            if (shopDicePreviewerLevel == 0) { UpgradesImage[7].sprite = UpgradesSpritesVariants[42]; upgradePriceText[7].text = upgradePrices[0].ToString(); }
            else if (shopDicePreviewerLevel == 1) { UpgradesImage[7].sprite = UpgradesSpritesVariants[43]; upgradePriceText[7].text = upgradePrices[1].ToString();}
            else if (shopDicePreviewerLevel == 2) { UpgradesImage[7].sprite = UpgradesSpritesVariants[44]; upgradePriceText[7].text = upgradePrices[2].ToString();}
            else if (shopDicePreviewerLevel == 3) { UpgradesImage[7].sprite = UpgradesSpritesVariants[45]; upgradePriceText[7].text = upgradePrices[3].ToString();}
            else if (shopDicePreviewerLevel == 4) { UpgradesImage[7].sprite = UpgradesSpritesVariants[46]; upgradePriceText[7].text = upgradePrices[4].ToString();}
            else if (shopDicePreviewerLevel == 5) { UpgradesImage[7].sprite = UpgradesSpritesVariants[47]; upgradePriceText[7].text = MaxBought;}
            else { Debug.Log("Error in Updating UI");}
        }
        if (UpdateUpgradeUI == 8 || UpdateUpgradeUI == 420)
        {
            if (shopDiceDroprate == 1000) { UpgradesImage[13].sprite = UpgradesSpritesVariants[58]; upgradePriceText[13].text = upgradePrices[0].ToString(); }
            else if (shopDiceDroprate == 500) { UpgradesImage[13].sprite = UpgradesSpritesVariants[59]; upgradePriceText[13].text = upgradePrices[1].ToString();}
            else if (shopDiceDroprate == 400) { UpgradesImage[13].sprite = UpgradesSpritesVariants[60]; upgradePriceText[13].text = upgradePrices[2].ToString();}
            else if (shopDiceDroprate == 300) { UpgradesImage[13].sprite = UpgradesSpritesVariants[61]; upgradePriceText[13].text = upgradePrices[3].ToString();}
            else if (shopDiceDroprate == 200) { UpgradesImage[13].sprite = UpgradesSpritesVariants[62]; upgradePriceText[13].text = upgradePrices[4].ToString();}
            else if (shopDiceDroprate == 100) { UpgradesImage[13].sprite = UpgradesSpritesVariants[63]; upgradePriceText[13].text = MaxBought;}
            else { Debug.Log("Error in Updating UI");}
        }
        if (UpdateUpgradeUI == 101 || UpdateUpgradeUI == 420)
        {
            if (shopHavePizza) { UpgradesImage[8].sprite = UpgradesSpritesVariants[49]; upgradePriceText[8].text = MaxBought; }
            else { UpgradesImage[8].sprite = UpgradesSpritesVariants[48]; upgradePriceText[8].text = upgradePrices[5].ToString(); }
        }
        if (UpdateUpgradeUI == 102 || UpdateUpgradeUI == 420)
        {
            if (shopHaveCarrotCake) { UpgradesImage[9].sprite = UpgradesSpritesVariants[51]; upgradePriceText[9].text = MaxBought; }
            else { UpgradesImage[9].sprite = UpgradesSpritesVariants[50]; upgradePriceText[9].text = upgradePrices[5].ToString(); }
        }
        if (UpdateUpgradeUI == 103 || UpdateUpgradeUI == 420)
        {
            if (shopHaveFlan) { UpgradesImage[10].sprite = UpgradesSpritesVariants[53]; upgradePriceText[10].text = MaxBought; }
            else { UpgradesImage[10].sprite = UpgradesSpritesVariants[52]; upgradePriceText[10].text = upgradePrices[6].ToString(); }
        }
        if (UpdateUpgradeUI == 104 || UpdateUpgradeUI == 420)
        {
            if (shopHaveCremeBrulee) { UpgradesImage[11].sprite = UpgradesSpritesVariants[55]; upgradePriceText[11].text = MaxBought; }
            else { UpgradesImage[11].sprite = UpgradesSpritesVariants[54]; upgradePriceText[11].text = upgradePrices[6].ToString(); }
        }
        if (UpdateUpgradeUI == 105 || UpdateUpgradeUI == 420)
        {
            if (shopHaveBanhmi) { UpgradesImage[12].sprite = UpgradesSpritesVariants[57]; upgradePriceText[12].text = MaxBought; }
            else { UpgradesImage[12].sprite = UpgradesSpritesVariants[56]; upgradePriceText[12].text = upgradePrices[5].ToString(); }
        }
        if (UpdateUpgradeUI == 106 || UpdateUpgradeUI == 420)
        {
            if (shopHaveCupcake) { UpgradesImage[14].sprite = UpgradesSpritesVariants[65]; upgradePriceText[14].text = MaxBought; }
            else { UpgradesImage[14].sprite = UpgradesSpritesVariants[64]; upgradePriceText[14].text = upgradePrices[6].ToString(); }
        }
        if (UpdateUpgradeUI == 107 || UpdateUpgradeUI == 420)
        {
            if (shopHaveChickenNuggets) { UpgradesImage[15].sprite = UpgradesSpritesVariants[67]; upgradePriceText[15].text = MaxBought; }
            else { UpgradesImage[15].sprite = UpgradesSpritesVariants[66]; upgradePriceText[15].text = upgradePrices[7].ToString(); }
        }
        if (UpdateUpgradeUI == 108 || UpdateUpgradeUI == 420)
        {
            if (shopHavePastelDeChoclo) { UpgradesImage[16].sprite = UpgradesSpritesVariants[69]; upgradePriceText[16].text = MaxBought; }
            else { UpgradesImage[16].sprite = UpgradesSpritesVariants[68]; upgradePriceText[16].text = upgradePrices[7].ToString(); }
        }
        if (UpdateUpgradeUI == 109 || UpdateUpgradeUI == 420)
        {
            if (shopHaveGarlicBread) { UpgradesImage[17].sprite = UpgradesSpritesVariants[71]; upgradePriceText[17].text = MaxBought; }
            else { UpgradesImage[17].sprite = UpgradesSpritesVariants[70]; upgradePriceText[17].text = upgradePrices[6].ToString(); }
        }
        if (UpdateUpgradeUI == 110 || UpdateUpgradeUI == 420)
        {
            if (shopHaveHornScallop) { UpgradesImage[18].sprite = UpgradesSpritesVariants[73]; upgradePriceText[18].text = MaxBought; }
            else { UpgradesImage[18].sprite = UpgradesSpritesVariants[72]; upgradePriceText[18].text = upgradePrices[5].ToString(); }
        }
        if (UpdateUpgradeUI == 111 || UpdateUpgradeUI == 420)
        {
            int EntreeValue = PlayerPrefs.GetInt("Ramen");
            if (EntreeValue == 0) { UpgradesImage[19].sprite = UpgradesSpritesVariants[74]; upgradePriceText[19].text = "???"; }
            else if (EntreeValue == 1) { UpgradesImage[19].sprite = UpgradesSpritesVariants[75]; upgradePriceText[19].text = "Ramen"; }
            else { UpgradesImage[19].sprite = UpgradesSpritesVariants[76]; upgradePriceText[19].text = "Ramen"; }
        }
        if (UpdateUpgradeUI == 112 || UpdateUpgradeUI == 420)
        {
            int EntreeValue = PlayerPrefs.GetInt("Salmon");
            if (EntreeValue == 0) { UpgradesImage[20].sprite = UpgradesSpritesVariants[77]; upgradePriceText[20].text = "???"; }
            else if (EntreeValue == 1) { UpgradesImage[20].sprite = UpgradesSpritesVariants[78]; upgradePriceText[20].text = "Salmon"; }
            else { UpgradesImage[20].sprite = UpgradesSpritesVariants[79]; upgradePriceText[20].text = "Salmon"; }
        }
        if (UpdateUpgradeUI == 113 || UpdateUpgradeUI == 420)
        {
            int EntreeValue = PlayerPrefs.GetInt("Steak");
            if (EntreeValue == 0) { UpgradesImage[21].sprite = UpgradesSpritesVariants[80]; upgradePriceText[21].text = "???"; }
            else if (EntreeValue == 1) { UpgradesImage[21].sprite = UpgradesSpritesVariants[81]; upgradePriceText[21].text = "Steak"; }
            else { UpgradesImage[21].sprite = UpgradesSpritesVariants[82]; upgradePriceText[21].text = "Steak"; }
        }
    }

    private void DisplayTrophy(int wins)
    {
        trophy.SetActive(true);
        winsText.text = wins.ToString();
    }

    IEnumerator NotifTextBuySuccess()
    {
        AudioPlayer.PlaySound("PurchaseShop");
        if (NotifTextBuySuccessDisplayed == false)
        {
            NotifTextBuySuccessDisplayed = true;
            NotifText.text = "Purchased!";
            NotifText.color = new Color32(255, 255, 255, 255);
            yield return new WaitForSeconds(1f);
            NotifTextBuySuccessDisplayed = false;
            NotifText.text = "";
            yield return null;
        }
    }

    IEnumerator NotifTextNotEnoughMoney()
    {
        AudioPlayer.PlaySound("UIButtonError");
        if (NotifTextNotEnoughMoneyDisplayed == false)
        {
            NotifTextNotEnoughMoneyDisplayed = true;
            NotifText.text = "Not Enough Wheat!";
            NotifText.color = new Color32(200, 0, 0, 255);
            yield return new WaitForSeconds(1f);
            NotifTextNotEnoughMoneyDisplayed = false;
            NotifText.text = "";
            yield return null;
        }
    }

    IEnumerator NotifTextAlreadyBought()
    {
        AudioPlayer.PlaySound("UIButtonError");
        if (NotifTextAlreadyBoughtDisplayed == false)
        {
            NotifTextAlreadyBoughtDisplayed = true;
            NotifText.text = "Maxed Upgrade";
            NotifText.color = new Color32(200, 0, 0, 255);
            yield return new WaitForSeconds(1f);
            NotifTextAlreadyBoughtDisplayed = false;
            NotifText.text = "";
            yield return null;
        }
    }

    IEnumerator NotifTextHaveNotBought()
    {
        AudioPlayer.PlaySound("UIButtonError");
        if (NotifTextHaveNotBoughtDisplayed == false)
        {
            NotifTextHaveNotBoughtDisplayed = true;
            NotifText.text = "Have Not Bought!";
            NotifText.color = new Color32(200, 0, 0, 255);
            yield return new WaitForSeconds(1f);
            NotifTextHaveNotBoughtDisplayed = false;
            NotifText.text = "";
            yield return null;
        }
    }

    IEnumerator NotifTextHaveNotUnlocked()
    {
        AudioPlayer.PlaySound("UIButtonError");
        if (NotifTextHaveNotBoughtDisplayed == false)
        {
            NotifTextHaveNotBoughtDisplayed = true;
            NotifText.text = "Have Not Unlocked!";
            NotifText.color = new Color32(200, 0, 0, 255);
            yield return new WaitForSeconds(1f);
            NotifTextHaveNotBoughtDisplayed = false;
            NotifText.text = "";
            yield return null;
        }
    }

    IEnumerator NotifTextNeedFlan()
    {
        AudioPlayer.PlaySound("UIButtonError");
        if (NotifTextHaveNotBoughtDisplayed == false)
        {
            NotifTextHaveNotBoughtDisplayed = true;
            NotifText.text = "Need Flan";
            NotifText.color = new Color32(200, 0, 0, 255);
            yield return new WaitForSeconds(1f);
            NotifTextHaveNotBoughtDisplayed = false;
            NotifText.text = "";
            yield return null;
        }
    }

    IEnumerator NotifTextNeedDicePreviewer()
    {
        AudioPlayer.PlaySound("UIButtonError");
        if (NotifTextHaveNotBoughtDisplayed == false)
        {
            NotifTextHaveNotBoughtDisplayed = true;
            NotifText.text = "Need Dice Previewer";
            NotifText.color = new Color32(200, 0, 0, 255);
            yield return new WaitForSeconds(1f);
            NotifTextHaveNotBoughtDisplayed = false;
            NotifText.text = "";
            yield return null;
        }
    }

    private void LoadData()
    {
        string json = File.ReadAllText(Application.dataPath + "/gameSaveData.json");
        PlayerData loadedPlayerData = JsonUtility.FromJson<PlayerData>(json);

        shopMoveSpeed = loadedPlayerData.MoveSpeed;
        shopMaxHealth = loadedPlayerData.maxHealth;
        shopPlayerHealth = loadedPlayerData.playerHealth;
        shopStrength = loadedPlayerData.strength;
        shopWheat = loadedPlayerData.Wheat;
        shopDiceNumber = loadedPlayerData.diceNumber;
        shopPlayerCooldownTime = loadedPlayerData.playerCooldownTime;
        shopDefense = loadedPlayerData.defense;
        shopWheatDroprate = loadedPlayerData.wheatDroprate;
        shopDicePreviewerLevel = loadedPlayerData.dicePreviewerLevel;
        shopDiceDroprate = loadedPlayerData.diceDroprate;
        shopHavePizza = loadedPlayerData.havePizza;
        shopHaveCarrotCake = loadedPlayerData.haveCarrotCake;
        shopHaveFlan = loadedPlayerData.haveFlan;
        shopHaveCremeBrulee = loadedPlayerData.haveCremeBrulee;
        shopHaveBanhmi = loadedPlayerData.haveBanhmi;
        shopHaveCupcake = loadedPlayerData.haveCupcake;
        shopHaveChickenNuggets = loadedPlayerData.haveChickenNuggets;
        shopHavePastelDeChoclo = loadedPlayerData.havePastelDeChoclo;
        shopHaveGarlicBread = loadedPlayerData.haveGarlicBread;
        shopHaveHornScallop = loadedPlayerData.haveHornScallop;

        ShopWheatCounterNumber.text = shopWheat.ToString();
    }

    private void SaveData()
    {
        PlayerData savingPlayerData = new PlayerData();
        savingPlayerData.MoveSpeed = shopMoveSpeed;
        savingPlayerData.maxHealth = shopMaxHealth;
        savingPlayerData.playerHealth = shopPlayerHealth;
        savingPlayerData.strength = shopStrength;
        savingPlayerData.Wheat = shopWheat;
        savingPlayerData.diceNumber = shopDiceNumber;
        savingPlayerData.playerCooldownTime = shopPlayerCooldownTime;
        savingPlayerData.defense = shopDefense;
        savingPlayerData.wheatDroprate = shopWheatDroprate;
        savingPlayerData.dicePreviewerLevel = shopDicePreviewerLevel;
        savingPlayerData.diceDroprate = shopDiceDroprate;
        savingPlayerData.havePizza = shopHavePizza;
        savingPlayerData.haveCarrotCake = shopHaveCarrotCake;
        savingPlayerData.haveFlan = shopHaveFlan;
        savingPlayerData.haveCremeBrulee = shopHaveCremeBrulee;
        savingPlayerData.haveBanhmi = shopHaveBanhmi;
        savingPlayerData.haveCupcake = shopHaveCupcake;
        savingPlayerData.haveChickenNuggets = shopHaveChickenNuggets;
        savingPlayerData.havePastelDeChoclo = shopHavePastelDeChoclo;
        savingPlayerData.haveGarlicBread = shopHaveGarlicBread;
        savingPlayerData.haveHornScallop = shopHaveHornScallop;

        string json = JsonUtility.ToJson(savingPlayerData);
        Debug.Log(json);

        File.WriteAllText(Application.dataPath + "/gameSaveData.json", json);    
    }

    private class PlayerData
    {
        public float MoveSpeed;
        public int maxHealth;
        public int playerHealth;
        public float strength;
        public int Wheat;
        public int diceNumber;
        public float playerCooldownTime;
        public float defense;
        public float wheatDroprate;
        public int dicePreviewerLevel;
        public int diceDroprate;
        public bool havePizza;
        public bool haveCarrotCake;
        public bool haveFlan;
        public bool haveCremeBrulee;
        public bool haveBanhmi;
        public bool haveCupcake;
        public bool haveChickenNuggets;
        public bool havePastelDeChoclo;
        public bool haveGarlicBread;
        public bool haveHornScallop;
    }
}

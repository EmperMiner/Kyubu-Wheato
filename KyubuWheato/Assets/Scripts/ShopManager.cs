using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

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
    private bool shopHavePizza;
    private bool shopHaveCarrotCake;
    private bool shopHaveFlan;
    private bool shopHaveCremeBrulee;
    private bool shopHaveBanhmi;

    [SerializeField] private Text ShopWheatCounterNumber;
    [SerializeField] private int[] upgradePrices;
    [SerializeField] private Text[] upgradePriceText;
    [SerializeField] private Image[] UpgradesImage;
    [SerializeField] private Sprite[] UpgradesSpritesVariants;
    [SerializeField] private Text NotifText;

    private bool NotifTextBuySuccessDisplayed = false;
    private bool NotifTextNotEnoughMoneyDisplayed = false;
    private bool NotifTextAlreadyBoughtDisplayed = false;
    private bool NotifTextHaveNotBoughtDisplayed = false;

    private string MaxBought = "Maxed";

    private int refundLoops;

    private void Start()
    {
        LoadData();
        UpdateUpgradeUI(420);
        NotifText.text = "";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            shopWheat += 10;
            ShopWheatCounterNumber.text = shopWheat.ToString();
            SaveData();
        } else if (Input.GetKeyDown(KeyCode.R))
        {
            shopWheat -= 10;
            ShopWheatCounterNumber.text = shopWheat.ToString();
            SaveData();
        }
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
            if (shopMaxHealth == 15 && shopWheat >= upgradePrices[0]) { shopMaxHealth = 25; shopWheat -= upgradePrices[0]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopMaxHealth == 25 && shopWheat >= upgradePrices[1]) { shopMaxHealth = 45; shopWheat -= upgradePrices[1]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopMaxHealth == 45 && shopWheat >= upgradePrices[2]) { shopMaxHealth = 70; shopWheat -= upgradePrices[2]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopMaxHealth == 70 && shopWheat >= upgradePrices[3]) { shopMaxHealth = 100; shopWheat -= upgradePrices[3]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopMaxHealth == 100 && shopWheat >= upgradePrices[4]) { shopMaxHealth = 150; shopWheat -= upgradePrices[4]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopMaxHealth == 150) { StartCoroutine(NotifTextAlreadyBought()); }
            else { StartCoroutine(NotifTextNotEnoughMoney()); }
        }
        if (UpgradeValue == 3)
        {
            if (shopMoveSpeed < 2.01 && shopWheat >= upgradePrices[0]) { shopMoveSpeed = 2.25f; shopWheat -= upgradePrices[0]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopMoveSpeed >= 2.24 && shopMoveSpeed < 2.26 && shopWheat >= upgradePrices[1]) { shopMoveSpeed = 2.5f; shopWheat -= upgradePrices[1]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopMoveSpeed >= 2.49 && shopMoveSpeed < 2.51 && shopWheat >= upgradePrices[2]) { shopMoveSpeed = 2.75f; shopWheat -= upgradePrices[2]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopMoveSpeed >= 2.74 && shopMoveSpeed < 2.76 && shopWheat >= upgradePrices[3]) { shopMoveSpeed = 3f; shopWheat -= upgradePrices[3]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopMoveSpeed >= 2.99 && shopMoveSpeed < 3.01 && shopWheat >= upgradePrices[4]) { shopMoveSpeed = 3.5f; shopWheat -= upgradePrices[4]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopMoveSpeed >= 3.49) { StartCoroutine(NotifTextAlreadyBought()); }
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
            if (shopDiceNumber == 3 && shopWheat >= upgradePrices[0]) { shopDiceNumber = 4; shopWheat -= upgradePrices[0]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopDiceNumber == 4 && shopWheat >= upgradePrices[1]) { shopDiceNumber = 6; shopWheat -= upgradePrices[1]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopDiceNumber == 6 && shopWheat >= upgradePrices[2]) { shopDiceNumber = 8; shopWheat -= upgradePrices[2]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopDiceNumber == 8 && shopWheat >= upgradePrices[3]) { shopDiceNumber = 11; shopWheat -= upgradePrices[3]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopDiceNumber == 11 && shopWheat >= upgradePrices[4]) { shopDiceNumber = 15; shopWheat -= upgradePrices[4]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopDiceNumber == 15) { StartCoroutine(NotifTextAlreadyBought()); }
            else { StartCoroutine(NotifTextNotEnoughMoney()); } 
        }
        if (UpgradeValue == 6)
        {
            if (shopWheatDroprate == 50.0 && shopWheat >= upgradePrices[0]) { shopWheatDroprate = 60.0f; shopWheat -= upgradePrices[0]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopWheatDroprate == 60.0 && shopWheat >= upgradePrices[1]) { shopWheatDroprate = 70.0f; shopWheat -= upgradePrices[1]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopWheatDroprate == 70.0 && shopWheat >= upgradePrices[2]) { shopWheatDroprate = 80.0f; shopWheat -= upgradePrices[2]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopWheatDroprate == 80.0 && shopWheat >= upgradePrices[3]) { shopWheatDroprate = 90.0f; shopWheat -= upgradePrices[3]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopWheatDroprate == 90.0 && shopWheat >= upgradePrices[4]) { shopWheatDroprate = 100.0f; shopWheat -= upgradePrices[4]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopWheatDroprate == 100.0) { StartCoroutine(NotifTextAlreadyBought()); }
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
            if (shopHaveFlan == false && shopWheat >= upgradePrices[5]) { shopHaveFlan = true; shopWheat -= upgradePrices[5]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopHaveFlan == true) { StartCoroutine(NotifTextAlreadyBought()); }
            else { StartCoroutine(NotifTextNotEnoughMoney()); }
        }
        if (UpgradeValue == 104)
        {
            if (shopHaveFlan == false) { StartCoroutine(NotifTextNeedFlan()); }
            else if (shopHaveCremeBrulee == false && shopWheat >= upgradePrices[5]) { shopHaveCremeBrulee = true; shopWheat -= upgradePrices[5]; StartCoroutine(NotifTextBuySuccess()); }
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
        UpdateUpgradeUI(UpgradeValue);   
        ShopWheatCounterNumber.text = shopWheat.ToString();
        SaveData();
    }

    public void Refund(int RefundValue)
    {
        if (RefundValue == 0)
        {
            if (shopStrength == 1.0) { refundLoops = 0; StartCoroutine(NotifTextHaveNotBought()); }
            else if (shopStrength == 1.5) { refundLoops = 1; }
            else if (shopStrength == 2) { refundLoops = 2; }
            else if (shopStrength == 2.5) { refundLoops = 3; }
            else if (shopStrength == 3) { refundLoops = 4; }
            else if (shopStrength == 5) { refundLoops = 5; }
            else { Debug.Log("Error In Refunding"); }   
            for (int i = 0; i < refundLoops; i++) { shopWheat += upgradePrices[i]; }        
            shopStrength = 1f;
        }
        if (RefundValue == 1)
        {
            if (shopDefense == 1.0) { refundLoops = 0; StartCoroutine(NotifTextHaveNotBought()); }
            else if (shopDefense == 1.5) { refundLoops = 1; }
            else if (shopDefense == 2) { refundLoops = 2; }
            else if (shopDefense == 3) { refundLoops = 3; }
            else if (shopDefense == 4) { refundLoops = 4; }
            else if (shopDefense == 5) { refundLoops = 5; }
            else { Debug.Log("Error In Refunding"); }   
            for (int i = 0; i < refundLoops; i++) { shopWheat += upgradePrices[i]; }       
            shopDefense = 1f;
        }
        if (RefundValue == 2)
        {
            if (shopMaxHealth == 15) { refundLoops = 0; StartCoroutine(NotifTextHaveNotBought()); }
            else if (shopMaxHealth == 25) { refundLoops = 1; }
            else if (shopMaxHealth == 45) { refundLoops = 2; }
            else if (shopMaxHealth == 70) { refundLoops = 3; }
            else if (shopMaxHealth == 100) { refundLoops = 4; }
            else if (shopMaxHealth == 150) { refundLoops = 5; }
            else { Debug.Log("Error In Refunding"); }  
            for (int i = 0; i < refundLoops; i++) { shopWheat += upgradePrices[i]; }         
            shopMaxHealth = 15;
        }
        if (RefundValue == 3)
        {
            if (shopMoveSpeed < 2.01) { refundLoops = 0; StartCoroutine(NotifTextHaveNotBought()); }
            else if (shopMoveSpeed >= 2.24 && shopMoveSpeed < 2.26 ) { refundLoops = 1; }
            else if (shopMoveSpeed >= 2.49 && shopMoveSpeed < 2.51 ) { refundLoops = 2; }
            else if (shopMoveSpeed >= 2.74 && shopMoveSpeed < 2.76 ) { refundLoops = 3; }
            else if (shopMoveSpeed >= 2.99 && shopMoveSpeed < 3.01 ) { refundLoops = 4; }
            else if (shopMoveSpeed >= 3.49) { refundLoops = 5; }
            else { Debug.Log("Error In Refunding"); }    
            for (int i = 0; i < refundLoops; i++) { shopWheat += upgradePrices[i]; }    
            shopMoveSpeed = 2.0f;
        }
        if (RefundValue == 4)
        {
            if (shopPlayerCooldownTime >= 10.0) { refundLoops = 0; StartCoroutine(NotifTextHaveNotBought()); }
            else if (shopPlayerCooldownTime == 9.0) { refundLoops = 1; }
            else if (shopPlayerCooldownTime == 8.0) { refundLoops = 2; }
            else if (shopPlayerCooldownTime == 7.0) { refundLoops = 3; }
            else if (shopPlayerCooldownTime == 6.0) { refundLoops = 4; }
            else if (shopPlayerCooldownTime <= 5.0) { refundLoops = 5; }
            else { Debug.Log("Error In Refunding"); }   
            for (int i = 0; i < refundLoops; i++) { shopWheat += upgradePrices[i]; }    
            shopPlayerCooldownTime = 10.0f;
        }
        if (RefundValue == 5)
        {
            if (shopDiceNumber == 3) { refundLoops = 0; StartCoroutine(NotifTextHaveNotBought()); }
            else if (shopDiceNumber == 4) { refundLoops = 1; }
            else if (shopDiceNumber == 6) { refundLoops = 2; }
            else if (shopDiceNumber == 8) { refundLoops = 3; }
            else if (shopDiceNumber == 11) { refundLoops = 4; }
            else if (shopDiceNumber == 15) { refundLoops = 5; }
            else { Debug.Log("Error In Refunding"); }         
            for (int i = 0; i < refundLoops; i++) { shopWheat += upgradePrices[i]; }  
            shopDiceNumber = 3;
        }
        if (RefundValue == 6)
        {
            if (shopWheatDroprate == 50.0) { refundLoops = 0; StartCoroutine(NotifTextHaveNotBought()); }
            else if (shopWheatDroprate == 60.0) { refundLoops = 1; }
            else if (shopWheatDroprate == 70.0) { refundLoops = 2; }
            else if (shopWheatDroprate == 80.0) { refundLoops = 3; }
            else if (shopWheatDroprate == 90.0) { refundLoops = 4; }
            else if (shopWheatDroprate == 100.0) { refundLoops = 5; }
            else { Debug.Log("Error In Refunding"); }    
            for (int i = 0; i < refundLoops; i++) { shopWheat += upgradePrices[i]; }      
            shopWheatDroprate = 50.0f;
        }
        if (RefundValue == 7)
        {
            if (shopDicePreviewerLevel == 0) { refundLoops = 0; StartCoroutine(NotifTextHaveNotBought()); }
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
        if (RefundValue == 101)
        {
            if (shopHavePizza == false) { StartCoroutine(NotifTextHaveNotBought()); }
            else { shopWheat += upgradePrices[5]; }
            shopHavePizza = false;
        }
        if (RefundValue == 102)
        {
            if (shopHaveCarrotCake == false) { StartCoroutine(NotifTextHaveNotBought()); }
            else { shopWheat += upgradePrices[5]; }
            shopHaveCarrotCake = false;
        }
        if (RefundValue == 103)
        {
            if (shopHaveFlan == false) { StartCoroutine(NotifTextHaveNotBought()); }
            else 
            { 
                shopWheat += upgradePrices[5]; 
                if(shopHaveCremeBrulee) 
                { 
                    shopWheat += upgradePrices[5]; 
                    shopHaveCremeBrulee = false; 
                    UpdateUpgradeUI(RefundValue + 1); 
                } 
            }
            shopHaveFlan = false;
        }
        if (RefundValue == 104)
        {
            if (shopHaveCremeBrulee == false) { StartCoroutine(NotifTextHaveNotBought()); }
            else { shopWheat += upgradePrices[5]; }
            shopHaveCremeBrulee = false;
        }
        if (RefundValue == 105)
        {
            if (shopHaveBanhmi == false) { StartCoroutine(NotifTextHaveNotBought()); }
            else { shopWheat += upgradePrices[5]; }
            shopHaveBanhmi = false;
        }
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
            if (shopMaxHealth == 15) { UpgradesImage[2].sprite = UpgradesSpritesVariants[12]; upgradePriceText[2].text = upgradePrices[0].ToString(); }
            else if (shopMaxHealth == 25) { UpgradesImage[2].sprite = UpgradesSpritesVariants[13]; upgradePriceText[2].text = upgradePrices[1].ToString();}
            else if (shopMaxHealth == 45) { UpgradesImage[2].sprite = UpgradesSpritesVariants[14]; upgradePriceText[2].text = upgradePrices[2].ToString();}
            else if (shopMaxHealth == 70) { UpgradesImage[2].sprite = UpgradesSpritesVariants[15]; upgradePriceText[2].text = upgradePrices[3].ToString();}
            else if (shopMaxHealth == 100) { UpgradesImage[2].sprite = UpgradesSpritesVariants[16]; upgradePriceText[2].text = upgradePrices[4].ToString();}
            else if (shopMaxHealth == 150) { UpgradesImage[2].sprite = UpgradesSpritesVariants[17]; upgradePriceText[2].text = MaxBought;}
            else { Debug.Log("Error in Updating UI");}
        }
        if (UpdateUpgradeUI == 3 || UpdateUpgradeUI == 420)
        {
            if (shopMoveSpeed < 2.01) { UpgradesImage[3].sprite = UpgradesSpritesVariants[18]; upgradePriceText[3].text = upgradePrices[0].ToString(); }
            else if (shopMoveSpeed >= 2.24 && shopMoveSpeed < 2.26 ) { UpgradesImage[3].sprite = UpgradesSpritesVariants[19]; upgradePriceText[3].text = upgradePrices[1].ToString();}
            else if (shopMoveSpeed >= 2.49 && shopMoveSpeed < 2.51 ) { UpgradesImage[3].sprite = UpgradesSpritesVariants[20]; upgradePriceText[3].text = upgradePrices[2].ToString();}
            else if (shopMoveSpeed >= 2.74 && shopMoveSpeed < 2.76 ) { UpgradesImage[3].sprite = UpgradesSpritesVariants[21]; upgradePriceText[3].text = upgradePrices[3].ToString();}
            else if (shopMoveSpeed >= 2.99 && shopMoveSpeed < 3.01 ) { UpgradesImage[3].sprite = UpgradesSpritesVariants[22]; upgradePriceText[3].text = upgradePrices[4].ToString();}
            else if (shopMoveSpeed >= 3.49) { UpgradesImage[3].sprite = UpgradesSpritesVariants[23]; upgradePriceText[3].text = MaxBought;}
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
            if (shopDiceNumber == 3) { UpgradesImage[5].sprite = UpgradesSpritesVariants[30]; upgradePriceText[5].text = upgradePrices[0].ToString(); }
            else if (shopDiceNumber == 4) { UpgradesImage[5].sprite = UpgradesSpritesVariants[31]; upgradePriceText[5].text = upgradePrices[1].ToString();}
            else if (shopDiceNumber == 6) { UpgradesImage[5].sprite = UpgradesSpritesVariants[32]; upgradePriceText[5].text = upgradePrices[2].ToString();}
            else if (shopDiceNumber == 8) { UpgradesImage[5].sprite = UpgradesSpritesVariants[33]; upgradePriceText[5].text = upgradePrices[3].ToString();}
            else if (shopDiceNumber == 11) { UpgradesImage[5].sprite = UpgradesSpritesVariants[34]; upgradePriceText[5].text = upgradePrices[4].ToString();}
            else if (shopDiceNumber == 15) { UpgradesImage[5].sprite = UpgradesSpritesVariants[35]; upgradePriceText[5].text = MaxBought;}
            else { Debug.Log("Error in Updating UI");}
        }
        if (UpdateUpgradeUI == 6 || UpdateUpgradeUI == 420)
        {
            if (shopWheatDroprate == 50.0) { UpgradesImage[6].sprite = UpgradesSpritesVariants[36]; upgradePriceText[6].text = upgradePrices[0].ToString(); }
            else if (shopWheatDroprate == 60.0) { UpgradesImage[6].sprite = UpgradesSpritesVariants[37]; upgradePriceText[6].text = upgradePrices[1].ToString();}
            else if (shopWheatDroprate == 70.0) { UpgradesImage[6].sprite = UpgradesSpritesVariants[38]; upgradePriceText[6].text = upgradePrices[2].ToString();}
            else if (shopWheatDroprate == 80.0) { UpgradesImage[6].sprite = UpgradesSpritesVariants[39]; upgradePriceText[6].text = upgradePrices[3].ToString();}
            else if (shopWheatDroprate == 90.0) { UpgradesImage[6].sprite = UpgradesSpritesVariants[40]; upgradePriceText[6].text = upgradePrices[4].ToString();}
            else if (shopWheatDroprate == 100.0) { UpgradesImage[6].sprite = UpgradesSpritesVariants[41]; upgradePriceText[6].text = MaxBought;}
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
            else { UpgradesImage[10].sprite = UpgradesSpritesVariants[52]; upgradePriceText[10].text = upgradePrices[5].ToString(); }
        }
        if (UpdateUpgradeUI == 104 || UpdateUpgradeUI == 420)
        {
            if (shopHaveCremeBrulee) { UpgradesImage[11].sprite = UpgradesSpritesVariants[55]; upgradePriceText[11].text = MaxBought; }
            else { UpgradesImage[11].sprite = UpgradesSpritesVariants[54]; upgradePriceText[11].text = upgradePrices[5].ToString(); }
        }
        if (UpdateUpgradeUI == 105 || UpdateUpgradeUI == 420)
        {
            if (shopHaveBanhmi) { UpgradesImage[12].sprite = UpgradesSpritesVariants[57]; upgradePriceText[12].text = MaxBought; }
            else { UpgradesImage[12].sprite = UpgradesSpritesVariants[56]; upgradePriceText[12].text = upgradePrices[5].ToString(); }
        }
    }

    IEnumerator NotifTextBuySuccess()
    {
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

    IEnumerator NotifTextNeedFlan()
    {
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
        shopHavePizza = loadedPlayerData.havePizza;
        shopHaveCarrotCake = loadedPlayerData.haveCarrotCake;
        shopHaveFlan = loadedPlayerData.haveFlan;
        shopHaveCremeBrulee = loadedPlayerData.haveCremeBrulee;
        shopHaveBanhmi = loadedPlayerData.haveBanhmi;

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
        savingPlayerData.havePizza = shopHavePizza;
        savingPlayerData.haveCarrotCake = shopHaveCarrotCake;
        savingPlayerData.haveFlan = shopHaveFlan;
        savingPlayerData.haveCremeBrulee = shopHaveCremeBrulee;
        savingPlayerData.haveBanhmi = shopHaveBanhmi;

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
        public bool havePizza;
        public bool haveCarrotCake;
        public bool haveFlan;
        public bool haveCremeBrulee;
        public bool haveBanhmi;
    }
}

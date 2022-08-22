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

    public Text ShopWheatCounterNumber;
    public int[] upgradePrices;
    public Text[] upgradePriceText;
    public Image[] UpgradesImage;
    public Sprite[] UpgradesSpritesVariants;
    public Text NotifText;

    private bool NotifTextBuySuccessDisplayed = false;
    private bool NotifTextNotEnoughMoneyDisplayed = false;
    private bool NotifTextAlreadyBoughtDisplayed = false;
    private bool NotifTextHaveNotBoughtDisplayed = false;

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
            UpdateUpgradeUI(UpgradeValue);   
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
            UpdateUpgradeUI(UpgradeValue);   
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
            UpdateUpgradeUI(UpgradeValue);   
        }
        if (UpgradeValue == 3)
        {
            if (shopMoveSpeed < 1.09 && shopWheat >= upgradePrices[0]) { shopMoveSpeed = 1.1f; shopWheat -= upgradePrices[0]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopMoveSpeed >= 1.09 && shopMoveSpeed < 1.11 && shopWheat >= upgradePrices[1]) { shopMoveSpeed = 1.2f; shopWheat -= upgradePrices[1]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopMoveSpeed >= 1.19 && shopMoveSpeed < 1.21 && shopWheat >= upgradePrices[2]) { shopMoveSpeed = 1.3f; shopWheat -= upgradePrices[2]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopMoveSpeed >= 1.29 && shopMoveSpeed < 1.31 && shopWheat >= upgradePrices[3]) { shopMoveSpeed = 1.4f; shopWheat -= upgradePrices[3]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopMoveSpeed >= 1.39 && shopMoveSpeed < 1.41 && shopWheat >= upgradePrices[4]) { shopMoveSpeed = 1.5f; shopWheat -= upgradePrices[4]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopMoveSpeed >= 1.41) { StartCoroutine(NotifTextAlreadyBought()); }
            else { StartCoroutine(NotifTextNotEnoughMoney()); }
            UpdateUpgradeUI(UpgradeValue);   
        }
        if (UpgradeValue == 4)
        {
            if (shopPlayerCooldownTime >= 1.0 && shopWheat >= upgradePrices[0]) { shopPlayerCooldownTime = 0.9f; shopWheat -= upgradePrices[0]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopPlayerCooldownTime <= 0.91 && shopPlayerCooldownTime >= 0.89 && shopWheat >= upgradePrices[1]) { shopPlayerCooldownTime = 0.8f; shopWheat -= upgradePrices[1]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopPlayerCooldownTime <= 0.81 && shopPlayerCooldownTime >= 0.79 && shopWheat >= upgradePrices[2]) { shopPlayerCooldownTime = 0.7f; shopWheat -= upgradePrices[2]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopPlayerCooldownTime <= 0.71 && shopPlayerCooldownTime >= 0.69 && shopWheat >= upgradePrices[3]) { shopPlayerCooldownTime = 0.6f; shopWheat -= upgradePrices[3]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopPlayerCooldownTime <= 0.61 && shopPlayerCooldownTime >= 0.59 && shopWheat >= upgradePrices[4]) { shopPlayerCooldownTime = 0.5f; shopWheat -= upgradePrices[4]; StartCoroutine(NotifTextBuySuccess()); }
            else if (shopPlayerCooldownTime <= 0.5) { StartCoroutine(NotifTextAlreadyBought()); }
            else { StartCoroutine(NotifTextNotEnoughMoney()); }
            UpdateUpgradeUI(UpgradeValue);   
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
            UpdateUpgradeUI(UpgradeValue);   
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
            UpdateUpgradeUI(UpgradeValue);   
        }
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
            UpdateUpgradeUI(RefundValue);
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
            UpdateUpgradeUI(RefundValue);
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
            UpdateUpgradeUI(RefundValue);
        }
        if (RefundValue == 3)
        {
            if (shopMoveSpeed < 1.09) { refundLoops = 0; StartCoroutine(NotifTextHaveNotBought()); }
            else if (shopMoveSpeed >= 1.09 && shopMoveSpeed < 1.11 ) { refundLoops = 1; }
            else if (shopMoveSpeed >= 1.19 && shopMoveSpeed < 1.21 ) { refundLoops = 2; }
            else if (shopMoveSpeed >= 1.29 && shopMoveSpeed < 1.31 ) { refundLoops = 3; }
            else if (shopMoveSpeed >= 1.39 && shopMoveSpeed < 1.41 ) { refundLoops = 4; }
            else if (shopMoveSpeed >= 1.49) { refundLoops = 5; }
            else { Debug.Log("Error In Refunding"); }
            for (int i = 0; i < refundLoops; i++) { shopWheat += upgradePrices[i]; }            
            shopMoveSpeed = 1.0f;
            UpdateUpgradeUI(RefundValue);
        }
        if (RefundValue == 4)
        {
            if (shopPlayerCooldownTime >= 1.0) { refundLoops = 0; StartCoroutine(NotifTextHaveNotBought()); }
            else if (shopPlayerCooldownTime <= 0.91 && shopPlayerCooldownTime >= 0.89) { refundLoops = 1; }
            else if (shopPlayerCooldownTime <= 0.81 && shopPlayerCooldownTime >= 0.79) { refundLoops = 2; }
            else if (shopPlayerCooldownTime <= 0.71 && shopPlayerCooldownTime >= 0.69) { refundLoops = 3; }
            else if (shopPlayerCooldownTime <= 0.61 && shopPlayerCooldownTime >= 0.59) { refundLoops = 4; }
            else if (shopPlayerCooldownTime <= 0.5) { refundLoops = 5; }
            else { Debug.Log("Error In Refunding"); }
            for (int i = 0; i < refundLoops; i++) { shopWheat += upgradePrices[i]; }            
            shopPlayerCooldownTime = 1.0f;
            UpdateUpgradeUI(RefundValue);
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
            UpdateUpgradeUI(RefundValue);
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
            UpdateUpgradeUI(RefundValue);
        }
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
            else if (shopStrength == 5) { UpgradesImage[0].sprite = UpgradesSpritesVariants[5]; upgradePriceText[0].text = "Maxed";}
            else { Debug.Log("Error in Updating UI");}
        }
        if (UpdateUpgradeUI == 1 || UpdateUpgradeUI == 420)
        {
            if (shopDefense == 1.0) { UpgradesImage[1].sprite = UpgradesSpritesVariants[6]; upgradePriceText[1].text = upgradePrices[0].ToString(); }
            else if (shopDefense == 1.5) { UpgradesImage[1].sprite = UpgradesSpritesVariants[7]; upgradePriceText[1].text = upgradePrices[1].ToString();}
            else if (shopDefense == 2) { UpgradesImage[1].sprite = UpgradesSpritesVariants[8]; upgradePriceText[1].text = upgradePrices[2].ToString();}
            else if (shopDefense == 3) { UpgradesImage[1].sprite = UpgradesSpritesVariants[9]; upgradePriceText[1].text = upgradePrices[3].ToString();}
            else if (shopDefense == 4) { UpgradesImage[1].sprite = UpgradesSpritesVariants[10]; upgradePriceText[1].text = upgradePrices[4].ToString();}
            else if (shopDefense == 5) { UpgradesImage[1].sprite = UpgradesSpritesVariants[11]; upgradePriceText[1].text = "Maxed";}
            else { Debug.Log("Error in Updating UI");}
        }
        if (UpdateUpgradeUI == 2 || UpdateUpgradeUI == 420)
        {
            if (shopMaxHealth == 15) { UpgradesImage[2].sprite = UpgradesSpritesVariants[12]; upgradePriceText[2].text = upgradePrices[0].ToString(); }
            else if (shopMaxHealth == 25) { UpgradesImage[2].sprite = UpgradesSpritesVariants[13]; upgradePriceText[2].text = upgradePrices[1].ToString();}
            else if (shopMaxHealth == 45) { UpgradesImage[2].sprite = UpgradesSpritesVariants[14]; upgradePriceText[2].text = upgradePrices[2].ToString();}
            else if (shopMaxHealth == 70) { UpgradesImage[2].sprite = UpgradesSpritesVariants[15]; upgradePriceText[2].text = upgradePrices[3].ToString();}
            else if (shopMaxHealth == 100) { UpgradesImage[2].sprite = UpgradesSpritesVariants[16]; upgradePriceText[2].text = upgradePrices[4].ToString();}
            else if (shopMaxHealth == 150) { UpgradesImage[2].sprite = UpgradesSpritesVariants[17]; upgradePriceText[2].text = "Maxed";}
            else { Debug.Log("Error in Updating UI");}
        }
        if (UpdateUpgradeUI == 3 || UpdateUpgradeUI == 420)
        {
            if (shopMoveSpeed < 1.09) { UpgradesImage[3].sprite = UpgradesSpritesVariants[18]; upgradePriceText[3].text = upgradePrices[0].ToString(); }
            else if (shopMoveSpeed >= 1.09 && shopMoveSpeed < 1.11 ) { UpgradesImage[3].sprite = UpgradesSpritesVariants[19]; upgradePriceText[3].text = upgradePrices[1].ToString();}
            else if (shopMoveSpeed >= 1.19 && shopMoveSpeed < 1.21 ) { UpgradesImage[3].sprite = UpgradesSpritesVariants[20]; upgradePriceText[3].text = upgradePrices[2].ToString();}
            else if (shopMoveSpeed >= 1.29 && shopMoveSpeed < 1.31 ) { UpgradesImage[3].sprite = UpgradesSpritesVariants[21]; upgradePriceText[3].text = upgradePrices[3].ToString();}
            else if (shopMoveSpeed >= 1.39 && shopMoveSpeed < 1.41 ) { UpgradesImage[3].sprite = UpgradesSpritesVariants[22]; upgradePriceText[3].text = upgradePrices[4].ToString();}
            else if (shopMoveSpeed >= 1.49) { UpgradesImage[3].sprite = UpgradesSpritesVariants[23]; upgradePriceText[3].text = "Maxed";}
            else { Debug.Log("Error in Updating UI");}
        }
        if (UpdateUpgradeUI == 4 || UpdateUpgradeUI == 420)
        {
            if (shopPlayerCooldownTime >= 1.0) { UpgradesImage[4].sprite = UpgradesSpritesVariants[24]; upgradePriceText[4].text = upgradePrices[0].ToString(); }
            else if (shopPlayerCooldownTime <= 0.91 && shopPlayerCooldownTime >= 0.89) { UpgradesImage[4].sprite = UpgradesSpritesVariants[25]; upgradePriceText[4].text = upgradePrices[1].ToString();}
            else if (shopPlayerCooldownTime <= 0.81 && shopPlayerCooldownTime >= 0.79) { UpgradesImage[4].sprite = UpgradesSpritesVariants[26]; upgradePriceText[4].text = upgradePrices[2].ToString();}
            else if (shopPlayerCooldownTime <= 0.71 && shopPlayerCooldownTime >= 0.69) { UpgradesImage[4].sprite = UpgradesSpritesVariants[27]; upgradePriceText[4].text = upgradePrices[3].ToString();}
            else if (shopPlayerCooldownTime <= 0.61 && shopPlayerCooldownTime >= 0.59) { UpgradesImage[4].sprite = UpgradesSpritesVariants[28]; upgradePriceText[4].text = upgradePrices[4].ToString();}
            else if (shopPlayerCooldownTime <= 0.5) { UpgradesImage[4].sprite = UpgradesSpritesVariants[29]; upgradePriceText[4].text = "Maxed";}
            else { Debug.Log("Error in Updating UI");}
        }
        if (UpdateUpgradeUI == 5 || UpdateUpgradeUI == 420)
        {
            if (shopDiceNumber == 3) { UpgradesImage[5].sprite = UpgradesSpritesVariants[30]; upgradePriceText[5].text = upgradePrices[0].ToString(); }
            else if (shopDiceNumber == 4) { UpgradesImage[5].sprite = UpgradesSpritesVariants[31]; upgradePriceText[5].text = upgradePrices[1].ToString();}
            else if (shopDiceNumber == 6) { UpgradesImage[5].sprite = UpgradesSpritesVariants[32]; upgradePriceText[5].text = upgradePrices[2].ToString();}
            else if (shopDiceNumber == 8) { UpgradesImage[5].sprite = UpgradesSpritesVariants[33]; upgradePriceText[5].text = upgradePrices[3].ToString();}
            else if (shopDiceNumber == 11) { UpgradesImage[5].sprite = UpgradesSpritesVariants[34]; upgradePriceText[5].text = upgradePrices[4].ToString();}
            else if (shopDiceNumber == 15) { UpgradesImage[5].sprite = UpgradesSpritesVariants[35]; upgradePriceText[5].text = "Maxed";}
            else { Debug.Log("Error in Updating UI");}
        }
        if (UpdateUpgradeUI == 6 || UpdateUpgradeUI == 420)
        {
            if (shopWheatDroprate == 50.0) { UpgradesImage[6].sprite = UpgradesSpritesVariants[36]; upgradePriceText[6].text = upgradePrices[0].ToString(); }
            else if (shopWheatDroprate == 60.0) { UpgradesImage[6].sprite = UpgradesSpritesVariants[37]; upgradePriceText[6].text = upgradePrices[1].ToString();}
            else if (shopWheatDroprate == 70.0) { UpgradesImage[6].sprite = UpgradesSpritesVariants[38]; upgradePriceText[6].text = upgradePrices[2].ToString();}
            else if (shopWheatDroprate == 80.0) { UpgradesImage[6].sprite = UpgradesSpritesVariants[39]; upgradePriceText[6].text = upgradePrices[3].ToString();}
            else if (shopWheatDroprate == 90.0) { UpgradesImage[6].sprite = UpgradesSpritesVariants[40]; upgradePriceText[6].text = upgradePrices[4].ToString();}
            else if (shopWheatDroprate == 100.0) { UpgradesImage[6].sprite = UpgradesSpritesVariants[41]; upgradePriceText[6].text = "Maxed";}
            else { Debug.Log("Error in Updating UI");}
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
            NotifText.color = new Color32(255, 0, 0, 255);
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
            NotifText.color = new Color32(255, 0, 0, 255);
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
            NotifText.color = new Color32(255, 0, 0, 255);
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
    }
}

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
            shopWheat += 5;
            ShopWheatCounterNumber.text = shopWheat.ToString();
            SaveData();
        } else if (Input.GetKeyDown(KeyCode.R))
        {
            shopWheat -= 5;
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
            UpdateUpgradeUI(0);   
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
            UpdateUpgradeUI(1);   
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
            UpdateUpgradeUI(0);
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
            UpdateUpgradeUI(1);
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
    }
}

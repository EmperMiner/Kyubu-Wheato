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
    public Text ShopWheatCounterNumber;

    public int[] upgradePrices;
    public Text[] upgradePriceText;

    private void Start()
    {
        LoadData();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            shopWheat += 1;
            ShopWheatCounterNumber.text = shopWheat.ToString();
            SaveData();
        } else if (Input.GetKeyDown(KeyCode.R))
        {
            shopWheat -= 1;
            ShopWheatCounterNumber.text = shopWheat.ToString();
            SaveData();
        }
        if (shopStrength == 1.0) {}
    }

    public void Buy(int UpgradeValue)
    {
        if (UpgradeValue == 0)
        {
            if (shopStrength == 1.0 && shopWheat >= upgradePrices[0])
            {
                shopStrength = 1.5f;
                shopWheat -= upgradePrices[0];
            }
            else if (shopStrength == 1.5 && shopWheat >= upgradePrices[1])
            {
                shopStrength = 2f;
                shopWheat -= upgradePrices[1];
            }
            else if (shopStrength == 2 && shopWheat >= upgradePrices[2])
            {
                shopStrength = 2.5f;
                shopWheat -= upgradePrices[2];
            }
            else if (shopStrength == 2.5 && shopWheat >= upgradePrices[3])
            {
                shopStrength = 3f;
                shopWheat -= upgradePrices[3];
            }
            else if (shopStrength == 3 && shopWheat >= upgradePrices[4])
            {
                shopStrength = 5f;
                shopWheat -= upgradePrices[4];
            }
            else if (shopStrength == 5)
            {
                Debug.Log("Maximum Strength Upgrade Unlocked");
            }
            else 
            {
                Debug.Log("You Do Not Have Enough Wheat!");
            }
            ShopWheatCounterNumber.text = shopWheat.ToString();
            SaveData();
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
    }
}

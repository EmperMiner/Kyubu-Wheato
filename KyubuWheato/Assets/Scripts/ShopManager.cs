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

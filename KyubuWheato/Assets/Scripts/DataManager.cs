using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private PlayerController player;
    private DiceThrow diceManager;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        diceManager =  GameObject.FindGameObjectWithTag("DiceManager").GetComponent<DiceThrow>();
        
        PlayerData playerData = new PlayerData();
        playerData.MoveSpeed = player.MoveSpeed;
        playerData.maxHealth = player.maxHealth;
        playerData.playerHealth = player.playerHealth;
        playerData.strength = player.strength;
        playerData.Wheat = player.Wheat;
        playerData.diceNumber = diceManager.diceNumber;
        playerData.playerCooldownTime = diceManager.playerCooldownTime;

        string json = JsonUtility.ToJson(playerData);
        Debug.Log(json);

        File.WriteAllText(Application.dataPath + "/gameSaveData.json", json);        
    }

    private void ReadSaveData()
    {
        string json = File.ReadAllText(Application.dataPath + "/gameSaveData.json");
        PlayerData loadedPlayerData = JsonUtility.FromJson<PlayerData>(json);
        Debug.Log("MoveSpeed: " + loadedPlayerData.MoveSpeed);
        Debug.Log("Current Health: " + loadedPlayerData.playerHealth);
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

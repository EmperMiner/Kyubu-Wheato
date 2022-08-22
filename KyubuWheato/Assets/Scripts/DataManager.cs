using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private PlayerController player;
    private DiceThrow diceManager;

    void Start()
    {
        
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

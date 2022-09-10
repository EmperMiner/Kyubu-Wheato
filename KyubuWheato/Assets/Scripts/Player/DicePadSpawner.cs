using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DicePadSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] DicePadTypes;
    [SerializeField] private float LeftMapLimit;
    [SerializeField] private float RightMapLimit;
    [SerializeField] private float UpperMapLimit;
    [SerializeField] private float LowerMapLimit;

    private bool haveChickenNuggets;
    private bool triggeredSpawnPadCycle = false;

    void Start()
    {
        LoadData();
    }

    
    void Update()
    {
        if (haveChickenNuggets && triggeredSpawnPadCycle == false) { StartCoroutine(SpawnPadCycle()); }
    }

    IEnumerator SpawnPadCycle()
    {
        triggeredSpawnPadCycle = true;
        yield return new WaitForSeconds(Random.Range(15f, 45f));
        SpawnPad();
        StartCoroutine(SpawnPadCycle());
    }

    private void SpawnPad()
    {
        Debug.Log("Spawned Pad");
        Instantiate(DicePadTypes[Random.Range(0,DicePadTypes.Length)], new Vector3(Random.Range(LeftMapLimit,RightMapLimit), Random.Range(LowerMapLimit, UpperMapLimit), 0), Quaternion.identity);
    }

    private void LoadData()
    {
        string json = File.ReadAllText(Application.dataPath + "/gameSaveData.json");
        PlayerData loadedPlayerData = JsonUtility.FromJson<PlayerData>(json);
        
        haveChickenNuggets = loadedPlayerData.haveChickenNuggets;
    }   

    private class PlayerData
    {
        public bool haveChickenNuggets;
    }
}

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
        DicePadLoadData();
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
        Instantiate(DicePadTypes[Random.Range(0,DicePadTypes.Length)], new Vector3(Random.Range(LeftMapLimit,RightMapLimit), Random.Range(LowerMapLimit, UpperMapLimit), 0), Quaternion.identity);
    }

    public void DicePadLoadData()
    {
        string json = File.ReadAllText(Application.dataPath + "/ingameSaveData.json");
        PlayerData loadedPlayerData = JsonUtility.FromJson<PlayerData>(json);
        
        haveChickenNuggets = loadedPlayerData.haveChickenNuggets;
    }   

    private class PlayerData
    {
        public bool haveChickenNuggets;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DicePadSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] DicePadTypes;
    private PlayerController player;

    private bool haveChickenNuggets;
    private bool triggeredSpawnPadCycle = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        DicePadLoadData();
    }

    
    private void Update()
    {
        if (haveChickenNuggets && triggeredSpawnPadCycle == false) { StartCoroutine(SpawnPadCycle()); }
    }

    IEnumerator SpawnPadCycle()
    {
        triggeredSpawnPadCycle = true;
        yield return new WaitForSeconds(Random.Range(35f, 70f));
        SpawnPad();
        StartCoroutine(SpawnPadCycle());
        yield return null;
    }

    private void SpawnPad()
    {
        Instantiate(DicePadTypes[Random.Range(0,DicePadTypes.Length)], new Vector3(Random.Range(player.LeftMapLimit,player.RightMapLimit), Random.Range(player.LowerMapLimit, player.UpperMapLimit), 0), Quaternion.identity);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TreasureChestSpawner : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private GameObject[] ChestTypes;
    [SerializeField] private int ChestLimit;
    public int ChestSpawned = 0;
    private bool triggeredChestSpawningCycle = false;
    
    private void Start() 
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (ChestSpawned < ChestLimit && triggeredChestSpawningCycle == false) { StartCoroutine(SpawnChestCycle()); }
    }

    private IEnumerator SpawnChestCycle()
    {
        triggeredChestSpawningCycle = true;
        if (SceneManager.GetActiveScene().buildIndex != 15) { yield return new WaitForSeconds(Random.Range(12f, 32f)); }
        else { yield return new WaitForSeconds(Random.Range(3f, 9f)); }
        SpawnChest();
        if (ChestSpawned < ChestLimit) { StartCoroutine(SpawnChestCycle()); }
        else { triggeredChestSpawningCycle = false; }
    }

    private void SpawnChest()
    {
        ChestSpawned++;
        Instantiate(ChestTypes[Random.Range(0,ChestTypes.Length)], new Vector3(Random.Range(player.LeftMapLimit,player.RightMapLimit), Random.Range(player.LowerMapLimit, player.UpperMapLimit), 0), Quaternion.identity);
    }
}

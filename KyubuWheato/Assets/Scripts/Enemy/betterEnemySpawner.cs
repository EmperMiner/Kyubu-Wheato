using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class betterEnemySpawner : MonoBehaviour
{
    [SerializeField] private float[] enemyIntervals;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private int[] enemyLimits;
    private int[] enemySpawned;

    private void Start()
    {
        enemySpawned = new int[enemyPrefabs.Length];
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            StartCoroutine(SpawnEnemy(enemyIntervals[i], enemyPrefabs[i], enemyLimits[i], i));
        }
    }

    private IEnumerator SpawnEnemy(float enemyInterval, GameObject enemy, int enemyLimit, int enemyIndex)
    {
        yield return new WaitForSeconds(enemyInterval);
        enemySpawned[enemyIndex]++;
        Instantiate(enemy, new Vector3(transform.position.x + Random.Range(-10f, 10f), transform.position.y + Random.Range(-10f, 10f), 0), Quaternion.identity);
        if (enemySpawned[enemyIndex] < enemyLimit) { StartCoroutine(SpawnEnemy(enemyInterval, enemy, enemyLimit, enemyIndex)); }
    }
}

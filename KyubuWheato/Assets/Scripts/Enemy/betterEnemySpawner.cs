using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class betterEnemySpawner : MonoBehaviour
{
    [SerializeField] private float[] enemyIntervals;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private int[] enemyLimits;
    private int[] enemySpawned;
    [SerializeField] private bool isSecretLevel;

    private GameObject[] spawnings;

    private void Start()
    {
        enemySpawned = new int[enemyPrefabs.Length];
        StartCoroutine(SpawnEnemyStartingDelay());
        if (isSecretLevel) { StartCoroutine(SpawnEnemyInCircle()); }
        spawnings = GameObject.FindGameObjectsWithTag("MobSpawner");
    }

    private IEnumerator SpawnEnemy(float enemyInterval, GameObject enemy, int enemyLimit, int enemyIndex)
    {
        yield return new WaitForSeconds(enemyInterval);
        enemySpawned[enemyIndex]++;
        int rand = Random.Range(0,3);
        if (rand > 0 || SceneManager.GetActiveScene().buildIndex == 15) { Instantiate(enemy, new Vector3(transform.position.x + Random.Range(-10f, 10f), transform.position.y + Random.Range(-10f, 10f), 0), Quaternion.identity); }
        else 
        { 
            Vector3 hi = spawnings[Random.Range(0, spawnings.Length)].transform.position;
            Instantiate(enemy, new Vector3(hi.x, hi.y, 0), Quaternion.identity); 
        }

        if (enemySpawned[enemyIndex] < enemyLimit) { StartCoroutine(SpawnEnemy(enemyInterval, enemy, enemyLimit, enemyIndex)); }
    }

    private IEnumerator SpawnEnemyStartingDelay()
    {
        yield return new WaitForSeconds(15f);
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            StartCoroutine(SpawnEnemy(enemyIntervals[i], enemyPrefabs[i], enemyLimits[i], i));
        }
        yield return null;
    }

    private IEnumerator SpawnEnemyInCircle()
    {
        yield return new WaitForSeconds(Random.Range(200f, 300f));
        for (int i =  0; i < 6; i++)
        {
            Instantiate(enemyPrefabs[i], new Vector3(transform.position.x + Random.Range(-15f, 15f), transform.position.y + Random.Range(-15f, 15f), 0), Quaternion.identity);
        }
        StartCoroutine(SpawnEnemyInCircle());
        yield return null;
    }
}

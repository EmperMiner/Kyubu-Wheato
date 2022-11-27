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
    [SerializeField] private GameObject ghost;
    private ExitHoeContainer exitScript;

    private GameObject[] spawnings;

    private void Start()
    {
        exitScript = GameObject.FindGameObjectWithTag("ExitHoeContainer").GetComponent<ExitHoeContainer>();
        enemySpawned = new int[enemyPrefabs.Length];
        StartCoroutine(SpawnEnemyStartingDelay());
        if (isSecretLevel) { StartCoroutine(SpawnEnemyInCircle()); }
        spawnings = GameObject.FindGameObjectsWithTag("MobSpawner");

        int ghostDecider = Random.Range(0,5);
        if (ghostDecider == 0) { StartCoroutine(spawnGhosts()); }
    }

    private IEnumerator SpawnEnemy(float enemyInterval, GameObject enemy, int enemyLimit, int enemyIndex)
    {
        yield return new WaitForSeconds(Random.Range(enemyInterval*0.25f, enemyInterval*1.25f));
        enemySpawned[enemyIndex]++;
        int rand = Random.Range(0,4);
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
        yield return new WaitForSeconds(5f);
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            StartCoroutine(SpawnEnemy(enemyIntervals[i], enemyPrefabs[i], enemyLimits[i], i));
        }
        yield return null;
    }

    private IEnumerator spawnGhosts()
    {
        yield return new WaitForSeconds(Random.Range(50f, 100f));

        for (int i = 0; i < Random.Range(1, 9); i++) { Instantiate(ghost, new Vector3(transform.position.x + Random.Range(-10f, 10f), transform.position.y + Random.Range(-10f, 10f), 0), Quaternion.identity); }

        if (exitScript.EnemiesKilled < exitScript.EnemyLimit) { StartCoroutine(spawnGhosts()); }
        yield return null;
    }
}

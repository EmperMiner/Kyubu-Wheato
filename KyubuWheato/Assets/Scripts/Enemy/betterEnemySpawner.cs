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
    [SerializeField] private GameObject ghost;
    [SerializeField] private GameObject[] flippedEnemyPrefabs;
    private ExitHoeContainer exitScript;

    private GameObject[] spawnings;

    [SerializeField] private float spiderInterval;
    [SerializeField] private GameObject spiderPrefab;
    [SerializeField] private int spiderLimit;
    private int spiderSpawned;

    private void Start()
    {
        PlayerPrefs.SetInt("BossDefeated", 0);
        exitScript = GameObject.FindGameObjectWithTag("ExitHoeContainer").GetComponent<ExitHoeContainer>();
        enemySpawned = new int[enemyPrefabs.Length];
        StartCoroutine(SpawnEnemyStartingDelay());
        spawnings = GameObject.FindGameObjectsWithTag("MobSpawner");

        int ghostDecider = Random.Range(0,5);
        if (ghostDecider == 0 && PlayerPrefs.GetInt("WinCounter") > 0) { StartCoroutine(spawnGhosts()); }

        if (PlayerPrefs.GetInt("WinCounter") > 0) { StartCoroutine(FlippedEnemyChance()); }

        if (SceneManager.GetActiveScene().buildIndex > 4 && SceneManager.GetActiveScene().buildIndex != 15) { StartCoroutine(SpawnSpooders());}
    }

    private IEnumerator SpawnSpooders()
    {
        yield return new WaitForSeconds(Random.Range(spiderInterval*0.25f, spiderInterval*1.25f));
        spiderSpawned++;
        if (PlayerPrefs.GetInt("BossDefeated") == 0) { Instantiate(spiderPrefab, transform.position, Quaternion.identity); }
        if (spiderSpawned < spiderLimit && PlayerPrefs.GetInt("BossDefeated") == 0) { StartCoroutine(SpawnSpooders()); }
        yield return null;
    }

    private IEnumerator SpawnEnemy(float enemyInterval, GameObject enemy, int enemyLimit, int enemyIndex)
    {
        yield return new WaitForSeconds(Random.Range(enemyInterval*0.25f, enemyInterval*1.25f));
        enemySpawned[enemyIndex]++;
        int rand = Random.Range(0,3);
        if (rand > 0 || SceneManager.GetActiveScene().buildIndex == 15) 
        { 
            float RandomXOffset;
            float RandomYOffset;   

            int rand1 = Random.Range(0,2);
            if (rand1 == 0) { RandomXOffset = Random.Range(-10f,-3f); }
            else { RandomXOffset = Random.Range(3f,10f); }

            int rand2 = Random.Range(0,2);
            if (rand2 == 0) { RandomYOffset = Random.Range(-10f,-3f); }
            else { RandomYOffset = Random.Range(3f,10f); }

            if (PlayerPrefs.GetInt("BossDefeated") == 0) { Instantiate(enemy, new Vector3(transform.position.x + RandomXOffset, transform.position.y + RandomYOffset, 0), Quaternion.identity); }
        }
        else 
        { 
            if (PlayerPrefs.GetInt("BossDefeated") == 0)
            {
                Vector3 hi = spawnings[Random.Range(0, spawnings.Length)].transform.position;
                Instantiate(enemy, new Vector3(hi.x, hi.y, 0), Quaternion.identity); 
            }
        }

        if (enemySpawned[enemyIndex] < enemyLimit && PlayerPrefs.GetInt("BossDefeated") == 0) { StartCoroutine(SpawnEnemy(enemyInterval, enemy, enemyLimit, enemyIndex)); }
    }

    private IEnumerator SpawnEnemyStartingDelay()
    {
        yield return new WaitForSeconds(5f);
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            if (PlayerPrefs.GetInt("BossDefeated") == 0) { StartCoroutine(SpawnEnemy(enemyIntervals[i]*1.5f, enemyPrefabs[i], enemyLimits[i], i)); }
        }
        yield return null;
    }

    private IEnumerator spawnGhosts()
    {
        yield return new WaitForSeconds(Random.Range(50f, 100f));

        if (PlayerPrefs.GetInt("BossDefeated") == 0) 
        {
            for (int i = 0; i < Random.Range(1, 9); i++) { Instantiate(ghost, new Vector3(transform.position.x + Random.Range(-10f, 10f), transform.position.y + Random.Range(-10f, 10f), 0), Quaternion.identity); }
        }

        if (exitScript.EnemiesKilled < exitScript.EnemyLimit && PlayerPrefs.GetInt("BossDefeated") == 0) { StartCoroutine(spawnGhosts()); }
        yield return null;
    }

    //more wins, more flipped enemies
    private IEnumerator FlippedEnemyChance()
    {
        float reducedFlippedSpawningCooldown = 180f - PlayerPrefs.GetInt("WinCounter")*Random.Range(2f, 5f);
        if (reducedFlippedSpawningCooldown < 100f) { reducedFlippedSpawningCooldown = 100f; }

        yield return new WaitForSeconds(UnityEngine.Random.Range(100f, reducedFlippedSpawningCooldown));

        int increasedFlippedEnemyChance = 20 - PlayerPrefs.GetInt("WinCounter");
        if (increasedFlippedEnemyChance < 0) { increasedFlippedEnemyChance = 0; }
        int flippedChance = UnityEngine.Random.Range(0, increasedFlippedEnemyChance);
        if  (flippedChance == 0 && PlayerPrefs.GetInt("BossDefeated") == 0)
        {
            StartCoroutine(SummonFlippedEnemy());
            StartCoroutine(FlippedEnemyChance());
        }
        yield return null;
    }

    //spawn flipped enemies on repeated wins
    private IEnumerator SummonFlippedEnemy()
    {
        FindObjectOfType<AudioManager>().PlaySound("FlippedSpawned" + UnityEngine.Random.Range(1,6));
        for (int i = 0; i < Mathf.FloorToInt(PlayerPrefs.GetInt("WinCounter")/2); i++)
        {
            Instantiate(flippedEnemyPrefabs[Random.Range(0,flippedEnemyPrefabs.Length)], new Vector3(transform.position.x + Random.Range(-10f, 10f), transform.position.y + Random.Range(-10f, 10f), 0), Quaternion.identity);
        }
        yield return null;
    }

}

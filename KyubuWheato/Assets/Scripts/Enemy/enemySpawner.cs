using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemy;
    [SerializeField] private int[] enemyLimit;
    [SerializeField] private float[] enemyInterval;
    [SerializeField] private int waveNumbers;
    [SerializeField] private float[] waveInterval;

    private int wave = 0;
    private int enemyCounter = 0;
    
    void Start()
    {
        StartCoroutine(callWave());
    }

    private IEnumerator spawnEnemy()
    {       
        Instantiate(enemy[wave], transform.position, Quaternion.identity);
        yield return new WaitForSeconds(enemyInterval[wave]);
        enemyCounter++;
        if ( enemyCounter < enemyLimit[wave]) { StartCoroutine(spawnEnemy()); }
        else { wave++; StartCoroutine(callWave()); }
    }

    private IEnumerator callWave()
    {
        yield return new WaitForSeconds(waveInterval[wave]);
        if (wave < waveNumbers) { enemyCounter = 0; StartCoroutine(spawnEnemy()); }
        else { Destroy(gameObject); } 
        yield return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject mousePrefab;
    [SerializeField] private int enemyLimit = 5;
    [SerializeField] private float mouseInterval = 4f;

    int i = 0;
    
    void Start()
    {
        StartCoroutine(spawnEnemy(mouseInterval, mousePrefab));
    }

    // Update is called once per frame
    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {       
        GameObject newEnemy = Instantiate(mousePrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(interval);
        i++;
        if ( i < enemyLimit ) { StartCoroutine(spawnEnemy(interval, enemy)); }
    }
}

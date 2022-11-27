using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EZCameraShake;

public class whaleSummon : MonoBehaviour
{
    [SerializeField] private int obstacleChance;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject[] flippedEnemyPrefabs;
    [SerializeField] private GameObject waterSplash;
    [SerializeField] private GameObject[] obstacles;
    private int enemyLimiter;
    private Animator water;
    void Start()
    {
        water = GameObject.Find("Water").GetComponent<Animator>();
        enemyLimiter = 0;
        if (SceneManager.GetActiveScene().buildIndex < 4) { enemyLimiter += 1; }
        if (SceneManager.GetActiveScene().buildIndex < 5) { enemyLimiter += 2; }
        if (SceneManager.GetActiveScene().buildIndex < 6) { enemyLimiter += 2; }
        if (SceneManager.GetActiveScene().buildIndex < 7) { enemyLimiter += 1; }
        if (SceneManager.GetActiveScene().buildIndex < 9) { enemyLimiter += 1; }
        if (SceneManager.GetActiveScene().buildIndex < 10) { enemyLimiter += 2; }

        StartCoroutine(WhaleAnim());
    }

    private IEnumerator WhaleAnim()
    {
        yield return new WaitForSeconds(1.5f);
        FindObjectOfType<AudioManager>().PlaySound("WhaleLaugh");
        yield return new WaitForSeconds(1.83f);
        CameraShaker.Instance.ShakeOnce(7f, 7f, .5f, 2.5f);
        water.SetTrigger("Underwater");
        Instantiate(waterSplash, transform.position, Quaternion.identity);
        FindObjectOfType<AudioManager>().PlaySound("WaterSplash");
        WhaleSummon(1);
        yield return null;
    }

    public void WhaleSummon(int FlippedEnemies)
    {
        if (FlippedEnemies == 0) { summonFlippedEnemies(); }
        else { summonEnemies(); }
    }

    private void summonEnemies()
    {
        int level = SceneManager.GetActiveScene().buildIndex - 3;
        for (int i = 0; i < (level*3 + 4); i++)
        {
            Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length - enemyLimiter)], new Vector3(transform.position.x + Random.Range(-11f, 11f), transform.position.y + Random.Range(-11f, 11f), 0), Quaternion.identity);
        }
    }

    private void summonFlippedEnemies()
    {
        int level = SceneManager.GetActiveScene().buildIndex - 3;
        for (int i = 0; i < 3 + Mathf.RoundToInt(level*1/4); i++)
        {
            Instantiate(flippedEnemyPrefabs[Random.Range(0, flippedEnemyPrefabs.Length)], new Vector3(transform.position.x + Random.Range(-11f, 11f), transform.position.y + Random.Range(-11f, 11f), 0), Quaternion.identity);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderSpawnRandom : MonoBehaviour
{
    [SerializeField] private GameObject SpiderWarning;

    private void Start()
    {
        FindObjectOfType<AudioManager>().PlaySound("SpooderSpawned");
        for (int i = 0; i < 7; i++)
        {
            Instantiate(SpiderWarning, new Vector3(transform.position.x + Random.Range(-3f, 3f), transform.position.y + Random.Range(-3f, 3f), 0), Quaternion.identity);
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonBoss : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject GoldenWheat;
    [SerializeField] private ExitHoeContainer exitScript;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            exitScript.notBossFight = false;
            Instantiate(boss, transform.position, Quaternion.identity);
            GoldenWheat.SetActive(false);
            Destroy(gameObject);    
        }
    }
}
